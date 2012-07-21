using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;

namespace Calc
{
	public class HelpTopic
	{
		private string _name;
		private string _body;
		private string _topic;

		public HelpTopic()
		{
			_name = "";
			_body = "";
		}

		public string Name
		{
			get { return _name; }
		}

		public string Body
		{
			get { return _body; }
		}

		public string TopicText
		{
			get { return _topic; }
		}

		public void Load(XmlElement xml)
		{
			if (!xml.HasAttribute("Name")) throw new Exception("Help topic has no 'Name' attribute.");
			_name = xml.GetAttribute("Name");

			if (xml.HasAttribute("Topic")) _topic = xml.GetAttribute("Topic");
			else _topic = _name;

			_body = "";

			string[] lines = xml.InnerText.Trim().Replace("\r", "").Split('\n');

			foreach (string line in lines)
			{
				if (_body != "") _body += "\r\n";

				string str = line.Trim();
				if (str.IndexOf("[sp]") >= 0) str = str.Replace("[sp]", " ");
				if (str.IndexOf("[tab]") >= 0) str = str.Replace("[tab]", "\t");
				if (str.IndexOf("[br]") >= 0) str = str.Replace("[br]", "\r\n");
				_body += str;
			}
		}
	}
}
