using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Drawing.Drawing2D;

namespace Calc
{
	public enum HistoryType
	{
		Echo,
		Info,
		Answer,
		Error,
		HelpTopic,
		HelpBody
	}

	public partial class HistoryView : UserControl
	{
		// Constants
		public const int k_scrollSmallChange = 100;

		// Static Variables
		private static Color _backgroundColor1 = SystemColors.Window;
		private static Color _backgroundColor2 = SystemColors.Window;

		// Member Variables
		private List<HistoryItem> _history = new List<HistoryItem>();
		private Dictionary<HistoryType, HistoryLook> _look = new Dictionary<HistoryType, HistoryLook>();
		private Dictionary<HistoryType, HistoryLook> _defaultLook = new Dictionary<HistoryType, HistoryLook>();
		private List<HistoryItem> _selectedItems = new List<HistoryItem>();
		private int _focusItemIndex = -1;
		private Control _focusControl = null;

		public event EventHandler SelectionChanged = null;
		public event EventHandler ItemActivate = null;
		public event EventHandler SelectPastBottom = null;

		public HistoryView()
		{
			InitializeComponent();
			CalcColors();

			TabStop = true;

			MouseWheel += new MouseEventHandler(HistoryView_MouseWheel);
		}

		private void HistoryView_Load(object sender, EventArgs e)
		{
			RecalculateScroll(true);
		}

		private void CalcColors()
		{
			_backgroundColor1 = SystemColors.Window;
			_backgroundColor2 = MixColor(SystemColors.Window, SystemColors.MenuBar, .3f);
		}

		private static Color MixColor(Color color1, Color color2, float ratio)
		{
			float r1 = (float)color1.R, r2 = (float)color2.R;
			float g1 = (float)color1.G, g2 = (float)color2.G;
			float b1 = (float)color1.B, b2 = (float)color2.B;

			int r = Convert.ToInt32(r1 * (1.0f - ratio) + r2 * ratio);
			int g = Convert.ToInt32(g1 * (1.0f - ratio) + g2 * ratio);
			int b = Convert.ToInt32(b1 * (1.0f - ratio) + b2 * ratio);

			if (r < 0) r = 0; else if (r > 255) r = 255;
			if (g < 0) g = 0; else if (g > 255) g = 255;
			if (b < 0) b = 0; else if (b > 255) b = 255;

			return Color.FromArgb(r, g, b);
		}

		public void Add(HistoryType type, string text)
		{
			Debug.WriteLine("Adding history [" + type.ToString() + "]: " + text);

			HistoryItem item = new HistoryItem(type, text, GetHistoryLook(type));
			AddHistoryItem(item, true);
			Invalidate();
		}

		private void AddHistoryItem(HistoryItem item, bool recalcScroll)
		{
			item.Index = _history.Count;

			int top;
			if (_history.Count != 0)
			{
				HistoryItem lastItem = _history[_history.Count - 1];
				top = lastItem.Top + lastItem.Height;
			}
			else
			{
				top = 0;
			}

			item.Layout(top, ViewWidth);
			_history.Add(item);

			if (recalcScroll) RecalculateScroll(false);
		}

		public void RecalcLayout(bool redraw)
		{
			RecalculateScroll(true);
			if (redraw) Invalidate();
		}

		private void RecalculateScroll(bool clientRectChanged)
		{
			int docHeight = 0;
			int clientWidth = ViewWidth;

			foreach (HistoryItem historyItem in _history)
			{
				if (clientRectChanged)
				{
					historyItem.Layout(docHeight, clientWidth);
				}
				docHeight += historyItem.Height;
			}

			vScroll.Minimum = 0;
			vScroll.Maximum = docHeight;

			vScroll.SmallChange = k_scrollSmallChange;
			vScroll.LargeChange = ClientRectangle.Height;

			if (vScroll.Value + vScroll.LargeChange > vScroll.Maximum)
			{
				int newValue = vScroll.Maximum - vScroll.LargeChange;
				if (newValue < 0) newValue = 0;
				vScroll.Value = newValue;
			}

			vScroll.Enabled = vScroll.LargeChange < docHeight;
		}

		public void ScrollToBottom()
		{
			int newValue = vScroll.Maximum - ClientRectangle.Height;
			if (newValue < 0) newValue = 0;
			vScroll.Value = newValue;
			Invalidate();
		}

		private void HistoryView_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			int viewTop = vScroll.Value;
			int viewHeight = ClientRectangle.Height;
			int clipTop = e.ClipRectangle.Top + viewTop;
			int clipBottom = e.ClipRectangle.Bottom + viewTop + viewHeight;

			foreach (HistoryItem item in _history)
			{
				if (item.Bottom <= clipTop) continue;
				if (item.Top > clipBottom) break;

				item.Render(g, viewTop, viewHeight);
			}
		}

		public HistoryLook GetHistoryLook(HistoryType type)
		{
			if (!_look.ContainsKey(type))
			{
				HistoryLook look = new HistoryLook(type, true);
				_look.Add(type, look);
				return look;
			}

			return _look[type];
		}

		public HistoryLook GetHistoryDefaultLook(HistoryType type)
		{
			if (!_defaultLook.ContainsKey(type)) return new HistoryLook(type, true);
			return _defaultLook[type];
		}

		public int Count
		{
			get { return _history.Count; }
		}

		public string ItemText(int index)
		{
			if (index < 0 || index >= _history.Count) throw new ArgumentOutOfRangeException();
			return _history[index].Text;
		}

		public HistoryType ItemType(int index)
		{
			if (index < 0 || index >= _history.Count) throw new ArgumentOutOfRangeException();
			return _history[index].Type;
		}

		public void Clear()
		{
			_history.Clear();
			RecalculateScroll(false);
			Invalidate();
		}

		private void vScroll_Scroll(object sender, ScrollEventArgs e)
		{
			Invalidate();
		}

		void HistoryView_MouseWheel(object sender, MouseEventArgs e)
		{
			NotifyMouseWheel(e.Delta);
		}

		public void NotifyMouseWheel(int delta)
		{
			if (delta != 0)
			{
				int newValue = vScroll.Value - vScroll.SmallChange * (delta / 120);
				if (newValue > vScroll.Maximum - vScroll.LargeChange) newValue = vScroll.Maximum - vScroll.LargeChange;
				if (newValue < vScroll.Minimum) newValue = vScroll.Minimum;
				vScroll.Value = newValue;
				Invalidate();
			}
		}

		private void HistoryView_Resize(object sender, EventArgs e)
		{
			RecalculateScroll(true);
			Invalidate();
		}

		public void SaveSettings(XmlTextWriter xml)
		{
			xml.WriteStartElement("History");

			foreach (HistoryType type in _look.Keys)
			{
				HistoryLook look = _look[type];
				if (look.Custom) look.Save(xml);
			}

			foreach (HistoryItem h in _history)
			{
				xml.WriteStartElement("HistoryItem");
				xml.WriteAttributeString("Type", h.Type.ToString());
				xml.WriteString(h.Text);
				xml.WriteEndElement();
			}

			xml.WriteEndElement();
		}

		public void LoadSettings(XmlDocument xml, bool custom)
		{
			foreach (XmlElement xmlLook in xml.GetElementsByTagName("HistoryLook"))
			{
				if (xmlLook["Type"] == null) continue;

				HistoryType type;
				try
				{
					type = (HistoryType)Enum.Parse(typeof(HistoryType), xmlLook["Type"].InnerText);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Failed to convert 'Type' element into HistoryType enum: " + ex.Message);
					continue;
				}

				HistoryLook look = new HistoryLook(type, custom);
				look.Load(xmlLook);
				_look[type] = look;
				if (!custom) _defaultLook[type] = look.Clone();
			}

			foreach (XmlElement h in xml.GetElementsByTagName("HistoryItem"))
			{
				if (!h.HasAttribute("Type")) continue;

				HistoryType type;
				try
				{
					type = (HistoryType)Enum.Parse(typeof(HistoryType), h.GetAttribute("Type"));
				}
				catch (Exception)
				{
					continue;
				}

				HistoryItem item = new HistoryItem(type, h.InnerText, GetHistoryLook(type));
				AddHistoryItem(item, false);
			}

			RecalculateScroll(false);
			ScrollToBottom();
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

		private void HistoryView_SystemColorsChanged(object sender, EventArgs e)
		{
			CalcColors();
		}

		public void SelectItem(int index, bool redraw)
		{
			if (index < 0 || index >= _history.Count) throw new ArgumentOutOfRangeException();

			SelectNone(false);

			HistoryItem item = _history[index];
			item.Selected = true;
			_selectedItems.Add(item);

			if (SelectionChanged != null) SelectionChanged(this, new EventArgs());
			if (redraw) Invalidate();
		}

		public void SelectItems(int[] indices, bool redraw)
		{
			SelectNone(false);

			foreach (int i in indices)
			{
				if (i < 0 || i >= _history.Count) throw new ArgumentOutOfRangeException();

				HistoryItem item = _history[i];
				item.Selected = true;
				_selectedItems.Add(item);
			}

			if (SelectionChanged != null) SelectionChanged(this, new EventArgs());
			if (redraw) Invalidate();
		}

		public void SelectRange(int start, int end, bool redraw)
		{
			if (start < 0 || start >= _history.Count || end < 0 || end >= _history.Count) throw new ArgumentOutOfRangeException();

			SelectNone(false);

			if (start > end)
			{
				int temp = start;
				start = end;
				end = temp;
			}

			for (int i = start; i <= end; i++)
			{
				HistoryItem item = _history[i];
				item.Selected = true;
				_selectedItems.Add(item);
			}

			if (SelectionChanged != null) SelectionChanged(this, new EventArgs());
			if (redraw) Invalidate();
		}

		public void ToggleSelect(int index, bool redraw)
		{
			if (index < 0 || index >= _history.Count) throw new ArgumentOutOfRangeException();

			HistoryItem item = _history[index];
			if (item.Selected)
			{
				_selectedItems.Remove(item);
				item.Selected = false;
			}
			else
			{
				_selectedItems.Add(item);
				item.Selected = true;
			}

			if (SelectionChanged != null) SelectionChanged(this, new EventArgs());
			if (redraw) Invalidate();
		}

		public int SelectedCount
		{
			get { return _selectedItems.Count; }
		}

		public int[] SelectedItems
		{
			get
			{
				List<int> ret = new List<int>();
				foreach (HistoryItem h in _selectedItems) ret.Add(h.Index);
				ret.Sort();
				return ret.ToArray();
			}
		}

		public int LastSelected
		{
			get
			{
				int max = -1;
				foreach (HistoryItem h in _selectedItems)
				{
					if (max == -1 || h.Index > max) max = h.Index;
				}
				return max;
			}
		}

		public int FirstSelected
		{
			get
			{
				int min = -1;
				foreach (HistoryItem h in _selectedItems)
				{
					if (min == -1 || h.Index < min) min = h.Index;
				}
				return min;
			}
		}

		public void SelectNone(bool redraw)
		{
			foreach (HistoryItem h in _selectedItems) h.Selected = false;
			_selectedItems.Clear();

			if (redraw) Invalidate();
		}

		public void SelectAll(bool redraw)
		{
			SelectNone(false);

			foreach (HistoryItem h in _history)
			{
				_selectedItems.Add(h);
				h.Selected = true;
			}

			if (redraw) Invalidate();
		}

		public int HitTest(int y, bool docCoords)
		{
			if (_history.Count == 0) return -1;

			int i = 0;
			int yOffset = docCoords ? y : y + vScroll.Value;

			if (yOffset < 0) return 0;

			foreach (HistoryItem h in _history)
			{
				if (h.Top <= yOffset && yOffset - h.Top < h.Height) return i;
				i++;
			}

			return _history.Count - 1;
		}

		public int HitTest(Point pt)
		{
			return HitTest(pt.Y, false);
		}

		private void HistoryView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int hit = HitTest(e.Location);
				if (hit < 0) return;

				if ((Control.ModifierKeys & Keys.Shift) != 0 && _focusItemIndex >= 0)
				{
					// Select everything between the focus item and this one.
					SelectRange(_focusItemIndex, hit, true);
				}
				else if ((Control.ModifierKeys & Keys.Control) != 0)
				{
					// Toggle selection on this item.
					ToggleSelect(hit, true);
					_focusItemIndex = hit;
				}
				else
				{
					// Select just this one.
					SelectItem(hit, true);
					_focusItemIndex = hit;
				}
			}
		}

		private void HistoryView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (_selectedItems.Count > 0)
				{
					if (ItemActivate != null) ItemActivate(this, new EventArgs());
				}
			}
		}

		public bool NotifyKeyDown(KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Down:
					{
						int maxIndex = LastSelected;
						if (maxIndex == -1) break;
						if (maxIndex + 1 < _history.Count)
						{
							if ((Control.ModifierKeys & Keys.Shift) != 0)
							{
								ToggleSelect(maxIndex + 1, true);
							}
							else
							{
								SelectItem(maxIndex + 1, true);
							}

							EnsureVisible(maxIndex + 1);
						}
						else if (maxIndex + 1 == _history.Count)
						{
							if (SelectPastBottom != null) SelectPastBottom(this, new EventArgs());
						}
						e.Handled = true;
					}
					return true;

				case Keys.Up:
					{
						int minIndex = FirstSelected;
						if (minIndex > 0)
						{
							if ((Control.ModifierKeys & Keys.Shift) != 0)
							{
								ToggleSelect(minIndex - 1, true);
							}
							else
							{
								SelectItem(minIndex - 1, true);
							}

							EnsureVisible(minIndex - 1);
						}
						else if (minIndex == -1)
						{
							// Assume the user wants to select the bottom item (first above the command line).
							int i = HitTest(vScroll.Value + vScroll.LargeChange, true);
							if (i != -1)
							{
								SelectItem(i, true);
								EnsureVisible(i);
							}
						}
						e.Handled = true;
					}
					return true;

				case Keys.Home:
					if ((Control.ModifierKeys & Keys.Control) != 0
						&& (_selectedItems.Count != 1 || _selectedItems[0].Index != 0))
					{
						SelectItem(0, true);
						EnsureVisible(0);
						e.Handled = true;
						return true;
					}
					return false;

				case Keys.End:
					if ((Control.ModifierKeys & Keys.Control) != 0)
					{
						int last = _history.Count - 1;
						if (last >= 0 && (_selectedItems.Count != 1 || _selectedItems[0].Index != last))
						{
							SelectItem(last, true);
							EnsureVisible(last);
						}
						e.Handled = true;
						return true;
					}
					return false;

				case Keys.PageUp:
					if (_history.Count > 0)
					{
						int selectItem = -1;
						if (_selectedItems.Count == 0)
						{
							selectItem = HitTest(vScroll.Value, true);
						}
						else
						{
							int first = FirstSelected;
							int y;
							if (first == -1) y = vScroll.Value;
							else y = _history[first].Top;
							y -= ClientRectangle.Height;

							selectItem = HitTest(y, true);
						}

						SelectItem(selectItem, true);
						EnsureVisible(selectItem);
						
					}
					break;

				case Keys.PageDown:
					if (_history.Count > 0 && _selectedItems.Count > 0)
					{
						int last = LastSelected;
						if (last == -1)
						{
							if ((last = HitTest(vScroll.Value + ClientRectangle.Height, true)) == -1) break;
						}
						int y = _history[last].Top;
						int i = HitTest(y + ClientRectangle.Height, true);
						if (i == -1) break;

						if (_selectedItems.Count == 1 && i == _selectedItems[0].Index)
						{
							// Is the last one, and was previously selected.
							// Assume the user wants to move down to the command line.
							EnsureVisible(i);
							if (SelectPastBottom != null) SelectPastBottom(this, new EventArgs());
						}
						else
						{
							SelectItem(i, true);
							EnsureVisible(i);
						}
					}
					break;
			}

			return false;
		}

		private void HistoryView_KeyDown(object sender, KeyEventArgs e)
		{
			NotifyKeyDown(e);
		}

		public Control FocusControl
		{
			get { return _focusControl; }
			set { _focusControl = value; }
		}

		private void HistoryView_Enter(object sender, EventArgs e)
		{
			if (_focusControl != null) _focusControl.Focus();
		}

		public void EnsureVisible(int index)
		{
			if (index < 0 || index >= _history.Count) throw new ArgumentOutOfRangeException();

			int viewTop = vScroll.Value;
			int viewHeight = ClientRectangle.Height;
			int viewBottom = viewTop + viewHeight;
			HistoryItem item = _history[index];

			if (item.Top < viewTop)
			{
				vScroll.Value = item.Top;
				Invalidate();
			}
			else if (item.Bottom >= viewBottom)
			{
				int newScroll = item.Bottom - viewHeight;
				if (newScroll < 0) newScroll = 0;
				else if (newScroll > vScroll.Maximum) newScroll = vScroll.Maximum;
				vScroll.Value = newScroll;
				Invalidate();
			}
		}

		public void DeleteSelected(bool redraw)
		{
			foreach (HistoryItem h in _selectedItems)
			{
				_history.Remove(h);
			}

			Reindex();
			SelectNone(redraw);
		}

		private void Reindex()
		{
			int index = 0;
			foreach (HistoryItem h in _history)
			{
				h.Index = index++;
			}

			RecalculateScroll(true);
		}

		public static Color BackgroundColor1
		{
			get { return _backgroundColor1; }
		}

		public static Color BackgroundColor2
		{
			get { return _backgroundColor2; }
		}

		public int ViewWidth
		{
			get { return ClientRectangle.Width - vScroll.Width; }
		}
	}
}
