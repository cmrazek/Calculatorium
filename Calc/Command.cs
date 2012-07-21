using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Calc
{
	class Command
	{
		private const char k_nullChar = '\0';

		private Stack<Token> _tokenStack;
		private int _pos;
		private string _cmd;
		private int _cmdLength;

		public Value Execute(string cmd, bool saveAnswer)
		{
			Token tokenTree = CreateTokenTree(cmd);

			if (Macro.ProcessPossibleMacro(tokenTree, cmd)) return null;

			tokenTree.ExecuteGroup();
			Value[] returnValues = tokenTree.ReturnValues;
			if (returnValues.Length != 1) throw new Exception("Syntax error.");

			if (saveAnswer) Data.Answer = returnValues[0];
			return returnValues[0];
		}

		private Token CreateTokenTree(string cmd)
		{
			_tokenStack = new Stack<Token>();
			_tokenStack.Push(new Token(TokenType.Group, ""));

			_pos = 0;
			_cmd = cmd;
			_cmdLength = cmd.Length;

			char ch, nextCh1, nextCh2, lastCh = k_nullChar;

			while (_pos < _cmdLength)
			{
				ch = cmd[_pos];
				nextCh1 = (_pos + 1 < _cmdLength ? cmd[_pos + 1] : k_nullChar);
				nextCh2 = (_pos + 2 < _cmdLength ? cmd[_pos + 2] : k_nullChar);

				// Ignore whitespace
				if (Char.IsWhiteSpace(ch))
				{
					_pos++;
					continue;
				}

				// In this if/else block, ensure that g_pos is always advanced under all conditions,
				// unless an error is thrown.
				if (Char.IsLetter(ch))
				{
					GrabAlnumToken();
				}
				else if (Char.IsDigit(ch) || ch == '.')
				{
					GrabNumberToken();
				}
				else if (ch == '+')
				{
					if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.AddAssign, "+="));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.Add, "+"));
						_pos++;
					}
				}
				else if (ch == '-')
				{
					if (Char.IsDigit(nextCh1) && (lastCh == k_nullChar || IsOperatorChar(lastCh)))
					{
						GrabNumberToken();
					}
					else if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.SubAssign, "-="));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.Sub, "-"));
						_pos++;
					}
				}
				else if (ch == '*')
				{
					if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.MulAssign, "*="));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.Mul, "*"));
						_pos++;
					}
				}
				else if (ch == '/')
				{
					if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.DivAssign, "/="));
						_pos += 2;
					}
					else
					{
						if (Settings.UseDivForFractions) AddToken(new Token(OperatorType.Fraction, "/"));
						else AddToken(new Token(OperatorType.Div, "/"));
						_pos++;
					}
				}
				else if (ch == '%')
				{
					if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.ModAssign, "%="));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.Mod, "%"));
						_pos++;
					}
				}
				else if (ch == '<')
				{
					if (nextCh1 == '<')
					{
						if (nextCh2 == '=')
						{
							AddToken(new Token(OperatorType.ShiftLeftAssign, "<<="));
							_pos += 3;
						}
						else
						{
							AddToken(new Token(OperatorType.ShiftLeft, "<<"));
							_pos += 2;
						}
					}
					else if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.LessOrEqual, "<="));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.LessThan, "<"));
						_pos++;
					}
				}
				else if (ch == '>')
				{
					if (nextCh1 == '>')
					{
						if (nextCh2 == '=')
						{
							AddToken(new Token(OperatorType.ShiftRightAssign, ">>="));
							_pos += 3;
						}
						else
						{
							AddToken(new Token(OperatorType.ShiftRight, ">>"));
							_pos += 2;
						}
					}
					else if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.GreaterOrEqual, ">="));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.GreaterThan, ">"));
						_pos++;
					}
				}
				else if (ch == '|')
				{
					if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.BitwiseOrAssign, "|="));
						_pos += 2;
					}
					else if (nextCh1 == '|')
					{
						AddToken(new Token(OperatorType.Or, "||"));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.BitwiseOr, "|"));
						_pos++;
					}
				}
				else if (ch == '^')
				{
					if (nextCh1 == '=')
					{
						AddToken(new Token(Settings.EnableXor ? OperatorType.BitwiseXorAssign : OperatorType.PwrAssign, "^="));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(Settings.EnableXor ? OperatorType.BitwiseXor : OperatorType.Pwr, "^"));
						_pos++;
					}
				}
				else if (ch == '&')
				{
					if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.BitwiseAndAssign, "&="));
						_pos += 2;
					}
					else if (nextCh1 == '&')
					{
						AddToken(new Token(OperatorType.And, "&&"));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.BitwiseAnd, "&"));
						_pos++;
					}
				}
				else if (ch == '=')
				{
					if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.Equal, "=="));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.Assign, "="));
						_pos++;
					}
				}
				else if (ch == '(')
				{
					Token groupToken = new Token(TokenType.Group, "");
					AddToken(groupToken);
					_tokenStack.Push(groupToken);
					_pos++;
				}
				else if (ch == ')')
				{
					if (_tokenStack.Count <= 1) throw new Exception("Mismatched ')'.");
					_tokenStack.Pop();
					_pos++;
				}
				else if (ch == ',')
				{
					AddToken(new Token(OperatorType.Comma, ","));
					_pos++;
				}
				else if (ch == '\\')
				{
					AddToken(new Token(OperatorType.Fraction, "\\"));
					_pos++;
				}
				else if (ch == '\"')
				{
					AddToken(new Token(OperatorType.Inches, "\""));
					_pos++;
				}
				else if (ch == '\'')
				{
					AddToken(new Token(OperatorType.Feet, "\'"));
					_pos++;
				}
				else if (ch == '~')
				{
					AddToken(new Token(OperatorType.BitwiseNot, "~"));
					_pos++;
				}
				else if (ch == '[')
				{
					GrabDateToken();
				}
				else if (ch == '{')
				{
					GrabTimeSpanToken();
				}
				else if (ch == '!')
				{
					if (nextCh1 == '=')
					{
						AddToken(new Token(OperatorType.NotEqual, "!="));
						_pos += 2;
					}
					else
					{
						AddToken(new Token(OperatorType.Not, "!"));
						_pos++;
					}
				}
				else if (ch == '?')
				{
					AddToken(new Token(OperatorType.Condition1, "?"));
					_pos++;
				}
				else if (ch == ':')
				{
					AddToken(new Token(OperatorType.Condition2, ":"));
					_pos++;
				}
				else
				{
					throw new Exception("Invalid char '" + ch + "'.");
				}

				lastCh = ch;
			}

			if (_tokenStack.Count != 1) throw new Exception("Mismatched '('.");
			return _tokenStack.Pop();
		}

		public static bool IsOperatorChar(char ch)
		{
			switch (ch)
			{
				case '+':
				case '-':
				case '*':
				case '/':
				case '%':
				case '^':
				case '<':
				case '>':
				case '=':
				case '|':
				case '&':
				case '\\':
				case '\"':
				case '\'':
				case ',':
				case '!':
					return true;

				default:
					return false;
			}
		}

		private void GrabAlnumToken()
		{
			string text = "";
			text += _cmd[_pos++];

			char ch = _pos < _cmdLength ? _cmd[_pos] : k_nullChar;
			while (_pos < _cmdLength && (Char.IsLetterOrDigit(ch) || ch == '_'))
			{
				text += ch;
				if (++_pos < _cmdLength) ch = _cmd[_pos];
			}

			if (Function.IsFunction(text))
			{
				AddToken(new Token(TokenType.Function, text));
			}
			else if (Data.IsMacro(text) && Data.IsVariable(text))
			{
				// This is both a macro and variable.
				// If the next character is a '(', assume the macro is being referenced.

				char nextCh = k_nullChar;
				for (int pos = _pos; (nextCh == k_nullChar || Char.IsWhiteSpace(nextCh)) && pos < _cmdLength; pos++)
				{
					nextCh = _cmd[pos];
				}

				if (nextCh == '(') AddToken(new Token(TokenType.Macro, text));
				else AddToken(new Token(TokenType.Variable, text));
			}
			else if (Data.IsMacro(text))
			{
				AddToken(new Token(TokenType.Macro, text));
			}
			else
			{
				AddToken(new Token(TokenType.Variable, text));
			}
		}

		private void GrabNumberToken()
		{
			bool neg = false;
			char ch = _cmd[_pos];
			char chNext = _pos + 1 < _cmdLength ? _cmd[_pos + 1] : k_nullChar;
			char chNext2 = _pos + 2 < _cmdLength ? _cmd[_pos + 2] : k_nullChar;
			string prefixText = "";

			if (ch == '-')
			{
				neg = true;
				prefixText = "-";
				_pos++;
				ch = _pos < _cmdLength ? _cmd[_pos] : k_nullChar;
				chNext = _pos + 1 < _cmdLength ? _cmd[_pos + 1] : k_nullChar;
				chNext2 = _pos + 2 < _cmdLength ? _cmd[_pos + 2] : k_nullChar;
			}

			ValueFormat format;

			if (ch == '0' && (chNext == 'x' || chNext == 'X') && IsHexDigit(chNext2))
			{
				_pos += 2;
				prefixText += ch + chNext;
				format = ValueFormat.Hex;
			}
			else if (ch == '0' && (chNext == 'b' || chNext == 'B') && IsBinDigit(chNext2))
			{
				_pos += 2;
				prefixText += ch + chNext;
				format = ValueFormat.Bin;
			}
			else if (ch == '0' && (chNext == 'o' || chNext == 'O') && IsOctDigit(chNext2))
			{
				_pos += 2;
				prefixText += ch + chNext;
				format = ValueFormat.Oct;
			}
			else
			{
				format = ValueFormat.Dec;
			}

			ch = _pos < _cmdLength ? _cmd[_pos] : k_nullChar;

			char lastCh = k_nullChar;
			string parseText = "";
			while (_pos < _cmdLength)
			{
				if (format == ValueFormat.Hex)
				{
					if (!IsHexDigit(ch)) break;
				}
				else if (format == ValueFormat.Bin)
				{
					if (!IsBinDigit(ch)) break;
				}
				else if (format == ValueFormat.Oct)
				{
					if (!IsOctDigit(ch)) break;
				}
				else
				{
					bool decValid = false;
					if (Char.IsDigit(ch) || ch == '.') decValid = true;
					else if ((ch == 'e' || ch == 'E') && Char.IsDigit(lastCh)) decValid = true;
					else if ((ch == '+' || ch == '-') && (lastCh == 'e' || lastCh == 'E')) decValid = true;
					if (!decValid) break;
				}

				parseText += ch;
				_pos++;
				lastCh = ch;
				ch = _pos < _cmdLength ? _cmd[_pos] : k_nullChar;
			}

			if (parseText == "") throw new Exception("Malformed number.");

			// Hack for scientific
			if (format == ValueFormat.Dec && parseText.Length > 2 && (parseText.IndexOf('e') >= 0 || parseText.IndexOf('E') >= 0))
			{
				format = ValueFormat.Sci;
			}

			decimal val = 0;

			switch (format)
			{
				case ValueFormat.Hex:
					val = ParseHexNumber(parseText);
					break;

				case ValueFormat.Bin:
					val = ParseBinNumber(parseText);
					break;

				case ValueFormat.Oct:
					val = ParseOctNumber(parseText);
					break;

				case ValueFormat.Dec:
				case ValueFormat.Sci:
					val = Decimal.Parse(parseText, System.Globalization.NumberStyles.Any);
					break;
			}

			if (neg) val *= -1;

			AddToken(new Token(TokenType.Number, prefixText + parseText, new Value(val, format)));
		}

		private static bool IsHexDigit(char ch)
		{
			return (ch >= '0' && ch <= '9') ||
				(ch >= 'a' && ch <= 'f') ||
				(ch >= 'A' && ch <= 'F');
		}

		private static bool IsBinDigit(char ch)
		{
			return ch == '0' || ch == '1';
		}

		private static bool IsOctDigit(char ch)
		{
			return ch >= '0' && ch <= '7';
		}

		private void AddToken(Token tok)
		{
			_tokenStack.Peek().AppendChild(tok);
		}

		private static decimal ParseHexNumber(string numString)
		{
			decimal val = 0;

			foreach (char ch in numString)
			{
				val *= 16;
				if (ch >= '0' && ch <= '9') val += (ch - '0');
				else if (ch >= 'a' && ch <= 'f') val += (ch - 'a' + 0x0a);
				else if (ch >= 'A' && ch <= 'F') val += (ch - 'A' + 0x0a);
			}

			return val;
		}

		private static decimal ParseBinNumber(string numString)
		{
			decimal val = 0;

			foreach (char ch in numString)
			{
				val *= 2;
				if (ch == '1') val++;
			}

			return val;
		}

		private static decimal ParseOctNumber(string numString)
		{
			decimal val = 0;

			foreach (char ch in numString)
			{
				val *= 8;
				if (ch >= '0' && ch <= '7') val += ch - '0';
			}

			return val;
		}

		private void GrabDateToken()
		{
			if (_pos >= _cmdLength) throw new Exception("Internal: GrabDateToken: position is beyond command length.");
			if (_cmd[_pos++] != '[') throw new Exception("Internal: GrabDateToken: first char must be '['.");

			int end;
			if ((end = _cmd.IndexOf(']', _pos)) < 0) throw new Exception("Mismatched '['.");

			string dtString = _cmd.Substring(_pos, end - _pos);
			DateTime dt;
			try
			{
				dt = DateTime.Parse(dtString);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to parse date string '" + dtString + "': " + ex.Message);
				throw new Exception("Invalid date.");
			}

			_pos = end + 1;
			AddToken(new Token(TokenType.Number, "[" + dtString + "]", new Value(dt.Ticks, ValueFormat.Date)));
		}

		private void GrabTimeSpanToken()
		{
			if (_pos >= _cmdLength) throw new Exception("Internal: GrabTimeSpanToken: position is beyond command length.");
			if (_cmd[_pos++] != '{') throw new Exception("Internal: GrabTimeSpanToken: first char must be '{'.");

			int end;
			if ((end = _cmd.IndexOf('}', _pos)) < 0) throw new Exception("Mismatched '{'.");

			string tmString = _cmd.Substring(_pos, end - _pos);
			TimeSpan tm;
			try
			{
				tm = TimeSpan.Parse(tmString);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to parse time span string {" + tmString + "}: " + ex.Message);
				throw new Exception("Invalid time span.");
			}

			_pos = end + 1;
			AddToken(new Token(TokenType.Number, "{" + tmString + "}", new Value(tm.Ticks, ValueFormat.TimeSpan)));
		}


	}
}
