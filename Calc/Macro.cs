using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Calc
{
	class Macro
	{
		private string _name = "";
		private string[] _args = null;
		private List<Token> _eqGroup = null;
		private string _source = "";

		public Macro(string name, string[] args, List<Token> eqGroup, string source)
		{
			if (args == null) throw new ArgumentNullException("args");
			if (eqGroup == null) throw new ArgumentNullException("eqGroup");

			_name = name;
			_args = args;
			_eqGroup = eqGroup;
			_source = source;
		}

		public Macro Clone()
		{
			string[] args = new string[_args.Length];
			for (int i = 0, ii = _args.Length; i < ii; i++) args[i] = _args[i];

			List<Token> eqGroup = new List<Token>();
			foreach (Token t in _eqGroup) eqGroup.Add(t.Clone());

			return new Macro(_name, args, eqGroup, Source);
		}

		public void LoadTokenGroup(Token token, Value[] args)
		{
			if (args.Length != _args.Length) throw new Exception("Macro '" + _name + "' requires " + _args.Length + " argument(s).");

			SortedList<string, Value> argList = new SortedList<string, Value>();
			int argIndex = 0;
			foreach (string argName in _args) argList.Add(argName, args[argIndex++]);

			foreach (Token eqToken in _eqGroup) token.AppendChild(eqToken.Clone());
			PrepareTokenGroupForMacroExe(token.Group, argList);
		}

		private void PrepareTokenGroupForMacroExe(List<Token> group, SortedList<string, Value> args)
		{
			foreach (Token tok in group)
			{
				switch(tok.Type)
				{
					case TokenType.MacroArg:
						tok.Value = args[tok.Text.ToLower()].Clone();
						break;

					case TokenType.Group:
						PrepareTokenGroupForMacroExe(tok.Group, args);
						break;
				}
			}
		}

		public string Name
		{
			get { return _name; }
		}

		public string Source
		{
			get { return _source; }
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static bool ProcessPossibleMacro(Token tokenGroup, string source)
		{
			// Macro definitions take the form:
			//	macroName(param1, param2, etc.)=<equation>

			List<Token> group = tokenGroup.Group;
			if (group.Count <= 3) return false;

			// First token must be the name
			if (group[0].Type != TokenType.Variable && group[0].Type != TokenType.Macro) return false;
			string macroName = group[0].Text;
			//if (Data.IsVariable(macroName)) return false;	// Cannot have a macro that is the same name as a variable.

			// Next token must be a set of brackets
			if (group[1].Type != TokenType.Group) return false;

			// Next token must be '='
			if (group[2].Type != TokenType.Operator || group[2].Operator != OperatorType.Assign) return false;

			// Check that arguments have correct form (arg, comma, arg, comma, ...)
			List<string> argNames = new List<string>();
			bool lookingForArg = true;
			bool gotAToken = false;

			foreach (Token argToken in group[1].Group)
			{
				if (lookingForArg)
				{
					switch (argToken.Type)
					{
						case TokenType.Variable:
						case TokenType.Function:
						case TokenType.Macro:
							break;

						default:
							throw new Exception("Expected macro argument name.");
					}

					// Check for duplicate argument name
					foreach (string argName in argNames)
					{
						if (argName.ToLower() == argToken.Text.ToLower()) throw new Exception("Duplicate macro argument '" + argToken.Text + "'.");
					}

					argNames.Add(argToken.Text.ToLower());
				}
				else // looking for comma
				{
					if (argToken.Type != TokenType.Operator || argToken.Operator != OperatorType.Comma) throw new Exception("Expected comma after argument.");
				}

				gotAToken = true;
				lookingForArg = !lookingForArg;
			}

			if (lookingForArg && gotAToken) return false;

			// Create a new group containing only the equation
			List<Token> eqGroup = new List<Token>();
			int eqGroupIndex = 0;
			foreach (Token tok in group)
			{
				if (eqGroupIndex++ >= 3) eqGroup.Add(tok.Clone());
			}

			PrepareMacroEquation(argNames, eqGroup);

			Macro macro = new Macro(macroName, argNames.ToArray(), eqGroup, source);

			if (Data.IsMacro(macroName))
			{
				Data.DeleteMacro(macroName);
				Data.AddMacro(macro);
				//_macros[macroName.ToLower()] = macro;
				if (!Globals.silentMode) Globals.form.WriteLine(HistoryType.Info, "Replaced macro '" + macroName + "'.");
			}
			else
			{
				Data.AddMacro(macro);
				//_macros.Add(macroName.ToLower(), macro);
				if (!Globals.silentMode) Globals.form.WriteLine(HistoryType.Info, "Created macro '" + macroName + "'.");
			}

			return true;
		}

		public static void PrepareMacroEquation(List<string> args, List<Token> eqGroup)
		{
			// Go through the new macro equation group, and change any variables with names
			// corresponding to argument names into different token types

			foreach (Token tok in eqGroup)
			{
				switch (tok.Type)
				{
					case TokenType.Variable:
					case TokenType.Function:
					case TokenType.Macro:
						if (args.Contains(tok.Text.ToLower())) tok.Type = TokenType.MacroArg;
						break;

					case TokenType.Group:
						PrepareMacroEquation(args, tok.Group);
						break;
				}
			}
		}
	}
}
