using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Calc
{
	public partial class DataDialog : Form
	{
		private string _selectTag = "";

		public DataDialog()
		{
			InitializeComponent();
		}

		private void DataDialog_Load(object sender, EventArgs e)
		{
			chkShowVariables.Checked = Settings.DataDialogShowVariables;
			chkShowMacros.Checked = Settings.DataDialogShowMacros;
			PopulateDataList();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void PopulateDataList()
		{
			lstData.Items.Clear();

			if (chkShowVariables.Checked)
			{
				string[] varNames = Data.UserVariableNames;
				foreach (string name in varNames)
				{
					Variable var = Data.GetVariable(name);
					AddDataItemToList(name, var.Value.ToString(false), "v" + name);
				}
			}

			if (chkShowMacros.Checked)
			{
				string[] macroNames = Data.MacroNames;
				foreach (string name in macroNames)
				{
					Macro macro = Data.GetMacro(name);

					string value = macro.Source;
					int i = value.IndexOf('=');
					if (i >= 0) value = value.Substring(i + 1).TrimStart();

					AddDataItemToList(name + "()", value, "m" + name);
				}
			}
		}

		private void AddDataItemToList(string name, string value, string tag)
		{
			ListViewItem lvi = new ListViewItem();
			foreach (ColumnHeader col in lstData.Columns)
			{
				string text;
				switch (col.Tag.ToString())
				{
					case "name": text = name; break;
					case "value": text = value; break;
					default: text = ""; break;
				}

				if (col.Index == 0) lvi.Text = text;
				else lvi.SubItems.Add(text);
			}

			lvi.Tag = tag;
			if (_selectTag == tag) lvi.Selected = true;
			lstData.Items.Add(lvi);
		}

		private void chkShowVariables_CheckedChanged(object sender, EventArgs e)
		{
			Settings.DataDialogShowVariables = chkShowVariables.Checked;
			PopulateDataList();
		}

		private void chkShowMacros_CheckedChanged(object sender, EventArgs e)
		{
			Settings.DataDialogShowMacros = chkShowMacros.Checked;
			PopulateDataList();
		}

		private void lstData_SelectedIndexChanged(object sender, EventArgs e)
		{
			Variable var;
			Macro macro;
			GetSelectedData(out var, out macro);
			if (var != null)
			{
				txtName.Text = var.Name;
				txtValue.Text = var.Value.ToString(false);
				btnChange.Enabled = false;
				btnDelete.Enabled = true;
				txtValue.ReadOnly = false;
			}
			else if (macro != null)
			{
				string source = macro.Source;
				int eq = source.IndexOf('=');
				if (eq <= 0 || source.Length <= eq + 1) throw new Exception("Macro source is malformed.");
				txtName.Text = source.Substring(0, eq);
				txtValue.Text = source.Substring(eq + 1);
				btnChange.Enabled = false;
				btnDelete.Enabled = true;
				txtValue.ReadOnly = false;
			}
			else
			{
				txtName.Text = "";
				txtValue.Text = "";
				btnChange.Enabled = false;
				btnDelete.Enabled = false;
				txtValue.ReadOnly = true;
			}
		}

		private bool GetSelectedData(out Variable var, out Macro macro)
		{
			if (lstData.SelectedItems.Count != 1)
			{
				_selectTag = "";
				var = null;
				macro = null;
				return false;
			}

			_selectTag = lstData.SelectedItems[0].Tag.ToString();
			if (_selectTag.Length < 2) throw new Exception("Item tag text is too short.");

			string name = _selectTag.Substring(1);
			switch (_selectTag[0])
			{
				case 'v':
					var = Data.GetVariable(name);
					macro = null;
					return true;

				case 'm':
					macro = Data.GetMacro(name);
					var = null;
					return true;

				default:
					throw new Exception("Item tag does not being with 'v' or 'm'.");
			}
		}

		private void btnChange_Click(object sender, EventArgs e)
		{
			bool silentMode = Globals.silentMode;
			Globals.silentMode = true;
			try
			{
				if (txtName.Text == "" || txtValue.Text == "") throw new Exception("Nothing to change.");

				string equation = txtName.Text + "=" + txtValue.Text;
				Command cmd = new Command();
				cmd.Execute(equation, false);
				PopulateDataList();
			}
			catch (Exception ex)
			{
				MessageBox.Show("This equation produced an error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			finally
			{
				Globals.silentMode = silentMode;
			}
		}

		private void txtValue_TextChanged(object sender, EventArgs e)
		{
			btnChange.Enabled = true;
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			Variable var;
			Macro macro;
			if (GetSelectedData(out var, out macro))
			{
				if (var != null)
				{
					Data.DeleteVariable(var.Name);
				}
				else if (macro != null)
				{
					Data.DeleteMacro(macro.Name);
				}

				PopulateDataList();
			}
		}

		private void txtValue_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && btnChange.Enabled)
			{
				btnChange_Click(this, new EventArgs());
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.Filter = "XML Files|*.xml|All Files|*.*";
				if (Settings.DataDialogExportDir != "") dlg.InitialDirectory = Settings.DataDialogExportDir;
				else dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				if (dlg.ShowDialog(this) != DialogResult.OK) return;

				Settings.DataDialogExportDir = Path.GetDirectoryName(dlg.FileName);

				XmlDocument xml = new XmlDocument();
				xml.Load(dlg.FileName);
				foreach (XmlElement xmlData in xml.GetElementsByTagName("Data"))
				{
					Data.Load(xml, true);
				}

				PopulateDataList();
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred when importing the data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			try
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Filter = "XML Files|*.xml|All Files|*.*";
				dlg.DefaultExt = "xml";
				if (Settings.DataDialogExportDir != "") dlg.InitialDirectory = Settings.DataDialogExportDir;
				else dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				if (dlg.ShowDialog(this) != DialogResult.OK) return;

				Settings.DataDialogExportDir = Path.GetDirectoryName(dlg.FileName);

				XmlTextWriter xml = new XmlTextWriter(dlg.FileName, Encoding.UTF8);
				xml.Formatting = Formatting.Indented;
				xml.WriteStartDocument();
				xml.WriteStartElement("Data");
				Data.Save(xml, false);
				xml.WriteEndElement();
				xml.WriteEndDocument();
				xml.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred when exporting the data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
	}
}
