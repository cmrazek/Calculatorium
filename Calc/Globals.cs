using System;
using System.Collections.Generic;
using System.Text;

namespace Calc
{
	class Globals
	{
		public const string k_appName = "Calculatorium";
		public const string k_appNameIdent = "Calculatorium";

		public const string k_defaultSettingsFileName = "Default.xml";
		public const string k_settingsFileName = "Settings.xml";
		public const string k_answer = "answer";

		public const bool k_defaultUseDegrees = false;

		public static Random rand = new Random();
		public static CalcForm form = null;
		public static bool silentMode = false;
	}
}
