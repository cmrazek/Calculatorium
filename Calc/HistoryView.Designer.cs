namespace Calc
{
	partial class HistoryView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.vScroll = new System.Windows.Forms.VScrollBar();
			this.SuspendLayout();
			// 
			// vScroll
			// 
			this.vScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.vScroll.Location = new System.Drawing.Point(132, 0);
			this.vScroll.Name = "vScroll";
			this.vScroll.Size = new System.Drawing.Size(18, 150);
			this.vScroll.TabIndex = 0;
			this.vScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScroll_Scroll);
			// 
			// HistoryView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.vScroll);
			this.DoubleBuffered = true;
			this.Name = "HistoryView";
			this.Load += new System.EventHandler(this.HistoryView_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.HistoryView_Paint);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.HistoryView_MouseDoubleClick);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HistoryView_MouseClick);
			this.Resize += new System.EventHandler(this.HistoryView_Resize);
			this.Enter += new System.EventHandler(this.HistoryView_Enter);
			this.SystemColorsChanged += new System.EventHandler(this.HistoryView_SystemColorsChanged);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HistoryView_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.VScrollBar vScroll;
	}
}
