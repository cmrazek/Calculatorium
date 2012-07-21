using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Calc
{
	public partial class SettingsDialog : Form
	{
		private HistoryView _historyView = null;
		private SortedList<HistoryType, HistoryLook> _looks = new SortedList<HistoryType, HistoryLook>();
		private HistoryType _currentLook;
		private bool _lookIsSetup = false;
		private Font _lookFont = null;

		private const int k_lookAlignLeft = 0;
		private const int k_lookAlignCenter = 1;
		private const int k_lookAlignRight = 2;

		public SettingsDialog(HistoryView historyView)
		{
			if (historyView == null) throw new ArgumentNullException();
			_historyView = historyView;

			InitializeComponent();

			_looks[HistoryType.Answer] = historyView.GetHistoryLook(HistoryType.Answer).Clone();
			_looks[HistoryType.Echo] = historyView.GetHistoryLook(HistoryType.Echo).Clone();
			_looks[HistoryType.Error] = historyView.GetHistoryLook(HistoryType.Error).Clone();
			_looks[HistoryType.Info] = historyView.GetHistoryLook(HistoryType.Info).Clone();
			_looks[HistoryType.HelpTopic] = historyView.GetHistoryLook(HistoryType.HelpTopic).Clone();
			_looks[HistoryType.HelpBody] = historyView.GetHistoryLook(HistoryType.HelpBody).Clone();

			_looks[HistoryType.Answer].Custom = false;
			_looks[HistoryType.Echo].Custom = false;
			_looks[HistoryType.Error].Custom = false;
			_looks[HistoryType.Info].Custom = false;
			_looks[HistoryType.HelpTopic].Custom = false;
			_looks[HistoryType.HelpBody].Custom = false;
		}

		private void SettingsDialog_Load(object sender, EventArgs e)
		{
			txtNumDecimals.Text = Settings.NumDecimals.ToString();
			chkDigitGrouping.Checked = Settings.DigitGrouping;
			chkTime12Hour.Checked = Settings.Time12Hour;
			chkShowMilliseconds.Checked = Settings.ShowMilliseconds;
			chkShowMixedFractions.Checked = Settings.ShowMixedFractions;
			chkReduceFeet.Checked = Settings.ReduceFeet;
			chkUseDivideForFractions.Checked = Settings.UseDivForFractions;
			chkEnableXor.Checked = Settings.EnableXor;

			if (Settings.UseDegrees) radDegs.Checked = true;
			else radRads.Checked = true;

			lstLook.SelectedIndex = 0;
			SetupLook(false);
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (ApplySettings())
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private bool ApplySettings()
		{
			int numDecimals;

			if (!ValidateInt(txtNumDecimals, 0, 20, "Num Decimals must be an integer between 0 and 20.", out numDecimals)) return false;

			Settings.NumDecimals = Convert.ToInt32(txtNumDecimals.Text);
			Settings.DigitGrouping = chkDigitGrouping.Checked;
			Settings.Time12Hour = chkTime12Hour.Checked;
			Settings.ShowMilliseconds = chkShowMilliseconds.Checked;
			Settings.ShowMixedFractions = chkShowMixedFractions.Checked;
			Settings.ReduceFeet = chkReduceFeet.Checked;
			Settings.UseDivForFractions = chkUseDivideForFractions.Checked;
			Settings.EnableXor = chkEnableXor.Checked;
			Settings.UseDegrees = radDegs.Checked;

			SaveCurrentLook();
			ApplyLook(HistoryType.Answer);
			ApplyLook(HistoryType.Echo);
			ApplyLook(HistoryType.Error);
			ApplyLook(HistoryType.Info);
			ApplyLook(HistoryType.HelpTopic);
			ApplyLook(HistoryType.HelpBody);

			return true;
		}

		private void ApplyLook(HistoryType type)
		{
			HistoryLook look = _looks[type];
			if (look.Custom) _historyView.GetHistoryLook(type).Copy(look);
		}

		private bool ValidateInt(TextBox textBox, int minValue, int maxValue, string err, out int valueOut)
		{
			return ValidateInt(textBox.Text, textBox, minValue, maxValue, err, out valueOut);
		}

		private bool ValidateInt(string text, Control ctrl, int minValue, int maxValue, string err, out int valueOut)
		{
			int val;
			try
			{
				val = Convert.ToInt32(text);
			}
			catch (Exception)
			{
				ShowError(err, ctrl);
				valueOut = 0;
				return false;
			}

			valueOut = val;

			if (val < minValue || val > maxValue)
			{
				ShowError(err, ctrl);
				return false;
			}

			return true;
		}

		private void ShowError(string msg, Control ctrl)
		{
			ctrl.Focus();
			MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		private void SaveCurrentLook()
		{
			if (!_lookIsSetup) return;

			bool changed = false;

			HistoryLook oldLook = _looks[_currentLook];
			if (oldLook.Font != _lookFont)
			{
				oldLook.Font = _lookFont;
				changed = true;
			}

			if (oldLook.TextColor != clrLookColor.Color)
			{
				oldLook.TextColor = clrLookColor.Color;
				changed = true;
			}

			TextFormatFlags textAlign = TextFormatFlags.Left;
			switch (lstLookAlign.SelectedIndex)
			{
				case k_lookAlignLeft: textAlign = TextFormatFlags.Left; break;
				case k_lookAlignCenter: textAlign = TextFormatFlags.HorizontalCenter; break;
				case k_lookAlignRight: textAlign = TextFormatFlags.Right; break;
			}
			if (oldLook.TextAlign != textAlign)
			{
				oldLook.TextAlign = textAlign;
				changed = true;
			}

			int margin;
			margin = Util.StringToInt(txtLookMarginLeft.Text, oldLook.MarginLeft);
			if (oldLook.MarginLeft != margin)
			{
				oldLook.MarginLeft = margin;
				changed = true;
			}

			margin = Util.StringToInt(txtLookMarginTop.Text, oldLook.MarginTop);
			if (oldLook.MarginTop != margin)
			{
				oldLook.MarginTop = margin;
				changed = true;
			}

			margin = Util.StringToInt(txtLookMarginRight.Text, oldLook.MarginRight);
			if (oldLook.MarginRight != margin)
			{
				oldLook.MarginRight = margin;
				changed = true;
			}

			margin = Util.StringToInt(txtLookMarginBottom.Text, oldLook.MarginBottom);
			if (oldLook.MarginBottom != margin)
			{
				oldLook.MarginBottom = margin;
				changed = true;
			}

			if (oldLook.BackgroundColor1 != clrLookBkgnd1.Color)
			{
				oldLook.BackgroundColor1 = clrLookBkgnd1.Color;
				changed = true;
			}

			if (oldLook.BackgroundColor2 != clrLookBkgnd2.Color)
			{
				oldLook.BackgroundColor2 = clrLookBkgnd2.Color;
				changed = true;
			}

			if (changed) oldLook.Custom = true;
		}

		private void SetupLook(bool saveCurrent)
		{
			try
			{
				if (saveCurrent) SaveCurrentLook();

				HistoryType type = (HistoryType)Enum.Parse(typeof(HistoryType), lstLook.Text);
				HistoryLook look = _looks[type];
				_currentLook = type;

				SetupLookFont(look.Font);
				clrLookColor.Color = look.TextColor;

				switch (look.TextAlign)
				{
					case TextFormatFlags.Right: lstLookAlign.SelectedIndex = k_lookAlignRight; break;
					case TextFormatFlags.HorizontalCenter: lstLookAlign.SelectedIndex = k_lookAlignCenter; break;
					default: lstLookAlign.SelectedIndex = k_lookAlignLeft; break;
				}

				txtLookMarginLeft.Text = look.MarginLeft.ToString();
				txtLookMarginTop.Text = look.MarginTop.ToString();
				txtLookMarginRight.Text = look.MarginRight.ToString();
				txtLookMarginBottom.Text = look.MarginBottom.ToString();

				clrLookBkgnd1.Color = look.BackgroundColor1;
				clrLookBkgnd2.Color = look.BackgroundColor2;

				_lookIsSetup = true;
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error when setting up the appearance record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				_lookIsSetup = false;
			}
		}

		private void lstLook_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetupLook(true);
		}

		private void btnFont_Click(object sender, EventArgs e)
		{
			FontDialog dlg = new FontDialog();
			dlg.Font = _lookFont;
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				SetupLookFont(dlg.Font);
			}
		}

		private void SetupLookFont(Font font)
		{
			_lookFont = font;

			string desc = font.Name + " " + font.SizeInPoints.ToString() + " pt";
			if (font.Style != FontStyle.Regular) desc += " " + font.Style.ToString();
			txtLookFont.Text = desc;
		}

		private void btnResetLook_Click(object sender, EventArgs e)
		{
			HistoryType type = (HistoryType)Enum.Parse(typeof(HistoryType), lstLook.Text);

			HistoryLook look = _historyView.GetHistoryDefaultLook(type).Clone();
			look.Custom = true;
			_looks[type] = look;
			SetupLook(false);
		}
	}
}
