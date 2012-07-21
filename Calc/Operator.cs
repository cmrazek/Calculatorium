using System;
using System.Collections.Generic;
using System.Text;

namespace Calc
{
	public enum OperatorType
	{
		None,

		// Basic
		Add,
		Sub,
		Mul,
		Div,
		Mod,
		Pwr,
		ShiftLeft,
		ShiftRight,
		BitwiseOr,
		BitwiseAnd,
		BitwiseXor,
		BitwiseNot,

		// Assignment
		Assign,
		AddAssign,
		SubAssign,
		MulAssign,
		DivAssign,
		ModAssign,
		PwrAssign,
		ShiftLeftAssign,
		ShiftRightAssign,
		BitwiseOrAssign,
		BitwiseAndAssign,
		BitwiseXorAssign,

		// Fractions
		Fraction,
		Feet,
		Inches,

		// Boolean operators
		Equal,
		NotEqual,
		LessThan,
		LessOrEqual,
		GreaterThan,
		GreaterOrEqual,
		Not,
		And,
		Or,
		Condition1,
		Condition2,

		Comma
	}

	class Operator
	{
		private static Token _token;
		private static Token _parentToken;
		private static Token _leftToken;
		private static Token _leftToken2;
		private static Token _rightToken;

		public static void Execute(Token token, Token parent, Token left2, Token left, Token right)
		{
			_token = token;
			_parentToken = parent;
			_leftToken = left;
			_leftToken2 = left2;
			_rightToken = right;

			switch (token.Operator)
			{
				case OperatorType.Add:
					BinaryOpTest();
					BinaryOp(left.Value.Add(right.Value));
					break;

				case OperatorType.Sub:
					if (right == null || !right.HasValue) throw new Exception("Operator '-' expected value on right.");
					if (left == null || !left.HasValue)
					{
						// Unary minus operator
						UnaryOpTest();
						UnaryOp((new Value(0, ValueFormat.Default)).Subtract(right.Value));
					}
					else
					{
						BinaryOpTest();
						BinaryOp(left.Value.Subtract(right.Value));
					}
					break;

				case OperatorType.Mul:
					BinaryOpTest();
					BinaryOp(left.Value.Multiply(right.Value));
					break;

				case OperatorType.Div:
					BinaryOpTest();
					BinaryOp(left.Value.Divide(right.Value));
					break;

				case OperatorType.Mod:
					BinaryOpTest();
					BinaryOp(left.Value.Modulus(right.Value));
					break;

				case OperatorType.Pwr:
					BinaryOpTest();
					BinaryOp(left.Value.Power(right.Value));
					break;

				case OperatorType.ShiftLeft:
					BinaryOpTest();
					BinaryOp(left.Value.ShiftLeft(right.Value));
					break;

				case OperatorType.ShiftRight:
					BinaryOpTest();
					BinaryOp(left.Value.ShiftRight(right.Value));
					break;

				case OperatorType.BitwiseOr:
					BinaryOpTest();
					BinaryOp(left.Value.BitwiseOr(right.Value));
					break;

				case OperatorType.BitwiseAnd:
					BinaryOpTest();
					BinaryOp(left.Value.BitwiseAnd(right.Value));
					break;

				case OperatorType.BitwiseXor:
					BinaryOpTest();
					BinaryOp(left.Value.BitwiseXor(right.Value));
					break;

				case OperatorType.Assign:
					AssignOpTest();
					AssignOp(right.Value);
					break;

				case OperatorType.AddAssign:
					AssignOpTest();
					AssignOp(left.Value.Add(right.Value));
					break;

				case OperatorType.SubAssign:
					AssignOpTest();
					AssignOp(left.Value.Subtract(right.Value));
					break;

				case OperatorType.MulAssign:
					AssignOpTest();
					AssignOp(left.Value.Multiply(right.Value));
					break;

				case OperatorType.DivAssign:
					AssignOpTest();
					AssignOp(left.Value.Divide(right.Value));
					break;

				case OperatorType.ModAssign:
					AssignOpTest();
					AssignOp(left.Value.Modulus(right.Value));
					break;

				case OperatorType.PwrAssign:
					AssignOpTest();
					AssignOp(left.Value.Power(right.Value));
					break;

				case OperatorType.ShiftLeftAssign:
					AssignOpTest();
					AssignOp(left.Value.ShiftLeft(right.Value));
					break;

				case OperatorType.ShiftRightAssign:
					AssignOpTest();
					AssignOp(left.Value.ShiftRight(right.Value));
					break;

				case OperatorType.BitwiseOrAssign:
					AssignOpTest();
					AssignOp(left.Value.BitwiseOr(right.Value));
					break;

				case OperatorType.BitwiseAndAssign:
					AssignOpTest();
					AssignOp(left.Value.BitwiseAnd(right.Value));
					break;

				case OperatorType.BitwiseXorAssign:
					AssignOpTest();
					AssignOp(left.Value.BitwiseXor(right.Value));
					break;

				case OperatorType.BitwiseNot:
					UnaryOpTest();
					UnaryOp(right.Value.BitwiseNot());
					break;

				case OperatorType.Fraction:
					{
						BinaryOpTest();

						decimal num = left.Value.DecValue;
						decimal denom = right.Value.DecValue;

						if (left2 != null && left2.HasValue)
						{
							decimal whole = left2.Value.DecValue;
							if (whole < 0) num = whole * denom - num;
							else num += whole * denom;

							left2.Enabled = false;
						}

						BinaryOp(new Value(num, denom, ValueFormat.Fraction));
					}
					break;

				case OperatorType.Inches:
					{
						if (left == null || !left.HasValue) throw new Exception("Operator '\"' expected value on left.");

						Value val;
						if (left.Value.IsFractional)
						{
							val = left.Value.Clone();
							val.Format = ValueFormat.Inches;
						}
						else if (left.Value.IsInteger)
						{
							val = Value.CreateFraction(left.Value, new Value(1, ValueFormat.Default));
							val.Format = ValueFormat.Inches;
						}
						else throw new Exception("Operator '\"' expected an integer or fraction on left.");

						SetValue(token, val);
						left.Enabled = false;
					}
					break;

				case OperatorType.Feet:
					{
						if (left == null || !left.HasValue) throw new Exception("Operator ' expected value on left.");
						if (!left.Value.IsFractional & !left.Value.IsInteger) throw new Exception("Operator ' expected an integer or fraction on left.");

						Value val = Value.CreateFraction(left.Value, new Value(1, ValueFormat.Default)).Multiply(new Value(12, ValueFormat.Default));
						val.Format = ValueFormat.Inches;
						left.Enabled = false;

						if (right != null && right.HasValue && (right.Value.Format == ValueFormat.Inches))
						{
							val = val.Add(right.Value);
							right.Enabled = false;
						}

						SetValue(token, val);
					}
					break;

				case OperatorType.Comma:
					// Hack for digit grouping. Only to be performed when not inside a function argument group.
					if (Settings.DigitGrouping && !parent.IsFuncArgs)
					{
						if (left != null && left.HasValue && right != null && right.HasValue)
						{
							Value leftValue = left.Value;
							Value rightValue = right.Value;

							if (leftValue.IsInteger && (leftValue.Format == ValueFormat.Dec || leftValue.Format == ValueFormat.Default) &&
								(rightValue.Format == ValueFormat.Dec || rightValue.Format == ValueFormat.Default))
							{
								SetValue(token, leftValue.Multiply(new Value(1000, ValueFormat.Default)).Add(rightValue));
								left.Enabled = false;
								right.Enabled = false;
							}
						}
					}
					token.Precedence = 0;
					break;

				case OperatorType.Equal:
					BinaryOpTest();
					BinaryOp(new Value(left.Value.DecValue == right.Value.DecValue ? 1 : 0, ValueFormat.Default));
					break;

				case OperatorType.NotEqual:
					BinaryOpTest();
					BinaryOp(new Value(left.Value.DecValue != right.Value.DecValue ? 1 : 0, ValueFormat.Default));
					break;

				case OperatorType.LessThan:
					BinaryOpTest();
					BinaryOp(new Value(left.Value.DecValue < right.Value.DecValue ? 1 : 0, ValueFormat.Default));
					break;

				case OperatorType.LessOrEqual:
					BinaryOpTest();
					BinaryOp(new Value(left.Value.DecValue <= right.Value.DecValue ? 1 : 0, ValueFormat.Default));
					break;

				case OperatorType.GreaterThan:
					BinaryOpTest();
					BinaryOp(new Value(left.Value.DecValue > right.Value.DecValue ? 1 : 0, ValueFormat.Default));
					break;

				case OperatorType.GreaterOrEqual:
					BinaryOpTest();
					BinaryOp(new Value(left.Value.DecValue >= right.Value.DecValue ? 1 : 0, ValueFormat.Default));
					break;

				case OperatorType.Not:
					UnaryOpTest();
					UnaryOp(new Value(right.Value.DecValue == 0 ? 1 : 0, ValueFormat.Default));
					break;

				case OperatorType.And:
					BinaryOpTest();
					BinaryOp(new Value(left.Value.DecValue != 0 && right.Value.DecValue != 0 ? 1 : 0, ValueFormat.Default));
					break;

				case OperatorType.Or:
					BinaryOpTest();
					BinaryOp(new Value(left.Value.DecValue != 0 || right.Value.DecValue != 0 ? 1 : 0, ValueFormat.Default));
					break;

				case OperatorType.Condition1:
					BinaryOpTest();
					if (left.Value.DecValue != 0) SetValue(token, new Value(1, ValueFormat.Default));
					else SetValue(token, new Value(0, ValueFormat.Default));
					left.Enabled = false;
					break;

				case OperatorType.Condition2:
					if (left == null || !left.HasValue) throw new Exception("Operator ':' expected value on left.");
					if (right == null || !right.HasValue) throw new Exception("Operator ':' expected value on right.");
					if (left2 == null || left2.Type != TokenType.Operator || left2.Operator != OperatorType.Condition1) throw new Exception("Operator ':' expected '?' to the left.");
					SetValue(token, left2.Value.DecValue != 0 ? left.Value.Clone() : right.Value.Clone());
					left.Enabled = left2.Enabled = right.Enabled = false;
					break;
			}
		}

		private static void BinaryOpTest()
		{
			if (_leftToken == null || !_leftToken.HasValue) throw new Exception("Operator '" + _token.Text + "' expected value on left.");
			if (_rightToken == null || !_rightToken.HasValue) throw new Exception("Operator '" + _token.Text + "' expected value on right.");
		}

		private static void BinaryOp(Value val)
		{
			SetValue(_token, val);
			_leftToken.Enabled = _rightToken.Enabled = false;
		}

		private static void AssignOpTest()
		{
			if (_leftToken == null || !_leftToken.CanSetValue) throw new Exception("Operator '" + _token.Text + "' expected variable on left.");
			if (_rightToken == null || !_rightToken.HasValue) throw new Exception("Operator '" + _token.Text + "' expected value on right.");
		}

		private static void AssignOp(Value val)
		{
			SetValue(_leftToken, val);
			_rightToken.Enabled = _token.Enabled = false;
		}

		private static void UnaryOpTest()
		{
			if (_rightToken == null || !_rightToken.HasValue) throw new Exception("Operator '" + _token.Text + "' expected value on right.");
		}

		private static void UnaryOp(Value val)
		{
			SetValue(_token, val);
			_rightToken.Enabled = false;
		}

		private static void SetValue(Token tok, Value val)
		{
			tok.Value = val;
			//tok.Type = TokenType.None;
			tok.Precedence = 0;
		}
	}
}
