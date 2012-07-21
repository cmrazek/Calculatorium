using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Calc
{
	public class Help
	{
		private static SortedList<string, HelpTopic> _help = new SortedList<string, HelpTopic>();

		public static bool IsHelp(string name)
		{
			return _help.ContainsKey(name.ToLower());
		}

		public static HelpTopic GetHelp(string name)
		{
			string nameLower = name.ToLower();
			if (!_help.ContainsKey(nameLower)) throw new Exception("No help found for '" + name + "'.");

			return _help[nameLower];
		}

		public static void Load(XmlDocument xml)
		{
			foreach (XmlElement xmlItem in xml.GetElementsByTagName("HelpItem"))
			{
				try
				{
					HelpTopic topic = new HelpTopic();
					topic.Load(xmlItem);
					_help[topic.Name.ToLower()] = topic;
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Failed to load help topic: " + ex.Message);
				}
			}
		}

		public static HelpTopic[] Topics
		{
			get
			{
				HelpTopic[] ret = new HelpTopic[_help.Count];
				int i = 0;

				foreach (HelpTopic h in _help.Values)
				{
					ret[i++] = h;
				}

				return ret;
			}
		}
	}
}
