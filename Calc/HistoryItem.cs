using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Calc
{
	public class HistoryItem
	{
		private HistoryType _type;
		private string _text;
		private HistoryLook _look;

		private int _index = -1;
		private Rectangle _itemRect;
		private Rectangle _textRect;
		private bool _selected = false;

		public HistoryItem(HistoryType type, string text, HistoryLook look)
		{
			#if DEBUG
			if (look == null) throw new ArgumentNullException();
			#endif

			_type = type;
			_text = text;
			_look = look;
		}

		public HistoryType Type
		{
			get { return _type; }
		}

		public string Text
		{
			get { return _text; }
		}

		public int Index
		{
			get { return _index; }
			set { _index = value; }
		}

		public int Top
		{
			get { return _itemRect.Top; }
		}

		public int Height
		{
			get { return _itemRect.Height; }
		}

		public int Bottom
		{
			get { return _itemRect.Bottom; }
		}

		public void Layout(int top, int clientWidth)
		{
			Size proposedSize = new Size(clientWidth - _look.MarginLeft - _look.MarginRight, Int32.MaxValue);
			Size requiredSize = TextRenderer.MeasureText(_text, _look.Font, proposedSize, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix | _look.TextAlign);

			_textRect = new Rectangle(_look.MarginLeft, top + _look.MarginTop, requiredSize.Width, requiredSize.Height);
			_itemRect = new Rectangle(0, top, clientWidth, requiredSize.Height + _look.MarginTop + _look.MarginBottom);

			if (_look.TextAlign == TextFormatFlags.Right)
			{
				int diff = _itemRect.Right - _look.MarginRight - _textRect.Right;
				if (diff != 0) _textRect.Offset(diff, 0);
			}
			else if (_look.TextAlign == TextFormatFlags.HorizontalCenter)
			{
				int totalTextWidth = _itemRect.Width - _look.MarginLeft - _look.MarginRight;
				if (totalTextWidth > 0)
				{
					int diff = totalTextWidth - _textRect.Width;
					if (diff > 1) _textRect.Offset(diff / 2, 0);
				}
			}
		}

		public void Render(Graphics g, int viewTop, int viewHeight)
		{
			Rectangle itemRect = _itemRect;
			Rectangle textRect = _textRect;
			itemRect.Offset(0, 0 - viewTop);
			textRect.Offset(0, 0 - viewTop);

			Brush brush;
			Color textColor;
			if (_selected)
			{
				brush = SystemBrushes.Highlight;
				textColor = SystemColors.HighlightText;
			}
			else
			{
				brush = new LinearGradientBrush(itemRect, _look.BackgroundColor1, _look.BackgroundColor2,
					LinearGradientMode.Vertical);
				//brush = new LinearGradientBrush(itemRect, HistoryView.BackgroundColor1, HistoryView.BackgroundColor2,
				//	LinearGradientMode.Vertical);
				textColor = _look.TextColor;
			}

			g.FillRectangle(brush, itemRect);

			TextRenderer.DrawText(g, _text, _look.Font, textRect, textColor, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix | _look.TextAlign);
		}

		public bool Selected
		{
			get { return _selected; }
			set { _selected = value; }
		}
	}
}
