using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Calc
{
	class Data
	{
		private static SortedList<string, Variable> _variables = new SortedList<string, Variable>();
		private static SortedList<string, Macro> _macros = new SortedList<string, Macro>();
		private static Stack<object> _dataStack = new Stack<object>();

		public static void Push()
		{
			SortedList<string, Variable> vars = new SortedList<string, Variable>();
			foreach (string varName in _variables.Keys)
			{
				Variable var = _variables[varName];
				vars.Add(varName, var.Clone());
			}

			SortedList<string, Macro> macros = new SortedList<string, Macro>();
			foreach (string macroName in _macros.Keys)
			{
				Macro macro = _macros[macroName];
				macros.Add(macroName, macro.Clone());
			}

			_dataStack.Push(_variables);
			_dataStack.Push(_macros);
			_variables = vars;
			_macros = macros;
		}

		public static void Pop()
		{
			if (_dataStack.Count < 2) throw new Exception("Internal: Cannot pop data because the data stack is empty.");

			_macros = (SortedList<string, Macro>)_dataStack.Pop();
			_variables = (SortedList<string, Variable>)_dataStack.Pop();
		}

		public static void DiscardPushed()
		{
			if (_dataStack.Count < 2) throw new Exception("Internal: Cannot discard pushed data because the data stack is empty.");

			_dataStack.Pop();
			_dataStack.Pop();
		}

		public static bool IsVariable(string varName)
		{
			return _variables.ContainsKey(varName.ToLower());
		}

		public static Variable GetVariable(string varName)
		{
			string varNameLower = varName.ToLower();
			if (!_variables.ContainsKey(varNameLower)) return null;
			return _variables[varNameLower];
		}

		public static Variable CreateVariable(string varName)
		{
			return CreateVariable(varName, false, false, false);
		}

		public static Variable CreateVariable(string varName, bool system, bool readOnly, bool calculated)
		{
			string varNameLower = varName.ToLower();
			if (_variables.ContainsKey(varNameLower)) throw new Exception("Internal: Variable '" + varName + "' already exists.");

			Variable var = new Variable(varName, system, readOnly, calculated);
			_variables.Add(varNameLower, var);

			if (!Globals.silentMode && !system) Globals.form.WriteLine(HistoryType.Info, "Created variable '" + varName + "'.");

			return var;
		}

		public static string[] UserVariableNames
		{
			get
			{
				List<string> names = new List<string>();
				foreach (Variable var in _variables.Values)
				{
					if (var.System) continue;
					names.Add(var.Name);
				}
				return names.ToArray();
			}
		}

		public static void DeleteVariable(string varName)
		{
			string varNameLower = varName.ToLower();
			if (!_variables.ContainsKey(varNameLower)) throw new Exception("Internal: Variable '" + varName + "' cannot be deleted because it does not exist.");
			_variables.Remove(varNameLower);
		}

		public static void Save(XmlTextWriter xml, bool includeAnswer)
		{
			xml.WriteStartElement("Variables");

			foreach (Variable var in _variables.Values)
			{
				if (var.System) continue;

				string name = "", value = "";
				try
				{
					name = var.Name;
					value = var.Value.ToString(true);
				}
				catch (Exception ex)
				{
					if (Globals.form != null)
					{
						Globals.form.WriteLine(HistoryType.Error, "Failed to generate value string for variable '" + name + "': " + ex.Message + "\n" + ex.StackTrace);
					}
					continue;
				}
				xml.WriteStartElement("Variable");
				xml.WriteAttributeString("Name", name);
				xml.WriteString(value);
				xml.WriteEndElement();
			}

			if (includeAnswer && IsVariable(Globals.k_answer))
			{
				string value = "";
				try
				{
					value = GetVariable(Globals.k_answer).Value.ToString(true);
				}
				catch (Exception ex)
				{
					if (Globals.form != null)
					{
						Globals.form.WriteLine(HistoryType.Error, "Failed to generate value string for answer variable: " + ex.Message + "\n" + ex.StackTrace);
					}
					value = "";
				}

				if (value != "")
				{
					xml.WriteStartElement("Answer");
					xml.WriteString(value);
					xml.WriteEndElement();
				}
			}

			xml.WriteEndElement();

			xml.WriteStartElement("Macros");
			foreach (Macro m in _macros.Values)
			{
				xml.WriteStartElement("Macro");
				xml.WriteAttributeString("Name", m.Name);
				xml.WriteString(m.Source);
				xml.WriteEndElement();
			}
			xml.WriteEndElement();
		}

		public static void Load(XmlDocument xml, bool userFile)
		{
			bool silentModeBefore = Globals.silentMode;
			Globals.silentMode = true;
			try
			{
				foreach (XmlElement variables in xml.GetElementsByTagName("Variables"))
				{
					foreach (XmlElement v in variables.GetElementsByTagName("Variable"))
					{
						if (!v.HasAttribute("Name")) continue;

						string name = v.GetAttribute("Name");
						string valueString = v.InnerText;
						if (valueString == "") continue;

						bool system = !userFile;
						bool readOnly = !userFile;

						//try
						//{
						//    if (v.HasAttribute("System")) system = Convert.ToBoolean(v.GetAttribute("System"));
						//}
						//catch (Exception)
						//{
						//    system = false;
						//}

						//try
						//{
						//    if (v.HasAttribute("ReadOnly")) readOnly = Convert.ToBoolean(v.GetAttribute("ReadOnly"));
						//}
						//catch (Exception)
						//{
						//    readOnly = false;
						//}

						try
						{
							if (!IsVariable(name)) CreateVariable(name);

							Command cmd = new Command();
							cmd.Execute(name + "=" + valueString, false);

							if (system || readOnly)
							{
								Variable var = GetVariable(name);
								if (system) var.System = true;
								if (readOnly) var.ReadOnly = true;
							}
						}
						catch (Exception ex)
						{
							if (Globals.form != null)
							{
								Globals.form.WriteLine(HistoryType.Error, "Error when running settings variable assignment:\n" + name + "=" + valueString + "\n" + ex.Message + "\n" + ex.StackTrace);
							}
						}
					}

					foreach (XmlElement a in variables.GetElementsByTagName("Answer"))
					{
						string valueString = a.InnerText;
						if (valueString == "") continue;

						try
						{
							Command cmd = new Command();
							cmd.Execute(Globals.k_answer + "=" + valueString, false);
						}
						catch (Exception ex)
						{
							if (Globals.form != null)
							{
								Globals.form.WriteLine(HistoryType.Error, "Error when running settings answer variable assignment:\n" + Globals.k_answer + "=" + valueString + "\n" + ex.Message + "\n" + ex.StackTrace);
							}
						}
					}
				}

				foreach (XmlElement xmlMacro in xml.GetElementsByTagName("Macro"))
				{
					string source = xmlMacro.InnerText;
					if (source == "") continue;

					try
					{
						Command cmd = new Command();
						cmd.Execute(source, false);
					}
					catch (Exception ex)
					{
						if (Globals.form != null)
						{
							Globals.form.WriteLine(HistoryType.Error, "Error when running macro source '" + source + "'\n" + ex.Message + "\n" + ex.StackTrace);
						}
					}
				}
			}
			finally
			{
				Globals.silentMode = silentModeBefore;
			}
		}

		public static Value Answer
		{
			get
			{
				if (!IsVariable(Globals.k_answer)) return new Value();
				return GetVariable(Globals.k_answer).Value;
			}
			set
			{
				Variable var = GetVariable(Globals.k_answer);
				if (var == null) var = CreateVariable(Globals.k_answer, true, false, false);
				var.Value = value;
			}
		}

		public static string AnswerName
		{
			get { return Globals.k_answer; }
		}

		public static bool IsMacro(string macroName)
		{
			return _macros.ContainsKey(macroName.ToLower());
		}

		public static Macro GetMacro(string macroName)
		{
			string macroNameLower = macroName.ToLower();
			if (!_macros.ContainsKey(macroNameLower)) throw new Exception("Internal: Macro '" + macroName + "' does not exist.");
			return _macros[macroNameLower];
		}

		public static void AddMacro(Macro macro)
		{
			string macroNameLower = macro.Name.ToLower();
			if (_macros.ContainsKey(macroNameLower)) throw new Exception("Internal: Macro '" + macro.Name + "' cannot be added because it already exists.");
			_macros.Add(macroNameLower, macro);
		}

		public static void DeleteMacro(string macroName)
		{
			string macroNameLower = macroName.ToLower();
			if (!_macros.ContainsKey(macroNameLower)) throw new Exception("Internal: Macro '" + macroName + "' cannot be deleted because it doesn't exist.");
			_macros.Remove(macroNameLower);
		}

		public static string[] MacroNames
		{
			get
			{
				string[] names = new string[_macros.Count];
				int i = 0;
				foreach (Macro macro in _macros.Values) names[i++] = macro.Name;
				return names;
			}
		}

		public static void SetDefaults()
		{
			if (!IsVariable(Globals.k_answer)) CreateVariable(Globals.k_answer, true, false, false);
			if (!IsVariable("now")) CreateVariable("now", true, true, true);
			if (!IsVariable("time")) CreateVariable("time", true, true, true);
			if (!IsVariable("date")) CreateVariable("date", true, true, true);
		}

		public static Value GetCalculatedValue(string varName)
		{
			switch (varName.ToLower())
			{
				case "now":
					return new Value(DateTime.Now.Ticks, ValueFormat.Date);

				case "time":
					return new Value(DateTime.Now.Ticks - DateTime.Today.Ticks, ValueFormat.TimeSpan);

				case "date":
					return new Value(DateTime.Today.Ticks, ValueFormat.Date);

				default:
					throw new Exception("Internal: Unknown calculated variable '" + varName + "'.");
			}
		}
	}
}
