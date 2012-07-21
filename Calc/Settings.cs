using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Drawing;

namespace Calc
{
	class Settings
	{
		private const int k_defaultNumDecimals = 6;

		private static bool _enableXor = false;
		private static bool _useDivForFractions = false;
		private static bool _showMixedFractions = false;
		private static bool _reduceFeet = true;
		private static bool _time12Hour = false;
		private static bool _showMsec = false;
		private static bool _digitGrouping = false;
		private static int _numDecimals = k_defaultNumDecimals;
		private static bool _dataDialogShowVariables = true;
		private static bool _dataDialogShowMacros = true;
		private static string _dataDialogExportDir = "";
		private static bool _useDegrees = Globals.k_defaultUseDegrees;

		public static bool EnableXor
		{
			get { return _enableXor; }
			set { _enableXor = value; }
		}

		public static bool UseDivForFractions
		{
			get { return _useDivForFractions; }
			set { _useDivForFractions = value; }
		}

		public static bool ShowMixedFractions
		{
			get { return _showMixedFractions; }
			set { _showMixedFractions = value; }
		}

		public static bool ReduceFeet
		{
			get { return _reduceFeet; }
			set { _reduceFeet = value; }
		}

		public static bool Time12Hour
		{
			get { return _time12Hour; }
			set { _time12Hour = value; }
		}

		public static bool ShowMilliseconds
		{
			get { return _showMsec; }
			set { _showMsec = value; }
		}

		public static bool DigitGrouping
		{
			get { return _digitGrouping; }
			set { _digitGrouping = value; }
		}

		public static Random Rand
		{
			get { return Globals.rand; }
		}

		public static int NumDecimals
		{
			get { return _numDecimals; }
			set { _numDecimals = value; }
		}

		public static bool DataDialogShowVariables
		{
			get { return _dataDialogShowVariables; }
			set { _dataDialogShowVariables = value; }
		}

		public static bool DataDialogShowMacros
		{
			get { return _dataDialogShowMacros; }
			set { _dataDialogShowMacros = value; }
		}

		public static string DataDialogExportDir
		{
			get { return _dataDialogExportDir; }
			set { _dataDialogExportDir = value; }
		}

		public static bool UseDegrees
		{
			get { return _useDegrees; }
			set { _useDegrees = value; }
		}

		public static void Save(CalcForm form)
		{
			try
			{
				string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + Globals.k_appNameIdent;
				if (!Directory.Exists(appDataPath)) Directory.CreateDirectory(appDataPath);

				string fileName = appDataPath + Path.DirectorySeparatorChar + Globals.k_settingsFileName;

				XmlTextWriter xml = new XmlTextWriter(fileName, Encoding.UTF8);
				try
				{
					xml.Formatting = Formatting.Indented;
					xml.WriteStartDocument();
					xml.WriteStartElement(Globals.k_appNameIdent);

					SaveSettings(xml);
					Data.Save(xml, true);
					form.SaveSettings(xml);

					xml.WriteEndElement();
					xml.WriteEndDocument();
				}
				finally
				{
					xml.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred when trying to save the settings:\n" + ex.Message + "\n\n" + ex.StackTrace, "Settings Not Saved");
			}
		}

		public static void Load(CalcForm form)
		{
			try
			{
				string defaultFile = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + Globals.k_defaultSettingsFileName;
				string userFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar
					+ Globals.k_appNameIdent + Path.DirectorySeparatorChar + Globals.k_settingsFileName;

				Data.SetDefaults();
				LoadFile(defaultFile, form, false);
				LoadFile(userFile, form, true);
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred when trying to load the settings:\n" + ex.Message + "\n\n" + ex.StackTrace, "Settings Not Loaded");
			}
		}

		public static void LoadFile(string fileName, CalcForm form, bool custom)
		{
			try
			{
				if (!File.Exists(fileName))
				{
					Debug.WriteLine("The settings file '" + fileName + "' does not exist.");
					return;
				}

				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(fileName);

				LoadSettings(xmlDoc);
				Data.Load(xmlDoc, custom);
				Help.Load(xmlDoc);
				form.LoadSettings(xmlDoc, custom);
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred when trying to load the settings file '" + fileName + "':\n" + ex.Message + "\n\n" + ex.StackTrace, "Settings Not Loaded");
			}
		}

		private static void SaveSettings(XmlTextWriter xml)
		{
			xml.WriteStartElement("Settings");
			if (_enableXor != false) xml.WriteElementString("EnableXor", _enableXor.ToString());
			if (_useDivForFractions != false) xml.WriteElementString("UseDivideForFractions", _useDivForFractions.ToString());
			if (_showMixedFractions != false) xml.WriteElementString("ShowMixedFractions", _showMixedFractions.ToString());
			if (_reduceFeet != true) xml.WriteElementString("ReduceFeet", _reduceFeet.ToString());
			if (_time12Hour != false) xml.WriteElementString("Time12Hour", _time12Hour.ToString());
			if (_showMsec != false) xml.WriteElementString("ShowMilliseconds", _showMsec.ToString());
			if (_digitGrouping != false) xml.WriteElementString("DigitGrouping", _digitGrouping.ToString());
			if (_numDecimals != k_defaultNumDecimals) xml.WriteElementString("NumDecimals", _numDecimals.ToString());
			if (_dataDialogShowVariables != true) xml.WriteElementString("DataDialogShowVariables", _dataDialogShowVariables.ToString());
			if (_dataDialogShowMacros != true) xml.WriteElementString("DataDialogShowMacros", _dataDialogShowMacros.ToString());
			if (_dataDialogExportDir != "") xml.WriteElementString("DataDialogExportDir", _dataDialogExportDir);
			if (_useDegrees != Globals.k_defaultUseDegrees) xml.WriteElementString("UseDegrees", _useDegrees.ToString());
			xml.WriteEndElement();
		}

		private static void LoadSettings(XmlDocument xml)
		{
			foreach (XmlElement s in xml.GetElementsByTagName("Settings"))
			{
				_enableXor = LoadBool(s, "EnableXor", _enableXor);
				_useDivForFractions = LoadBool(s, "UseDivideForFractions", _useDivForFractions);
				_showMixedFractions = LoadBool(s, "ShowMixedFractions", _showMixedFractions);
				_reduceFeet = LoadBool(s, "ReduceFeet", _reduceFeet);
				_time12Hour = LoadBool(s, "Time12Hour", _time12Hour);
				_showMsec = LoadBool(s, "ShowMilliseconds", _showMsec);
				_digitGrouping = LoadBool(s, "DigitGrouping", _digitGrouping);
				_numDecimals = LoadInt(s, "NumDecimals", _numDecimals, 0, 20);
				_dataDialogShowVariables = LoadBool(s, "DataDialogShowVariables", _dataDialogShowVariables);
				_dataDialogShowMacros = LoadBool(s, "DataDialogShowMacros", _dataDialogShowMacros);
				_dataDialogExportDir = LoadString(s, "DataDialogExportDir", _dataDialogExportDir);
				_useDegrees = LoadBool(s, "UseDegrees", _useDegrees);
			}
		}

		private static bool LoadBool(XmlElement element, string tagName, bool defaultValue)
		{
			try
			{
				if (element[tagName] == null) return defaultValue;
				return Convert.ToBoolean(element[tagName].InnerText);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		private static int LoadInt(XmlElement element, string tagName, int defaultValue, int minValue, int maxValue)
		{
			try
			{
				if (element[tagName] == null) return defaultValue;
				int val = Convert.ToInt32(element[tagName].InnerText);
				if (val < minValue || val > maxValue) return defaultValue;
				return val;
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		private static string LoadString(XmlElement element, string tagName, string defaultValue)
		{
			try
			{
				if (element[tagName] == null) return defaultValue;
				return element[tagName].InnerText;
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
	}
}
