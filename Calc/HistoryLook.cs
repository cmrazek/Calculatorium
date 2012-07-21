using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

namespace Calc
{
	public class HistoryLook
	{
		// Constants
		private const int k_defaultMarginLeft = 2;
		private const int k_defaultMarginTop = 2;
		private const int k_defaultMarginRight = 2;
		private const int k_defaultMarginBottom = 2;
		private static Color k_defaultBackgroundColor = Color.White;

		// Member variables
		HistoryType _type;
		private bool _custom;
		private Font _font;
		private Color _textColor;
		private TextFormatFlags _textAlign;
		private int _marginLeft;
		private int _marginTop;
		private int _marginRight;
		private int _marginBottom;
		private Color _backgroundColor1;
		private Color _backgroundColor2;

		public HistoryLook(HistoryType type, bool custom)
		{
			_type = type;
			_custom = custom;
			_font = SystemFonts.DefaultFont;
			_textColor = SystemColors.WindowText;
			_textAlign = TextFormatFlags.Left;
			_marginLeft = 2;
			_marginTop = 2;
			_marginRight = 2;
			_marginBottom = 2;
			_backgroundColor1 = k_defaultBackgroundColor;
			_backgroundColor2 = k_defaultBackgroundColor;
		}

		public HistoryLook(HistoryLook copy)
		{
			Copy(copy);
		}

		public void Copy(HistoryLook copy)
		{
			_custom = copy._custom;
			_font = copy._font;
			_textColor = copy._textColor;
			_textAlign = copy._textAlign;
			_marginLeft = copy._marginLeft;
			_marginTop = copy._marginTop;
			_marginRight = copy._marginRight;
			_marginBottom = copy._marginBottom;
			_backgroundColor1 = copy._backgroundColor1;
			_backgroundColor2 = copy._backgroundColor2;
		}

		public HistoryLook Clone()
		{
			return new HistoryLook(this);
		}

		public bool Custom
		{
			get { return _custom; }
			set { _custom = value; }
		}

		public Font Font
		{
			get { return _font; }
			set { _font = value; }
		}

		public Color TextColor
		{
			get { return _textColor; }
			set { _textColor = value; }
		}

		public TextFormatFlags TextAlign
		{
			get { return _textAlign; }
			set
			{
				switch (value)
				{
					case TextFormatFlags.Left:
					case TextFormatFlags.HorizontalCenter:
					case TextFormatFlags.Right:
						_textAlign = value;
						break;

					default:
						throw new Exception("Text alignment can only be set to left, center or right.");
				}
			}
		}

		public int MarginLeft
		{
			get { return _marginLeft; }
			set { _marginLeft = value; }
		}

		public int MarginTop
		{
			get { return _marginTop; }
			set { _marginTop = value; }
		}

		public int MarginRight
		{
			get { return _marginRight; }
			set { _marginRight = value; }
		}

		public int MarginBottom
		{
			get { return _marginBottom; }
			set { _marginBottom = value; }
		}

		public void Save(XmlTextWriter xml)
		{
			xml.WriteStartElement("HistoryLook");
			xml.WriteElementString("Type", _type.ToString());
			xml.WriteElementString("FontFamily", _font.FontFamily.Name.ToString());
			xml.WriteElementString("FontSize", _font.Size.ToString());
			xml.WriteElementString("FontStyle", _font.Style.ToString());
			if (_textColor != SystemColors.WindowText) xml.WriteElementString("TextColor", Util.ColorToString(_textColor));
			if (_textAlign != TextFormatFlags.Left)
			{
				if ((_textAlign & TextFormatFlags.Left) != 0) xml.WriteElementString("TextAlign", "Left");
				else if ((_textAlign & TextFormatFlags.Right) != 0) xml.WriteElementString("TextAlign", "Right");
				else if ((_textAlign & TextFormatFlags.HorizontalCenter) != 0) xml.WriteElementString("TextAlign", "Center");
			}
			if (_marginLeft != k_defaultMarginLeft) xml.WriteElementString("MarginLeft", _marginLeft.ToString());
			if (_marginTop != k_defaultMarginTop) xml.WriteElementString("MarginTop", _marginTop.ToString());
			if (_marginRight != k_defaultMarginRight) xml.WriteElementString("MarginRight", _marginRight.ToString());
			if (_marginBottom != k_defaultMarginBottom) xml.WriteElementString("MarginBottom", _marginBottom.ToString());
			if (_backgroundColor1 != k_defaultBackgroundColor) xml.WriteElementString("BackgroundColor1", _backgroundColor1.Name);
			if (_backgroundColor2 != k_defaultBackgroundColor) xml.WriteElementString("BackgroundColor2", _backgroundColor2.Name);
			xml.WriteEndElement();
		}

		public void Load(XmlElement xml)
		{
			if (xml["FontFamily"] != null && xml["FontSize"] != null)
			{
				string familyName = xml["FontFamily"].InnerText;
				float size = Util.StringToFloat(xml["FontSize"].InnerText, SystemFonts.DefaultFont.Size);

				FontStyle style;
				if (xml["FontStyle"] != null)
				{
					try
					{
						style = (FontStyle)Enum.Parse(typeof(FontStyle), xml["FontStyle"].InnerText);
					}
					catch (Exception)
					{
						style = FontStyle.Regular;
					}
				}
				else
				{
					style = FontStyle.Regular;
				}

				try
				{
					_font = new Font(familyName, size, style);
				}
				catch (Exception)
				{
					_font = SystemFonts.DefaultFont;
				}
			}
			else
			{
				_font = SystemFonts.DefaultFont;
			}

			_marginLeft = LoadInt(xml["MarginLeft"], k_defaultMarginLeft);
			_marginTop = LoadInt(xml["MarginTop"], k_defaultMarginTop);
			_marginRight = LoadInt(xml["MarginRight"], k_defaultMarginRight);
			_marginBottom = LoadInt(xml["MarginBottom"], k_defaultMarginBottom);

			if (xml["TextColor"] != null) _textColor = Util.StringToColor(xml["TextColor"].InnerText, SystemColors.WindowText);

			if (xml["TextAlign"] != null)
			{
				switch (xml["TextAlign"].InnerText)
				{
					case "Right": _textAlign = TextFormatFlags.Right; break;
					case "Center": _textAlign = TextFormatFlags.HorizontalCenter; break;
					default: _textAlign = TextFormatFlags.Left; break;
				}
			}

			if (xml["BackgroundColor1"] != null) _backgroundColor1 = Util.StringToColor(xml["BackgroundColor1"].InnerText, _backgroundColor1);
			if (xml["BackgroundColor2"] != null) _backgroundColor2 = Util.StringToColor(xml["BackgroundColor2"].InnerText, _backgroundColor2);
		}

		private int LoadInt(XmlElement element, int defaultValue)
		{
			try
			{
				if (element == null) return defaultValue;
				return Convert.ToInt32(element.InnerText);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		public Color BackgroundColor1
		{
			get { return _backgroundColor1; }
			set { _backgroundColor1 = value; }
		}

		public Color BackgroundColor2
		{
			get { return _backgroundColor2; }
			set { _backgroundColor2 = value; }
		}
	}
}
