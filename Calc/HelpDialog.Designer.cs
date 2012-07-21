namespace Calc
{
	partial class HelpDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lstTopic = new System.Windows.Forms.ListBox();
			this.txtBody = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// lstTopic
			// 
			this.lstTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lstTopic.FormattingEnabled = true;
			this.lstTopic.Location = new System.Drawing.Point(12, 12);
			this.lstTopic.Name = "lstTopic";
			this.lstTopic.Size = new System.Drawing.Size(200, 433);
			this.lstTopic.Sorted = true;
			this.lstTopic.TabIndex = 0;
			this.lstTopic.SelectedIndexChanged += new System.EventHandler(this.lstTopic_SelectedIndexChanged);
			// 
			// txtBody
			// 
			this.txtBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtBody.Location = new System.Drawing.Point(218, 12);
			this.txtBody.Name = "txtBody";
			this.txtBody.ReadOnly = true;
			this.txtBody.Size = new System.Drawing.Size(304, 433);
			this.txtBody.TabIndex = 1;
			this.txtBody.Text = "";
			// 
			// HelpDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(534, 456);
			this.Controls.Add(this.txtBody);
			this.Controls.Add(this.lstTopic);
			this.Name = "HelpDialog";
			this.ShowIcon = false;
			this.Text = "Help";
			this.Load += new System.EventHandler(this.HelpDialog_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstTopic;
		private System.Windows.Forms.RichTextBox txtBody;
	}
}