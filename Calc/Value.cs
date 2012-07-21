using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Diagnostics;

namespace Calc
{
	public enum ValueFormat
	{
		Default,
		Dec,
		Hex,
		Bin,
		Oct,
		Sci,
		Fraction,
		Inches,
		Date,
		TimeSpan
	}

	public class Value
	{
		private decimal _decValue;
		private decimal _num;
		private decimal _denom;
		private ValueFormat _format;

		private static decimal[] k_factorPrimes = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61,
								67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137,
								139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199 };

		private const byte k_hexZeroOffset = 48;
		private const byte k_hexAlphaOffset = 55;

		public Value()
		{
			_decValue = 0;
			_num = 0;
			_denom = 1;
			_format = ValueFormat.Default;
		}

		public Value(decimal decValue, ValueFormat format)
		{
			_decValue = decValue;
			_format = format;
			_num = decValue;
			_denom = 1;
		}

		public Value(decimal num, decimal denom, ValueFormat format)
		{
			if (denom == 0) throw new Exception("Denominator cannot be zero.");

			_num = num;
			_denom = denom;
			_format = format;
			_decValue = _num / _denom;

			SimplifyFraction();
		}

		public Value(decimal decValue, decimal num, decimal denom, ValueFormat format)
		{
			if (denom == 0) throw new Exception("Denominator cannot be zero.");

			_decValue = decValue;
			_num = num;
			_denom = denom;
			_format = format;

			SimplifyFraction();
		}

		public Value(double dblValue, ValueFormat format)
		{
			_decValue = Convert.ToDecimal(dblValue);
			_format = format;
			_num = _decValue;
			_denom = 1;
		}

		public Value(int intValue, ValueFormat format)
		{
			_decValue = Convert.ToDecimal(intValue);
			_format = format;
			_num = _decValue;
			_denom = 1;
		}

		public Value(long longValue, ValueFormat format)
		{
			_decValue = Convert.ToDecimal(longValue);
			_format = format;
			_num = _decValue;
			_denom = 1;
		}

		public decimal DecValue
		{
			get { return _decValue; }
			set
			{
				_decValue = value;
				_num = _decValue;
				_denom = 1;
			}
		}

		public double DblValue
		{
			get { return Convert.ToDouble(_decValue); }
		}

		public long LongValue
		{
			get { return Convert.ToInt64(_decValue); }
		}

		public int IntValue
		{
			get { return Convert.ToInt32(_decValue); }
		}

		public ValueFormat Format
		{
			get { return _format; }
			set
			{
				if (!IsFractionalFormat(value) && IsFractionalFormat(_format))
				{
					_num = _decValue;
					_denom = 1;
				}
				_format = value;
			}
		}

		public Value Clone()
		{
			Value val = new Value(_decValue, _num, _denom, _format);
			val._num = _num;
			val._denom = _denom;
			return val;
		}

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(bool saving)
		{
			switch (_format)
			{
				case ValueFormat.Hex:
					return ToHexString();

				case ValueFormat.Bin:
					return ToBinString();

				case ValueFormat.Oct:
					return ToOctString();

				case ValueFormat.Sci:
					return _decValue.ToString("E");

				case ValueFormat.Fraction:
					{
						string fracSymbol = "\\";
						if (!saving && Settings.UseDivForFractions) fracSymbol = "/";

						if (saving) return _num.ToString() + fracSymbol + _denom.ToString();
						if (_denom == 1) return _num.ToString();

						if (Settings.ShowMixedFractions && Math.Abs(_num) > Math.Abs(_denom))
						{
							decimal whole, num, denom;
							GetMixedFractionValues(out whole, out num, out denom);
							return whole.ToString() + " " + num.ToString() + fracSymbol + denom.ToString();
						}

						return _num.ToString() + fracSymbol + _denom.ToString();
					}

				case ValueFormat.Inches:
					{
						string fracSymbol = "\\";
						if (!saving && Settings.UseDivForFractions) fracSymbol = "/";

						decimal whole, num, denom;
						GetMixedFractionValues(out whole, out num, out denom);

						decimal feet, inches;
						if (whole >= 12 && Settings.ReduceFeet)
						{
							feet = Math.Floor(whole / 12);
							inches = Math.Floor(whole % 12);
						}
						else
						{
							feet = 0;
							inches = whole;
						}

						string str = "";
						if (feet != 0) str = feet.ToString() + "'";
						if (inches != 0)
						{
							if (str != "") str += " ";
							str += inches.ToString();
						}

						if (num != 0)
						{
							if (str != "") str += " ";
							str += num.ToString() + fracSymbol + denom.ToString();
						}

						if (inches != 0 || num != 0) str += "\"";
						return str;
					}

				case ValueFormat.Date:
					{
						DateTime dt = new DateTime(Convert.ToInt64(_decValue));
						string str = "["
							+ dt.Year.ToString().PadLeft(4, '0') + "-"
							+ dt.Month.ToString().PadLeft(2, '0') + "-"
							+ dt.Day.ToString().PadLeft(2, '0');

						if (dt.Hour != 0 || dt.Minute != 0 || dt.Second != 0 || (dt.Millisecond != 0 && Settings.ShowMilliseconds))
						{
							str += " ";

							if (Settings.Time12Hour)
							{
								if (dt.Hour == 0) str += "12";
								else if (dt.Hour > 12) str += (dt.Hour - 12).ToString();
								else str += dt.Hour.ToString();
							}
							else
							{
								str += dt.Hour.ToString().PadLeft(2, '0');
							}
							str += ":" + dt.Minute.ToString().PadLeft(2, '0');
							str += ":" + dt.Second.ToString().PadLeft(2, '0');
							if (dt.Millisecond != 0 && Settings.ShowMilliseconds)
							{
								str += "." + dt.Millisecond.ToString().PadLeft(3, '0');
							}

							if (Settings.Time12Hour)
							{
								if (dt.Hour >= 12) str += " PM";
								else str += " AM";
							}
						}

						str += "]";

						Debug.WriteLine("Date ToString '" + str + "' ticks [" + dt.Ticks.ToString() + "]");
						return str;
					}

				case ValueFormat.TimeSpan:
					{
						TimeSpan t = new TimeSpan(Convert.ToInt64(Math.Abs(_decValue)));
						return "{" + t.ToString() + "}";
					}

				default:	// Dec or Default
					{
						string fmt;
						if (!saving && Settings.DigitGrouping) fmt = "#,0";
						else fmt = "0";

						if (Settings.NumDecimals > 0)
						{
							fmt += ".";
							for (int i = 0, ii = Settings.NumDecimals; i < ii; i++) fmt += "#";
						}

						return _decValue.ToString(fmt, CultureInfo.InvariantCulture);
					}
			}
		}

		private void GetMixedFractionValues(out decimal whole, out decimal num, out decimal denom)
		{
			if (!IsFractional)
			{
				whole = 0;
				num = _decValue;
				denom = 1;
				return;
			}

			if (_denom == 1)
			{
				whole = _num;
				num = 0;
				denom = 1;
				return;
			}

			if (_num == 0)
			{
				whole = 0;
				num = 0;
				denom = 1;
				return;
			}

			whole = Math.Floor(Math.Abs(_num) / Math.Abs(_denom));
			num = Math.Round(_num - (whole * _denom));
			denom = _denom;
			if (num < 0)
			{
				whole = -whole;
				num = -num;
			}

			SimplifyFraction(ref num, ref denom);
		}

		private void SimplifyFraction()
		{
			SimplifyFraction(ref _num, ref _denom);
		}

		private static void SimplifyFraction(ref decimal num, ref decimal denom)
		{
			if (denom == 1) return;

			if (num == 0)
			{
				denom = 1;
				return;
			}

			if (num % denom == 0)
			{
				num = Math.Round(num / denom);
				denom = 1;
			}

			if (denom % num == 0)
			{
				denom = Math.Round(denom / num);
				num = 1;
			}

			if (denom < 0)
			{
				num = -num;
				denom = -denom;
			}

			if (num == 1 || denom == 1) return;

			// Try to reduce by using some prime numbers.
			int i, ii;
			decimal prime;
			ii = k_factorPrimes.Length;
			for (i = 0; i < ii; i++)
			{
				prime = k_factorPrimes[i];

				if (num == prime || denom == prime) return;	// Can't reduce a prime any further

				if (num % prime == 0 && denom % prime == 0)
				{
					num = Math.Round(num / prime);
					denom = Math.Round(denom / prime);
					if (denom == 1) return;
					i = -1;
				}
			}
		}

		public decimal FindCommonDenom(decimal rightDenom)
		{
			if (_denom == rightDenom) return 1;

			if (_denom % rightDenom == 0) return Math.Round(_denom / rightDenom);

			if (rightDenom % _denom == 0)
			{
				decimal scale = Math.Round(rightDenom / _denom);
				_num = Math.Round(_num * scale);
				_denom = Math.Round(_denom * scale);
				return 1;
			}

			decimal thisDenom = _denom;
			_num = Math.Round(_num * rightDenom);
			_denom = Math.Round(_denom * rightDenom);
			return thisDenom;
		}

		public static bool IsFractionalFormat(ValueFormat fmt)
		{
			return fmt == ValueFormat.Fraction || fmt == ValueFormat.Inches;
		}

		public bool IsFractional
		{
			get { return IsFractionalFormat(_format); }
		}

		public bool IsInteger
		{
			get { return _decValue % 1 == 0; }
		}

		public Value Add(Value right)
		{
			if ((IsFractional && right.IsFractional)
				|| (IsFractional && right.IsInteger)
				|| (IsInteger && right.IsFractional))
			{
				Value ret = Clone();
				if (!ret.IsFractional) ret._format = right._format;
				decimal scale = ret.FindCommonDenom(right._denom);
				ret._num = Math.Round(ret._num + right._num * scale);
				ret._decValue = ret._num / ret._denom;
				ret.SimplifyFraction();
				return ret;
			}

			return new Value(_decValue + right._decValue, DecideFormat(_format, right._format));
		}

		public Value Subtract(Value right)
		{
			if ((IsFractional && right.IsFractional)
				|| (IsFractional && right.IsInteger)
				|| (IsInteger && right.IsFractional))
			{
				Value ret = Clone();
				if (!ret.IsFractional) ret._format = right._format;
				decimal scale = ret.FindCommonDenom(right._denom);
				ret._num = Math.Round(ret._num - right._num * scale);
				ret._decValue = ret._num / ret._denom;
				ret.SimplifyFraction();
				return ret;
			}

			if (_format == ValueFormat.Date)
			{
				decimal result = _decValue - right._decValue;
				ValueFormat fmt = ValueFormat.Date;
				if (right._format == ValueFormat.Date) fmt = ValueFormat.TimeSpan;
				return new Value(result, fmt);
			}

			return new Value(_decValue - right._decValue, DecideFormat(_format, right._format));
		}

		public Value Multiply(Value right)
		{
			if ((IsFractional && right.IsFractional)
				|| (IsFractional && right.IsInteger)
				|| (IsInteger && right.IsFractional))
			{
				Value ret = Clone();
				if (!ret.IsFractional) ret._format = right._format;
				ret._num = Math.Round(ret._num * right._num);
				ret._denom = Math.Round(ret._denom * right._denom);
				ret._decValue = ret._num / ret._denom;
				ret.SimplifyFraction();
				return ret;
			}

			//if (_format == ValueFormat.Date || right._format == ValueFormat.Date) throw new Exception("Dates cannot be multiplied.");

			return new Value(_decValue * right._decValue, DecideFormat(_format, right._format));
		}

		public Value Divide(Value right)
		{
			if ((IsFractional && right.IsFractional)
				|| (IsFractional && right.IsInteger)
				|| (IsInteger && right.IsFractional))
			{
				Value ret = Clone();
				if (!ret.IsFractional) ret._format = right._format;
				if (right._num == 0 || right._denom == 0) throw new Exception("Division by zero.");
				ret._num = Math.Round(ret._num / right._num);
				ret._denom = Math.Round(ret._denom / right._denom);
				ret._decValue = ret._num / ret._denom;
				ret.SimplifyFraction();
				return ret;
			}

			if (right._decValue == 0) throw new Exception("Division by zero.");
			//if (_format == ValueFormat.Date || right._format == ValueFormat.Date) throw new Exception("Dates cannot be divided.");

			return new Value(_decValue / right._decValue, DecideFormat(_format, right._format));
		}

		public Value Modulus(Value right)
		{
			if ((IsFractional && right.IsFractional)
				|| (IsFractional && right.IsInteger)
				|| (IsInteger && right.IsFractional))
			{
				Value ret = Clone();
				if (!ret.IsFractional) ret._format = right._format;
				decimal scale = ret.FindCommonDenom(right._denom);
				ret._num = Math.Round(ret._num % (right._num * scale));
				ret._decValue = ret._num / ret._denom;
				ret.SimplifyFraction();
				return ret;
			}

			//if (_format == ValueFormat.Date || right._format == ValueFormat.Date) throw new Exception("Modulus cannot be used with dates.");

			return new Value(_decValue % right._decValue, DecideFormat(_format, right._format));
		}

		public Value Power(Value right)
		{
			// When a value is being raised to the power of a fraction, don't keep it as a fraction.
			// Exponent value must be integer in order to stay a fraction.

			if (IsFractional && right.IsInteger)
			{
				Value ret = Clone();
				ret._num = Math.Round(Convert.ToDecimal(Math.Pow(Convert.ToDouble(ret._num), Convert.ToDouble(right._decValue))));
				ret._denom = Math.Round(Convert.ToDecimal(Math.Pow(Convert.ToDouble(ret._denom), Convert.ToDouble(right._decValue))));
				ret._decValue = ret._num / ret._denom;
				ret.SimplifyFraction();
				return ret;
			}

			//if (_format == ValueFormat.Date || right._format == ValueFormat.Date) throw new Exception("Exponent cannot be used with dates.");

			return new Value(Convert.ToDecimal(Math.Pow(Convert.ToDouble(_decValue), Convert.ToDouble(right._decValue))), DecideFormat(_format, right._format));
		}

		public Value ShiftLeft(Value right)
		{
			return new Value(Convert.ToDecimal(Convert.ToInt32(_decValue) << Convert.ToInt32(right._decValue)), DecideFormat(_format, right._format));
		}

		public Value ShiftRight(Value right)
		{
			return new Value(Convert.ToDecimal(Convert.ToInt32(_decValue) >> Convert.ToInt32(right._decValue)), DecideFormat(_format, right._format));
		}

		public Value BitwiseOr(Value right)
		{
			return new Value(Convert.ToDecimal(Convert.ToInt64(_decValue) | Convert.ToInt64(right._decValue)), DecideFormat(_format, right._format));
		}

		public Value BitwiseXor(Value right)
		{
			return new Value(Convert.ToDecimal(Convert.ToInt64(_decValue) ^ Convert.ToInt64(right._decValue)), DecideFormat(_format, right._format));
		}

		public Value BitwiseAnd(Value right)
		{
			return new Value(Convert.ToDecimal(Convert.ToInt64(_decValue) & Convert.ToInt64(right._decValue)), DecideFormat(_format, right._format));
		}

		public Value BitwiseNot()
		{
			return new Value(Convert.ToDecimal(~Convert.ToInt64(_decValue)), _format);
		}

		public static ValueFormat DecideFormat(ValueFormat leftFormat, ValueFormat rightFormat)
		{
			// When fractions are passed into this function, they will always be changed to another format.
			// This is to avoid problems with non-integer values mixing with fractions.

			if (IsFractionalFormat(leftFormat) && IsFractionalFormat(rightFormat)) return ValueFormat.Default;
			if (IsFractionalFormat(leftFormat)) return rightFormat;
			if (IsFractionalFormat(rightFormat)) return leftFormat;

			if (leftFormat == ValueFormat.Default && rightFormat == ValueFormat.Default) return ValueFormat.Default;
			if (leftFormat == ValueFormat.Default) return rightFormat;
			if (rightFormat == ValueFormat.Default) return leftFormat;

			if (leftFormat == ValueFormat.TimeSpan && rightFormat == ValueFormat.Date) return ValueFormat.Date;

			return leftFormat;
		}

		public static Value CreateFraction(Value numValue, Value denomValue)
		{
			if (denomValue.DecValue == 0) throw new Exception("Division by zero.");

			if ((!numValue.IsInteger && !numValue.IsFractional) ||
				(!denomValue.IsInteger && !denomValue.IsFractional))
			{
				// These values are not integers, so this operator will use the default format instead.
				// In this case the fraction operator will act like the division operator.
				return new Value(numValue.DecValue / denomValue.DecValue, Value.DecideFormat(numValue.Format, denomValue.Format));
			}

			if (numValue.IsFractional)
			{
				// Numerator is fraction, and denominator 'might' be.
				return numValue.Divide(denomValue);
			}
			
			if (denomValue.IsFractional)
			{
				// Numerator is not a fraction but is an integer, and denominator is a fraction.
				return new Value(numValue.DecValue, ValueFormat.Fraction).Divide(denomValue);
			}
			
			// Numerator and denominator are both integers.
			return new Value(numValue.DecValue, denomValue.DecValue, ValueFormat.Fraction);
		}

		private string ToBinString()
		{
			decimal val = Math.Floor(_decValue);
			string ret = "";

			while (val > 0)
			{
				if (val % 2 != 0) ret = "1" + ret;
				else ret = "0" + ret;

				val = Math.Floor(val / 2);
			}

			if (ret == "") ret = "0";
			return "0b" + ret;
		}

		private string ToOctString()
		{
			decimal val = Math.Floor(_decValue);
			string ret = "";
			byte[] data = new byte[1];

			while (val > 0)
			{
				data[0] = (byte)(val % 8 + 48);
				ret = Encoding.ASCII.GetString(data) + ret;
				val = Math.Floor(val / 8);
			}

			if (ret == "") ret = "0";
			return "0o" + ret;
		}

		private string ToHexString()
		{
			decimal val = Math.Floor(_decValue);
			string ret = "";
			byte[] data = new byte[1];
			byte ch;

			while (val > 0)
			{
				ch = Convert.ToByte(val % 16);
				if (ch <= 9) data[0] = (byte)(ch + k_hexZeroOffset);
				else data[0] = (byte)(ch + k_hexAlphaOffset);
				ret = Encoding.ASCII.GetString(data) + ret;
				val = Math.Floor(val / 16);
			}

			if (ret == "") ret = "0";
			return "0x" + ret;
		}


	}
}
