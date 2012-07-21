using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Calc
{
	class Util
	{
		public static int StringToInt(string str, int def)
		{
			try
			{
				return Convert.ToInt32(str);
			}
			catch (Exception)
			{
				return def;
			}
		}

		public static bool StringToBool(string str, bool def)
		{
			try
			{
				return Convert.ToBoolean(str);
			}
			catch (Exception)
			{
				return def;
			}
		}

		public static float StringToFloat(string str, float def)
		{
			try
			{
				return Convert.ToSingle(str);
			}
			catch (Exception)
			{
				return def;
			}
		}

		public static Color StringToColor(string str, Color def)
		{
			try
			{
				return Color.FromName(str);
			}
			catch (Exception)
			{
				return def;
			}
		}

		public static string ColorToString(Color color)
		{
			return color.Name;
		}

		public static double RadToDeg(double r)
		{
			return r * 180.0d / Math.PI;
		}

		public static double DegToRad(double d)
		{
			return d * Math.PI / 180.0d;
		}

		public static double ToRads(double angle)
		{
			if (Settings.UseDegrees) return angle * Math.PI / 180.0d;
			return angle;
		}

		public static double ToAngle(double rads)
		{
			if (Settings.UseDegrees) return rads * 180.0d / Math.PI;
			return rads;
		}
	}
}
