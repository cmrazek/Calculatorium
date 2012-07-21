using System;
using System.Collections.Generic;
using System.Text;

namespace Calc
{
	//public delegate Value FunctionProc(Value[] args);

	class Function
	{

		public static bool IsFunction(string funcName)
		{
			switch (funcName.ToLower())
			{
				case "abs":
				case "acos":
				case "asin":
				case "atan":
				case "bin":
				case "case":
				case "ceil":
				case "cos":
				case "cosh":
				case "day":
				case "days":
				case "dec":
				case "floor":
				case "hex":
				case "hour":
				case "hours":
				case "if":
				case "lg":
				case "ln":
				case "log":
				case "makedate":
				case "max":
				case "min":
				case "minute":
				case "minutes":
				case "month":
				case "msec":
				case "msecs":
				case "oct":
				case "pow":
				case "rand":
				case "round":
				case "sci":
				case "second":
				case "seconds":
				case "sin":
				case "sinh":
				case "sqrt":
				case "tan":
				case "tanh":
				case "xor":
				case "year":
					return true;

				default:
					return false;
			}
		}

		public static Value Execute(string funcName, Value[] args)
		{
			switch (funcName.ToLower())
			{
				case "abs":
					if (args.Length != 1) throw new Exception("'abs' requires 1 argument.");
					return new Value(Math.Abs(args[0].DecValue), args[0].Format);

				case "acos":
					if (args.Length != 1) throw new Exception("'acos' requires 1 argument.");
					return new Value(Util.ToAngle(Math.Acos(args[0].DblValue)), args[0].Format);

				case "asin":
					if (args.Length != 1) throw new Exception("'asin' requires 1 argument.");
					return new Value(Util.ToAngle(Math.Asin(args[0].DblValue)), args[0].Format);

				case "atan":
					if (args.Length != 1) throw new Exception("'atan' requires 1 argument.");
					return new Value(Util.ToAngle(Math.Atan(args[0].DblValue)), args[0].Format);

				case "bin":
					if (args.Length != 1) throw new Exception("'bin' requires 1 argument.");
					return new Value(args[0].DecValue, ValueFormat.Bin);

				case "case":
					{
						if (args.Length < 4) throw new Exception("'case' requires at least 4 arguments.");
						if (args.Length % 2 != 0) throw new Exception("'case' requires an even number of arguments.");
						for (int i = 1, ii = args.Length - 1; i < ii; i += 2)
						{
							if (args[i].DecValue == args[0].DecValue) return args[i + 1].Clone();
						}
						return args[args.Length - 1].Clone();
					}

				case "ceil":
					if (args.Length != 1) throw new Exception("'ceil' requires 1 argument.");
					return new Value(Math.Ceiling(args[0].DecValue), args[0].Format);

				case "cos":
					if (args.Length != 1) throw new Exception("'cos' requires 1 argument.");
					return new Value(Math.Cos(Util.ToRads(args[0].DblValue)), args[0].Format);

				case "cosh":
					if (args.Length != 1) throw new Exception("'cosh' requires 1 argument.");
					return new Value(Math.Cosh(Util.ToRads(args[0].DblValue)), args[0].Format);

				case "day":
					{
						if (args.Length != 1) throw new Exception("'day' requires 1 argument.");
						DateTime dt = new DateTime(args[0].LongValue);
						return new Value(dt.Day, ValueFormat.Dec);
					}

				case "days":
					{
						if (args.Length != 1) throw new Exception("'days' requires 1 argument.");
						return new Value(args[0].LongValue / 864000000000, ValueFormat.Dec);
					}

				case "dec":
					if (args.Length != 1) throw new Exception("'dec' requires 1 argument.");
					return new Value(args[0].DecValue, ValueFormat.Dec);

				case "floor":
					if (args.Length != 1) throw new Exception("'floor' requires 1 argument.");
					return new Value(Math.Floor(args[0].DecValue), args[0].Format);

				case "hex":
					if (args.Length != 1) throw new Exception("'hex' requires 1 argument.");
					return new Value(args[0].DecValue, ValueFormat.Hex);

				case "hour":
					{
						if (args.Length != 1) throw new Exception("'hour' requires 1 argument.");
						DateTime dt = new DateTime(args[0].LongValue);
						return new Value(dt.Hour, ValueFormat.Dec);
					}

				case "hours":
					{
						if (args.Length != 1) throw new Exception("'hours' requires 1 argument.");
						return new Value(args[0].LongValue / 36000000000, ValueFormat.Dec);
					}

				case "if":
					if (args.Length < 3) throw new Exception("'if' requires at least 3 arguments.");
					if (args.Length % 2 == 0) throw new Exception("'if' requires an odd number of arguments.");
					for (int i = 0, ii = args.Length - 1; i < ii; i += 2)
					{
						if (args[i].DecValue != 0) return args[i + 1].Clone();
					}
					return args[args.Length - 1].Clone();

				case "lg":
					if (args.Length != 1) throw new Exception("'lg' requires 1 argument.");
					return new Value(Math.Log(args[0].DblValue, 2), args[0].Format);

				case "ln":
					if (args.Length != 1) throw new Exception("'ln' requires 1 argument.");
					return new Value(Math.Log10(args[0].DblValue), args[0].Format);

				case "log":
					if (args.Length == 1)
					{
						return new Value(Math.Log10(args[0].DblValue), args[0].Format);
					}
					else if (args.Length == 2)
					{
						return new Value(Math.Log(args[0].DblValue, args[1].DblValue), Value.DecideFormat(args[0].Format, args[1].Format));
					}
					else throw new Exception("'log' requires 1 or 2 arguments.");

				case "makedate":
					{
						if (args.Length < 3 || args.Length > 7) throw new Exception("'makedate' requires between 3 and 7 arguments.");
						if (!args[0].IsInteger) throw new Exception("'makedate' year must be an integer.");
						if (!args[1].IsInteger) throw new Exception("'makedate' month must be an integer.");
						if (!args[2].IsInteger) throw new Exception("'makedate' day must be an integer.");
						if (args.Length > 3 && !args[3].IsInteger) throw new Exception("'makedate' hour must be an integer.");
						if (args.Length > 4 && !args[4].IsInteger) throw new Exception("'makedate' minute must be an integer.");
						if (args.Length > 5 && !args[5].IsInteger) throw new Exception("'makedate' second must be an integer.");
						if (args.Length > 6 && !args[6].IsInteger) throw new Exception("'makedate' millisecond must be an integer.");

						DateTime dt;
						try
						{
							dt = new DateTime(args[0].IntValue, args[1].IntValue, args[2].IntValue,
								(args.Length > 3 ? args[3].IntValue : 0),
								(args.Length > 4 ? args[4].IntValue : 0),
								(args.Length > 5 ? args[5].IntValue : 0),
								(args.Length > 6 ? args[6].IntValue : 0));
						}
						catch (Exception)
						{
							throw new Exception("Invalid date.");
						}

						return new Value(dt.Ticks, ValueFormat.Date);
					}

				case "max":
					{
						if (args.Length < 2) throw new Exception("'max' requires 2 or more arguments.");
						decimal m = 0;
						bool first = true;
						foreach (Value arg in args)
						{
							if (first || arg.DecValue > m)
							{
								m = arg.DecValue;
								first = false;
							}
						}
						return new Value(m, args[0].Format);
					}

				case "min":
					{
						if (args.Length < 2) throw new Exception("'min' requires 2 or more arguments.");
						decimal m = 0;
						bool first = true;
						foreach (Value arg in args)
						{
							if (first || arg.DecValue < m)
							{
								m = arg.DecValue;
								first = false;
							}
						}
						return new Value(m, args[0].Format);
					}

				case "minute":
					{
						if (args.Length != 1) throw new Exception("'minute' requires 1 argument.");
						DateTime dt = new DateTime(args[0].LongValue);
						return new Value(dt.Minute, ValueFormat.Dec);
					}

				case "minutes":
					{
						if (args.Length != 1) throw new Exception("'minutes' requires 1 argument.");
						return new Value(args[0].LongValue / 600000000, ValueFormat.Dec);
					}

				case "month":
					{
						if (args.Length != 1) throw new Exception("'month' requires 1 argument.");
						DateTime dt = new DateTime(args[0].LongValue);
						return new Value(dt.Month, ValueFormat.Dec);
					}

				case "msec":
					{
						if (args.Length != 1) throw new Exception("'msec' requires 1 argument.");
						DateTime dt = new DateTime(args[0].LongValue);
						return new Value(dt.Millisecond, ValueFormat.Dec);
					}

				case "msecs":
					{
						if (args.Length != 1) throw new Exception("'msecs' requires 1 argument.");
						return new Value(args[0].LongValue / 10000, ValueFormat.Dec);
					}

				case "oct":
					if (args.Length != 1) throw new Exception("'oct' requires 1 argument.");
					return new Value(args[0].DecValue, ValueFormat.Oct);

				case "pow":
					if (args.Length != 2) throw new Exception("'pow' requires 2 arguments.");
					return args[0].Power(args[1]);

				case "rand":
					if (args.Length == 0)
					{
						return new Value(Settings.Rand.NextDouble(), ValueFormat.Default);
					}
					else if (args.Length == 1)
					{
						return new Value(Settings.Rand.NextDouble() * args[0].DblValue, args[0].Format);
					}
					else if (args.Length == 2)
					{
						return new Value(Settings.Rand.NextDouble() * (args[1].DblValue - args[0].DblValue) + args[0].DblValue, Value.DecideFormat(args[0].Format, args[1].Format));
					}
					else throw new Exception("'rand' requires 0, 1 or 2 arguments.");

				case "round":
					if (args.Length == 1)
					{
						return new Value(Math.Round(args[0].DecValue, MidpointRounding.AwayFromZero), args[0].Format);
					}
					else if (args.Length == 2)
					{
						int numDecimals = args[1].IntValue;
						if (numDecimals < 0 || numDecimals > 20) throw new Exception("'round' number of decimals must be between 0 and 20.");
						return new Value(Math.Round(args[0].DecValue, args[1].IntValue, MidpointRounding.AwayFromZero), args[0].Format);
					}
					else throw new Exception("'round' requires 1 or 2 arguments.");

				case "sci":
					if (args.Length != 1) throw new Exception("'sci' requires 1 argument.");
					return new Value(args[0].DecValue, ValueFormat.Sci);

				case "second":
					{
						if (args.Length != 1) throw new Exception("'second' requires 1 argument.");
						DateTime dt = new DateTime(args[0].LongValue);
						return new Value(dt.Second, ValueFormat.Dec);
					}

				case "seconds":
					{
						if (args.Length != 1) throw new Exception("'seconds' requires 1 argument.");
						return new Value(args[0].LongValue / (10000000), ValueFormat.Dec);
					}

				case "sin":
					if (args.Length != 1) throw new Exception("'sin' requires 1 argument.");
					return new Value(Math.Sin(Util.ToRads(args[0].DblValue)), args[0].Format);

				case "sinh":
					if (args.Length != 1) throw new Exception("'sinh' requires 1 argument.");
					return new Value(Math.Sinh(Util.ToRads(args[0].DblValue)), args[0].Format);

				case "sqrt":
					if (args.Length != 1) throw new Exception("'sqrt' requires 1 argument.");
					if (args[0].DblValue < 0) throw new Exception("Cannot calculate the square-root of a negative number.");
					return new Value(Math.Sqrt(args[0].DblValue), args[0].Format);

				case "tan":
					if (args.Length != 1) throw new Exception("'tan' requires 1 argument.");
					return new Value(Math.Tan(Util.ToRads(args[0].DblValue)), args[0].Format);

				case "tanh":
					if (args.Length != 1) throw new Exception("'tanh' requires 1 argument.");
					return new Value(Math.Tanh(Util.ToRads(args[0].DblValue)), args[0].Format);

				case "xor":
					if (args.Length != 2) throw new Exception("'xor' requires 2 arguments.");
					return args[0].BitwiseXor(args[1]);

				case "year":
					{
						if (args.Length != 1) throw new Exception("'year' requires 1 argument.");
						DateTime dt = new DateTime(args[0].LongValue);
						return new Value(dt.Year, ValueFormat.Dec);
					}

				default:
					throw new Exception("Unknown function '" + funcName + "'.");
			}
		}

	}
}
