using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Calc
{
	class Variable
	{
		private string _name = "";
		private Value _value = new Value();
		private bool _system = false;
		private bool _readOnly = false;
		private bool _calculated = false;

		public Variable(string name, bool system, bool readOnly)
		: this(name, system, readOnly, false)
		{
		}

		public Variable(string name, bool system, bool readOnly, bool calculated)
		{
			_name = name;
			_system = system;
			_readOnly = readOnly;
			_calculated = calculated;
		}

		public Variable Clone()
		{
			Variable var = new Variable(_name, _system, _readOnly);
			var._value = _value.Clone();
			return var;
		}

		public Value Value
		{
			get
			{
				if (_calculated) return Data.GetCalculatedValue(_name);
				return _value.Clone();
			}
			set
			{
				if (_readOnly) throw new Exception("Variable '" + _name + "' is read-only.");
				_value = value.Clone();
			}
		}

		public string Name
		{
			get { return _name; }
		}

		public bool System
		{
			get { return _system; }
			set { _system = value; }
		}

		public bool ReadOnly
		{
			get { return _readOnly; }
			set { _readOnly = value; }
		}
		
	}
}
