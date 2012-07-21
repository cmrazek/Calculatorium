using System;
using System.Collections.Generic;
using System.Text;

namespace Calc
{
	public enum TokenType
	{
		None,
		Group,
		Operator,
		Number,
		Function,
		Variable,
		Macro,
		MacroArg
	}

	public class Token
	{
		private TokenType _type;
		private string _text;
		private Value _value = null;
		private OperatorType _opType = OperatorType.None;
		private List<Token> _group = null;
		private bool _enabled = true;
		private int _precedence = 0;
		private List<Value> _returnValues = null;
		private bool _isFuncArgs = false;

		public Token(TokenType type, string text)
		{
			_type = type;
			_text = text;

			Initialize();
		}

		public Token(OperatorType opType, string text)
		{
			_type = TokenType.Operator;
			_opType = opType;
			_text = text;

			Initialize();
		}

		public Token(TokenType type, string text, Value value)
		{
			_type = type;
			_text = text;
			_value = value;

			Initialize();
		}

		private void Initialize()
		{
			if (_type == TokenType.Group)
			{
				_group = new List<Token>();
			}
		}

		public Token Clone()
		{
			Token tok = new Token(_type, _text, _value != null ? _value.Clone() : null);
			tok._opType = _opType;
			tok._isFuncArgs = _isFuncArgs;

			if (_group != null)
			{
				foreach (Token t in _group) tok._group.Add(t.Clone());
			}

			if (_returnValues != null)
			{
				tok._returnValues = new List<Value>();
				foreach (Value v in _returnValues) tok._returnValues.Add(v.Clone());
			}

			return tok;
		}

		public TokenType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public OperatorType Operator
		{
			get
			{
				if (_type != TokenType.Operator) return OperatorType.None;
				return _opType;
			}
		}

		public void AppendChild(Token tok)
		{
			if (_type != TokenType.Group) throw new Exception("Internal: Child tokens cannot be appended to '" + _type.ToString() + "' tokens.");
			if (_group == null) _group = new List<Token>();
			_group.Add(tok);
		}

		public void ExecuteGroup()
		{
			if (_type != TokenType.Group) throw new Exception("Internal: Cannot execute a token that is not a group.");

			// Give everything the default precedence
			foreach (Token tok in _group)
			{
				tok._precedence = tok.GetDefaultPrecedence();
			}

			while (true)
			{
				// Find the highest precedence in this group
				int highestPrecedence = 0;
				foreach (Token tok in _group)
				{
					if (tok._enabled && tok._precedence > highestPrecedence) highestPrecedence = tok._precedence;
				}
				if (highestPrecedence == 0) break;

				// Execute tokens at that precedence
				if (highestPrecedence % 2 == 0)
				{
					// Left-to-right
					for (int i = 0, ii = _group.Count; i < ii; i++)
					{
						Token tok = _group[i];
						if (tok._enabled && tok._precedence == highestPrecedence) ExecuteSubToken(i);
					}
				}
				else
				{
					// Right-to-left
					for (int i = _group.Count - 1; i >= 0; i--)
					{
						Token tok = _group[i];
						if (tok._enabled && tok._precedence == highestPrecedence) ExecuteSubToken(i);
					}
				}
			}

			_returnValues = new List<Value>();
			bool lookingForValue = true;
			bool gotAToken = false;
			foreach (Token tok in _group)
			{
				if (!tok._enabled) continue;
				gotAToken = true;

				if (lookingForValue)
				{
					if (!tok.HasValue)
					{
						if (tok._type == TokenType.Variable) throw new Exception("Unknown variable '" + tok._text + "'.");
						throw new Exception("Syntax error.");
					}
					_returnValues.Add(tok.Value);
					lookingForValue = false;
				}
				else // looking for comma
				{
					if (tok._type != TokenType.Operator || tok._opType != OperatorType.Comma) throw new Exception("Syntax error.");
					lookingForValue = true;
				}
			}

			if (lookingForValue && gotAToken) throw new Exception("Expected value after comma.");
		}

		private void ExecuteSubToken(int index)
		{
			Token token = _group[index];

			// Find the tokens on the left
			Token leftToken = null;
			Token leftToken2 = null;
			int numFound = 0;
			for (var i = index - 1; i >= 0; i--)
			{
				Token tok = _group[i];
				if (!tok._enabled) continue;

				if (numFound == 0)
				{
					leftToken = tok;
				}
				else if (numFound == 1)
				{
					leftToken2 = tok;
				}
				numFound++;
			}

			// Find the token on the right
			Token rightToken = null;
			for (int i = index + 1, ii = _group.Count; i < ii; i++)
			{
				Token tok = _group[i];
				if (!tok._enabled) continue;
				rightToken = tok;
				break;
			}

			token.Execute(this, leftToken2, leftToken, rightToken);
		}

		private void Execute(Token parentToken, Token leftToken2, Token leftToken, Token rightToken)
		{
			switch (_type)
			{
				case TokenType.Operator:
					Calc.Operator.Execute(this, parentToken, leftToken2, leftToken, rightToken);
					break;

				case TokenType.Group:
					if (leftToken != null && (leftToken._type == TokenType.Function || leftToken._type == TokenType.Macro)) _isFuncArgs = true;
					ExecuteGroup();
					_precedence = 0;
					break;

				case TokenType.Function:
					if (rightToken == null || rightToken._returnValues == null) throw new Exception("Missing argument list for function '" + _text + "'.");
					_value = Function.Execute(_text, rightToken._returnValues.ToArray());
					_precedence = 0;
					rightToken.Enabled = false;
					break;

				case TokenType.Macro:
					{
						if (rightToken == null || rightToken._returnValues == null) throw new Exception("Missing argument list for macro '" + _text + "'.");

						// Pretend we're a group
						this._type = TokenType.Group;

						Data.GetMacro(Text).LoadTokenGroup(this, rightToken._returnValues.ToArray());
						ExecuteGroup();
						if (_returnValues.Count != 1) throw new Exception("Macro '" + _text + "' syntax error.");
						_value = _returnValues[0];
						_precedence = 0;
						rightToken.Enabled = false;
					}
					break;

				default:
					throw new Exception("Internal: Cannot execute token type '" + _type.ToString() + "'.");
			}
		}

		
		public bool HasValue
		{
			get
			{
				switch (_type)
				{
					case TokenType.Variable:
						return Data.GetVariable(_text) != null;

					case TokenType.Group:
						return _returnValues != null && _returnValues.Count == 1;

					default:
						return _value != null;
				}
			}
		}

		public Value Value
		{
			get
			{
				switch (_type)
				{
					case TokenType.Variable:
						Variable var = Data.GetVariable(_text);
						if (var == null) throw new Exception("Unknown variable '" + _text + "'.");
						return var.Value;

					case TokenType.Group:
						if (_returnValues == null || _returnValues.Count != 1) throw new Exception("No value for group.");
						return _returnValues[0].Clone();

					default:
						return _value;
				}
			}

			set
			{
				if (_type == TokenType.Variable)
				{
					Variable var = Data.GetVariable(_text);
					if (var == null) var = Data.CreateVariable(_text);
					var.Value = value;
				}
				else
				{
					_value = value.Clone();
				}
			}
		}

		public bool CanSetValue
		{
			get
			{
				return _type == TokenType.Variable;
			}
		}

		private int GetDefaultPrecedence()
		{
			switch (_type)
			{
				case TokenType.Operator:
					switch (_opType)
					{
						case OperatorType.Comma: return 40;
						case OperatorType.Fraction: return 34;
						case OperatorType.Inches: return 32;
						case OperatorType.Feet: return 30;
						case OperatorType.BitwiseNot: return 28;
						case OperatorType.Not: return 26;
						case OperatorType.Pwr: return 24;
						case OperatorType.Mul:
						case OperatorType.Div:
						case OperatorType.Mod: return 22;
						case OperatorType.Add:
						case OperatorType.Sub: return 20;
						case OperatorType.ShiftLeft:
						case OperatorType.ShiftRight: return 18;
						case OperatorType.BitwiseAnd: return 16;
						case OperatorType.BitwiseOr: return 14;
						case OperatorType.BitwiseXor: return 12;
						case OperatorType.Equal:
						case OperatorType.NotEqual:
						case OperatorType.LessThan:
						case OperatorType.LessOrEqual:
						case OperatorType.GreaterThan:
						case OperatorType.GreaterOrEqual: return 10;
						case OperatorType.And: return 8;
						case OperatorType.Or: return 6;
						case OperatorType.Condition1: return 4;
						case OperatorType.Condition2: return 3;
						case OperatorType.Assign:
						case OperatorType.AddAssign:
						case OperatorType.SubAssign:
						case OperatorType.MulAssign:
						case OperatorType.DivAssign:
						case OperatorType.ModAssign:
						case OperatorType.PwrAssign:
						case OperatorType.ShiftLeftAssign:
						case OperatorType.ShiftRightAssign:
						case OperatorType.BitwiseAndAssign:
						case OperatorType.BitwiseOrAssign:
						case OperatorType.BitwiseXorAssign: return 1;
						default: return 0;
					}

				case TokenType.Group:
					return 38;

				case TokenType.Function:
				case TokenType.Macro:
					return 36;

				default:
					return 0;
			}
		}


		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public Value[] ReturnValues
		{
			get { return _returnValues.ToArray(); }
		}

		public int Precedence
		{
			get { return _precedence; }
			set { _precedence = value; }
		}

		public string Text
		{
			get { return _text; }
		}

		public List<Token> Group
		{
			get
			{
				if (_group == null) throw new Exception("Internal: Cannot get the group of a non-group token.");
				return _group;
			}
		}

		public bool IsFuncArgs
		{
			get { return _isFuncArgs; }
		}

	}
}
