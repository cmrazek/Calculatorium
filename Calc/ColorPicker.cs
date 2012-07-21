using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Calc
{
	public partial class ColorPicker : UserControl
	{
		private Color _color = SystemColors.Window;
		private Brush _colorBrush = null;
		private VisualStyleRenderer _vsr = null;

		public ColorPicker()
		{
			InitializeComponent();
		}

		private void ColorPicker_Load(object sender, EventArgs e)
		{
			try
			{
				_vsr = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);
			}
			catch(Exception)
			{
				_vsr = null;
			}

			_colorBrush = new SolidBrush(_color);
		}

		private void ColorPicker_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.FillRectangle(_colorBrush, ClientRectangle);
			if (_vsr != null)
			{
				_vsr.DrawEdge(g, ClientRectangle, Edges.Left | Edges.Top | Edges.Right | Edges.Bottom, EdgeStyle.Sunken, EdgeEffects.Soft);
			}
			else
			{
				Point bottomLeft = new Point(ClientRectangle.Left, ClientRectangle.Bottom - 1);
				Point topLeft = new Point(ClientRectangle.Left, ClientRectangle.Top);
				Point topRight = new Point(ClientRectangle.Right - 1, ClientRectangle.Top);
				Point bottomRight = new Point(ClientRectangle.Right - 1, ClientRectangle.Bottom - 1);

				Point[] darkLine = new Point[] { bottomLeft, topLeft, topRight };
				Point[] lightLine = new Point[] { topRight, bottomRight, bottomLeft };

				g.DrawLines(SystemPens.ControlDark, darkLine);
				g.DrawLines(SystemPens.ControlLight, lightLine);
			}
		}

		private void ColorPicker_Click(object sender, EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _color;
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				Color = dlg.Color;
			}
		}

		public Color Color
		{
			get { return _color; }
			set
			{
				_color = value;
				_colorBrush = new SolidBrush(_color);
				Invalidate();
			}
		}

	}
}
