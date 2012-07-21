using System;
using System.Collections.Generic;
using System.Text;

namespace Calc
{
	class TagString
	{
		private string _string;
		private object _tag;

		public TagString(string str, object tag)
		{
			_string = str;
			_tag = tag;
		}

		public override string ToString()
		{
			return _string;
		}

		public object Tag
		{
			get { return _tag; }
			set { _tag = value; }
		}

		public string String
		{
			get { return _string; }
			set { _string = value; }
		}
	}
}
