using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Calc
{
	public partial class CalcForm : Form
	{
		[DllImport("User32.dll")]
		public static extern Int32 SetForegroundWindow(IntPtr hWnd);

		private delegate void VoidDelegate();

		private const string k_historyHtmlFile = "History.htm";

		private FormWindowState _windowState = FormWindowState.Normal;
		private Rectangle _windowBounds = new Rectangle(0, 0, 0, 0);
		private string _lastCommandText = "";
		private int _lastCommandSelectionStart = 0;
		private int _lastCommandSelectionLength = 0;
		private bool _selfTextEdit = false;
		private bool _answerInserted = false;
		private VoidDelegate _appActivateFunc = null;

		public CalcForm()
		{
			InitializeComponent();

			txtCommand.MouseWheel += new MouseEventHandler(txtCommand_MouseWheel);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void CalcForm_Load(object sender, EventArgs e)
		{
			Text = Application.ProductName;
			Settings.Load(this);
		}

		private void CalcForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Save(this);
		}

		public void WriteLine(HistoryType type, string str)
		{
			historyView.Add(type, str);
			historyView.SelectNone(false);
			historyView.ScrollToBottom();
		}

		private void btnCalc_Click(object sender, EventArgs e)
		{
			ExecuteCommand();
			txtCommand.Focus();
		}

		private void ExecuteCommand()
		{
			if (txtCommand.Text == "") return;

			string cmd = txtCommand.Text.Trim();
			txtCommand.Text = "";

			if (CheckForSysCommand(cmd)) return;

			if (!Globals.silentMode) WriteLine(HistoryType.Echo, cmd);

			try
			{
				Command command = new Command();
				Value returnValue = command.Execute(cmd, true);
				if (returnValue != null && !Globals.silentMode) WriteLine(HistoryType.Answer, returnValue.ToString());
			}
			catch (Exception ex)
			{
				if (!Globals.silentMode) WriteLine(HistoryType.Error, ex.Message);
			}

			_answerInserted = false;
		}

		private bool CheckForSysCommand(string cmd)
		{
			string cmdLower = cmd.ToLower();

			if (cmdLower == "cls")
			{
				ClearScreen();
				return true;
			}
			else if (cmdLower == "exit")
			{
				Close();
				return true;
			}
			else if (cmdLower.Length > 5 && cmdLower.Substring(0, 5) == "help ")
			{
				string topic = cmdLower.Substring(5, cmdLower.Length - 5).Trim();
				if (Help.IsHelp(topic))
				{
					HelpTopic help = Help.GetHelp(topic);
					WriteLine(HistoryType.HelpTopic, help.TopicText);
					WriteLine(HistoryType.HelpBody, help.Body);
				}
				else
				{
					WriteLine(HistoryType.Error, "The help topic '" + topic + "' does not exist.");
				}
				return true;
			}
			else if (cmdLower == "help")
			{
				if (Help.IsHelp("help"))
				{
					HelpTopic help = Help.GetHelp("help");
					WriteLine(HistoryType.HelpTopic, help.TopicText);
					WriteLine(HistoryType.HelpBody, help.Body);
					return true;
				}
			}

			return false;
		}

		private void ClearScreen()
		{
			historyView.Clear();
		}

		private void clearScreenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClearScreen();
		}

		public void SaveSettings(XmlTextWriter xml)
		{
			xml.WriteStartElement("Window");
			xml.WriteElementString("State", _windowState.ToString());
			if (_windowBounds.Width > 0 && _windowBounds.Height > 0)
			{
				xml.WriteStartElement("Bounds");
				xml.WriteAttributeString("Left", _windowBounds.Left.ToString());
				xml.WriteAttributeString("Top", _windowBounds.Top.ToString());
				xml.WriteAttributeString("Width", _windowBounds.Width.ToString());
				xml.WriteAttributeString("Height", _windowBounds.Height.ToString());
				xml.WriteEndElement();
			}
			xml.WriteEndElement();

			historyView.SaveSettings(xml);
		}

		public void LoadSettings(XmlDocument xmlDoc, bool custom)
		{
			foreach (XmlElement w in xmlDoc.GetElementsByTagName("Window"))
			{
				if (w["State"] != null)
				{
					try
					{
						_windowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), w["State"].InnerText);
						if (_windowState == FormWindowState.Minimized) _windowState = FormWindowState.Normal;
					}
					catch (Exception)
					{
						_windowState = FormWindowState.Normal;
					}
				}

				if (w["Bounds"] != null)
				{
					try
					{
						XmlElement b = w["Bounds"];
						_windowBounds = new Rectangle(
							Convert.ToInt32(b.GetAttribute("Left")),
							Convert.ToInt32(b.GetAttribute("Top")),
							Convert.ToInt32(b.GetAttribute("Width")),
							Convert.ToInt32(b.GetAttribute("Height")));
					}
					catch (Exception)
					{
						_windowBounds = new Rectangle(0, 0, 0, 0);
					}
				}

				WindowState = _windowState;
				if (_windowBounds.Width > 0 && _windowBounds.Height > 0) Bounds = _windowBounds;
			}

			historyView.LoadSettings(xmlDoc, custom);
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SettingsDialog dlg = new SettingsDialog(historyView);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				historyView.RecalcLayout(true);
			}
		}

		private void CalcForm_Resize(object sender, EventArgs e)
		{
			UpdateWindowSettings();
		}

		private void CalcForm_Move(object sender, EventArgs e)
		{
			UpdateWindowSettings();
		}

		private void UpdateWindowSettings()
		{
			switch (WindowState)
			{
				case FormWindowState.Normal:
					_windowState = FormWindowState.Normal;
					if (Bounds.Width > 0 && Bounds.Height > 0) _windowBounds = Bounds;
					break;

				case FormWindowState.Maximized:
					_windowState = FormWindowState.Maximized;
					if (RestoreBounds.Width > 0 && RestoreBounds.Height > 0) _windowBounds = RestoreBounds;
					break;
			}
		}

		private void historyView_ItemActivate(object sender, EventArgs e)
		{
			Debug.WriteLine("Item Activate!");
		}

		private void CalcForm_KeyDown(object sender, KeyEventArgs e)
		{
			historyView.NotifyKeyDown(e);
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			historyView.SelectAll(true);
		}

		void txtCommand_MouseWheel(object sender, MouseEventArgs e)
		{
			historyView.NotifyMouseWheel(e.Delta);
		}

		private void historyView_SelectionChanged(object sender, EventArgs e)
		{
			if (historyView.SelectedCount == 1)
			{
				int i = historyView.SelectedItems[0];
				switch (historyView.ItemType(i))
				{
					case HistoryType.Echo:
					case HistoryType.Answer:
						SelectHistoryText(historyView.ItemText(i));
						break;
				}
			}
		}

		private void SelectHistoryText(string text)
		{
			bool selfTextEdit = _selfTextEdit;
			try
			{
				_selfTextEdit = true;

				string cmdText = txtCommand.Text;
				int selStart = txtCommand.SelectionStart;
				int selLength = txtCommand.SelectionLength;

				// If the cursor is at the end of the string, and the last char typed was an operator,
				// then just append the text.
				if (cmdText.Length > 0 &&
					selStart == cmdText.Length &&
					Command.IsOperatorChar(cmdText[cmdText.Length - 1]))
				{
					txtCommand.Text += text;
					txtCommand.Select(cmdText.Length, text.Length);
					return;
				}

				// If user has selected some text, then replace this text with the history selection.
				if (cmdText.Length > 0 &&
					selLength != 0)
				{
					string str = "";
					if (selStart > 0) str = cmdText.Substring(0, selStart);
					str += text;
					if (selStart + selLength < cmdText.Length) str += cmdText.Substring(selStart + selLength);

					txtCommand.Text = str;
					txtCommand.Select(selStart, text.Length);
					return;
				}

				// Otherwise just replace the whole string with the history selection.
				txtCommand.Text = text;
				txtCommand.SelectAll();
			}
			finally
			{
				UpdateLastCommandText();
				_selfTextEdit = selfTextEdit;
			}
		}

		private void historyView_SelectPastBottom(object sender, EventArgs e)
		{
			historyView.SelectNone(true);
			txtCommand.Focus();
		}

		private void txtCommand_Click(object sender, EventArgs e)
		{
			historyView.SelectNone(true);
		}

		private void CopyHistorySelection()
		{
			string text = "";
			int[] items = historyView.SelectedItems;
			foreach (int item in items)
			{
				if (text != "") text += "\r\n";
				text += historyView.ItemText(item);
			}

			Clipboard.SetData(DataFormats.Text, text);
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (historyView.SelectedCount > 0)
			{
				CopyHistorySelection();
				historyView.DeleteSelected(true);
			}
			else
			{
				txtCommand.Cut();
			}
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (historyView.SelectedCount > 0)
			{
				CopyHistorySelection();
			}
			else
			{
				txtCommand.Copy();
			}
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			txtCommand.Paste();
		}

		private void DeleteHistory()
		{
			if (historyView.SelectedCount > 0)
			{
				int oldIndex = historyView.SelectedItems[0];
				historyView.DeleteSelected(false);
				if (oldIndex < historyView.Count)
				{
					historyView.SelectItem(oldIndex, true);
				}
				else if (historyView.Count > 0)
				{
					historyView.SelectItem(historyView.Count - 1, true);
				}
				else
				{
					// Don't select anything. But the control needs to be redrawn.
					historyView.Invalidate();
				}
			}
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (historyView.SelectedCount > 0)
			{
				DeleteHistory();
			}
		}

		private void txtCommand_TextChanged(object sender, EventArgs e)
		{
			if (_selfTextEdit) return;

			switch (historyView.SelectedCount)
			{
				case 0:
					break;

				case 1:
					{
						string text = historyView.ItemText(historyView.SelectedItems[0]);
						if (text != txtCommand.Text) historyView.SelectNone(true);
					}
					break;

				default:
					historyView.SelectNone(true);
					break;
			}
		}

		private void txtCommand_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (txtCommand.Text == "" && Command.IsOperatorChar(e.KeyChar) &&
				(e.KeyChar != '-' || !_answerInserted))	// Only insert answer before a '-' the first time only.
			{
				string str = Data.AnswerName + e.KeyChar;
				txtCommand.Text = str;
				txtCommand.Select(str.Length, 0);

				e.Handled = true;
				_answerInserted = true;
			}
		}

		private void txtCommand_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				ExecuteCommand();
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Escape)
			{
				txtCommand.Text = "";
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Delete)
			{
				if (historyView.SelectedCount > 0)
				{
					DeleteHistory();
					e.SuppressKeyPress = true;
					e.Handled = true;
				}
			}
			else
			{
				historyView.NotifyKeyDown(e);
			}
		}


		private void txtCommand_KeyUp(object sender, KeyEventArgs e)
		{
			if (txtCommand.Text != _lastCommandText ||
				   txtCommand.SelectionStart != _lastCommandSelectionStart ||
				   txtCommand.SelectionLength != _lastCommandSelectionLength)
			{
				if (historyView.SelectedCount != 0) historyView.SelectNone(true);

				UpdateLastCommandText();
			}
		}

		private void UpdateLastCommandText()
		{
			_lastCommandText = txtCommand.Text;
			_lastCommandSelectionStart = txtCommand.SelectionStart;
			_lastCommandSelectionLength = txtCommand.SelectionLength;
		}

		private void dataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Data.Push();
			try
			{
				DataDialog dlg = new DataDialog();
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					Data.DiscardPushed();
				}
				else
				{
					Data.Pop();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred in the Data dialog: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Data.Pop();
			}
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutDialog dlg = new AboutDialog();
			dlg.ShowDialog(this);
		}

		private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HelpDialog dlg = new HelpDialog(historyView);
			dlg.ShowDialog(this);
		}

		public void AppActivate()
		{
			if (InvokeRequired)
			{
				if (_appActivateFunc == null) _appActivateFunc = new VoidDelegate(AppActivate);
				Invoke(_appActivateFunc);
				return;
			}

			WindowState = _windowState;
			Bounds = _windowBounds;

			SetForegroundWindow(this.Handle);
		}
	}
}
