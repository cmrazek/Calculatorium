namespace Calc
{
	partial class SettingsDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
			this.tabFormat = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radDegs = new System.Windows.Forms.RadioButton();
			this.radRads = new System.Windows.Forms.RadioButton();
			this.grpProgrammers = new System.Windows.Forms.GroupBox();
			this.chkEnableXor = new System.Windows.Forms.CheckBox();
			this.grpFractions = new System.Windows.Forms.GroupBox();
			this.chkUseDivideForFractions = new System.Windows.Forms.CheckBox();
			this.chkReduceFeet = new System.Windows.Forms.CheckBox();
			this.chkShowMixedFractions = new System.Windows.Forms.CheckBox();
			this.grpNumbers = new System.Windows.Forms.GroupBox();
			this.txtNumDecimals = new System.Windows.Forms.TextBox();
			this.lblNumDecimals = new System.Windows.Forms.Label();
			this.chkDigitGrouping = new System.Windows.Forms.CheckBox();
			this.grpTime = new System.Windows.Forms.GroupBox();
			this.chkTime12Hour = new System.Windows.Forms.CheckBox();
			this.chkShowMilliseconds = new System.Windows.Forms.CheckBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtLookMarginBottom = new System.Windows.Forms.TextBox();
			this.txtLookMarginRight = new System.Windows.Forms.TextBox();
			this.txtLookMarginTop = new System.Windows.Forms.TextBox();
			this.txtLookMarginLeft = new System.Windows.Forms.TextBox();
			this.lstLookAlign = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnFont = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtLookFont = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstLook = new System.Windows.Forms.ComboBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.clrLookBkgnd1 = new Calc.ColorPicker();
			this.clrLookBkgnd2 = new Calc.ColorPicker();
			this.clrLookColor = new Calc.ColorPicker();
			this.btnResetLook = new System.Windows.Forms.Button();
			this.tabFormat.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.grpProgrammers.SuspendLayout();
			this.grpFractions.SuspendLayout();
			this.grpNumbers.SuspendLayout();
			this.grpTime.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabFormat
			// 
			this.tabFormat.Controls.Add(this.groupBox3);
			this.tabFormat.Controls.Add(this.grpProgrammers);
			this.tabFormat.Controls.Add(this.grpFractions);
			this.tabFormat.Controls.Add(this.grpNumbers);
			this.tabFormat.Controls.Add(this.grpTime);
			this.tabFormat.Location = new System.Drawing.Point(4, 22);
			this.tabFormat.Name = "tabFormat";
			this.tabFormat.Padding = new System.Windows.Forms.Padding(3);
			this.tabFormat.Size = new System.Drawing.Size(376, 328);
			this.tabFormat.TabIndex = 0;
			this.tabFormat.Text = "Formatting";
			this.tabFormat.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radDegs);
			this.groupBox3.Controls.Add(this.radRads);
			this.groupBox3.Location = new System.Drawing.Point(8, 264);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(360, 49);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Angles";
			// 
			// radDegs
			// 
			this.radDegs.AutoSize = true;
			this.radDegs.Location = new System.Drawing.Point(76, 19);
			this.radDegs.Name = "radDegs";
			this.radDegs.Size = new System.Drawing.Size(65, 17);
			this.radDegs.TabIndex = 1;
			this.radDegs.TabStop = true;
			this.radDegs.Text = "Degrees";
			this.radDegs.UseVisualStyleBackColor = true;
			// 
			// radRads
			// 
			this.radRads.AutoSize = true;
			this.radRads.Location = new System.Drawing.Point(6, 19);
			this.radRads.Name = "radRads";
			this.radRads.Size = new System.Drawing.Size(64, 17);
			this.radRads.TabIndex = 0;
			this.radRads.TabStop = true;
			this.radRads.Text = "Radians";
			this.radRads.UseVisualStyleBackColor = true;
			// 
			// grpProgrammers
			// 
			this.grpProgrammers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpProgrammers.Controls.Add(this.chkEnableXor);
			this.grpProgrammers.Location = new System.Drawing.Point(8, 213);
			this.grpProgrammers.Name = "grpProgrammers";
			this.grpProgrammers.Size = new System.Drawing.Size(360, 45);
			this.grpProgrammers.TabIndex = 3;
			this.grpProgrammers.TabStop = false;
			this.grpProgrammers.Text = "Programmers";
			// 
			// chkEnableXor
			// 
			this.chkEnableXor.AutoSize = true;
			this.chkEnableXor.Location = new System.Drawing.Point(6, 19);
			this.chkEnableXor.Name = "chkEnableXor";
			this.chkEnableXor.Size = new System.Drawing.Size(198, 17);
			this.chkEnableXor.TabIndex = 0;
			this.chkEnableXor.Text = "Use The Power \'^\' Operator As XOR";
			this.chkEnableXor.UseVisualStyleBackColor = true;
			// 
			// grpFractions
			// 
			this.grpFractions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpFractions.Controls.Add(this.chkUseDivideForFractions);
			this.grpFractions.Controls.Add(this.chkReduceFeet);
			this.grpFractions.Controls.Add(this.chkShowMixedFractions);
			this.grpFractions.Location = new System.Drawing.Point(8, 137);
			this.grpFractions.Name = "grpFractions";
			this.grpFractions.Size = new System.Drawing.Size(360, 70);
			this.grpFractions.TabIndex = 2;
			this.grpFractions.TabStop = false;
			this.grpFractions.Text = "Fractions";
			// 
			// chkUseDivideForFractions
			// 
			this.chkUseDivideForFractions.AutoSize = true;
			this.chkUseDivideForFractions.Location = new System.Drawing.Point(6, 42);
			this.chkUseDivideForFractions.Name = "chkUseDivideForFractions";
			this.chkUseDivideForFractions.Size = new System.Drawing.Size(198, 17);
			this.chkUseDivideForFractions.TabIndex = 2;
			this.chkUseDivideForFractions.Text = "Use Divide \'/\' Operator For Fractions";
			this.chkUseDivideForFractions.UseVisualStyleBackColor = true;
			// 
			// chkReduceFeet
			// 
			this.chkReduceFeet.AutoSize = true;
			this.chkReduceFeet.Location = new System.Drawing.Point(211, 19);
			this.chkReduceFeet.Name = "chkReduceFeet";
			this.chkReduceFeet.Size = new System.Drawing.Size(143, 17);
			this.chkReduceFeet.TabIndex = 1;
			this.chkReduceFeet.Text = "Convert Inches Into Feet";
			this.chkReduceFeet.UseVisualStyleBackColor = true;
			// 
			// chkShowMixedFractions
			// 
			this.chkShowMixedFractions.AutoSize = true;
			this.chkShowMixedFractions.Location = new System.Drawing.Point(6, 19);
			this.chkShowMixedFractions.Name = "chkShowMixedFractions";
			this.chkShowMixedFractions.Size = new System.Drawing.Size(130, 17);
			this.chkShowMixedFractions.TabIndex = 0;
			this.chkShowMixedFractions.Text = "Show Mixed Fractions";
			this.chkShowMixedFractions.UseVisualStyleBackColor = true;
			// 
			// grpNumbers
			// 
			this.grpNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpNumbers.Controls.Add(this.txtNumDecimals);
			this.grpNumbers.Controls.Add(this.lblNumDecimals);
			this.grpNumbers.Controls.Add(this.chkDigitGrouping);
			this.grpNumbers.Location = new System.Drawing.Point(8, 6);
			this.grpNumbers.Name = "grpNumbers";
			this.grpNumbers.Size = new System.Drawing.Size(360, 73);
			this.grpNumbers.TabIndex = 0;
			this.grpNumbers.TabStop = false;
			this.grpNumbers.Text = "Numbers";
			// 
			// txtNumDecimals
			// 
			this.txtNumDecimals.Location = new System.Drawing.Point(90, 19);
			this.txtNumDecimals.Name = "txtNumDecimals";
			this.txtNumDecimals.Size = new System.Drawing.Size(60, 20);
			this.txtNumDecimals.TabIndex = 1;
			// 
			// lblNumDecimals
			// 
			this.lblNumDecimals.AutoSize = true;
			this.lblNumDecimals.Location = new System.Drawing.Point(6, 22);
			this.lblNumDecimals.Name = "lblNumDecimals";
			this.lblNumDecimals.Size = new System.Drawing.Size(78, 13);
			this.lblNumDecimals.TabIndex = 0;
			this.lblNumDecimals.Text = "Num Decimals:";
			// 
			// chkDigitGrouping
			// 
			this.chkDigitGrouping.AutoSize = true;
			this.chkDigitGrouping.Location = new System.Drawing.Point(6, 45);
			this.chkDigitGrouping.Name = "chkDigitGrouping";
			this.chkDigitGrouping.Size = new System.Drawing.Size(93, 17);
			this.chkDigitGrouping.TabIndex = 2;
			this.chkDigitGrouping.Text = "Digit Grouping";
			this.chkDigitGrouping.UseVisualStyleBackColor = true;
			// 
			// grpTime
			// 
			this.grpTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpTime.Controls.Add(this.chkTime12Hour);
			this.grpTime.Controls.Add(this.chkShowMilliseconds);
			this.grpTime.Location = new System.Drawing.Point(8, 85);
			this.grpTime.Name = "grpTime";
			this.grpTime.Size = new System.Drawing.Size(360, 46);
			this.grpTime.TabIndex = 1;
			this.grpTime.TabStop = false;
			this.grpTime.Text = "Date / Time";
			// 
			// chkTime12Hour
			// 
			this.chkTime12Hour.AutoSize = true;
			this.chkTime12Hour.Location = new System.Drawing.Point(6, 19);
			this.chkTime12Hour.Name = "chkTime12Hour";
			this.chkTime12Hour.Size = new System.Drawing.Size(147, 17);
			this.chkTime12Hour.TabIndex = 0;
			this.chkTime12Hour.Text = "Use 12 Hour Time Format";
			this.chkTime12Hour.UseVisualStyleBackColor = true;
			// 
			// chkShowMilliseconds
			// 
			this.chkShowMilliseconds.AutoSize = true;
			this.chkShowMilliseconds.Location = new System.Drawing.Point(241, 19);
			this.chkShowMilliseconds.Name = "chkShowMilliseconds";
			this.chkShowMilliseconds.Size = new System.Drawing.Size(113, 17);
			this.chkShowMilliseconds.TabIndex = 1;
			this.chkShowMilliseconds.Text = "Show Milliseconds";
			this.chkShowMilliseconds.UseVisualStyleBackColor = true;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabFormat);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(384, 354);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnResetLook);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.lstLook);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(376, 328);
			this.tabPage1.TabIndex = 1;
			this.tabPage1.Text = "Appearance";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.groupBox4);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.lstLookAlign);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.clrLookColor);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.btnFont);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtLookFont);
			this.groupBox1.Location = new System.Drawing.Point(11, 33);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(357, 289);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.clrLookBkgnd1);
			this.groupBox4.Controls.Add(this.clrLookBkgnd2);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Location = new System.Drawing.Point(147, 97);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(110, 84);
			this.groupBox4.TabIndex = 8;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Background Color";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 54);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(43, 13);
			this.label10.TabIndex = 1;
			this.label10.Text = "Bottom:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 25);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(29, 13);
			this.label9.TabIndex = 0;
			this.label9.Text = "Top:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.txtLookMarginBottom);
			this.groupBox2.Controls.Add(this.txtLookMarginRight);
			this.groupBox2.Controls.Add(this.txtLookMarginTop);
			this.groupBox2.Controls.Add(this.txtLookMarginLeft);
			this.groupBox2.Location = new System.Drawing.Point(9, 97);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(132, 128);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Margins";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 100);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(43, 13);
			this.label8.TabIndex = 6;
			this.label8.Text = "Bottom:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 74);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(35, 13);
			this.label7.TabIndex = 4;
			this.label7.Text = "Right:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 48);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(29, 13);
			this.label6.TabIndex = 2;
			this.label6.Text = "Top:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 22);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(28, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Left:";
			// 
			// txtLookMarginBottom
			// 
			this.txtLookMarginBottom.Location = new System.Drawing.Point(59, 97);
			this.txtLookMarginBottom.Name = "txtLookMarginBottom";
			this.txtLookMarginBottom.Size = new System.Drawing.Size(60, 20);
			this.txtLookMarginBottom.TabIndex = 3;
			// 
			// txtLookMarginRight
			// 
			this.txtLookMarginRight.Location = new System.Drawing.Point(59, 71);
			this.txtLookMarginRight.Name = "txtLookMarginRight";
			this.txtLookMarginRight.Size = new System.Drawing.Size(60, 20);
			this.txtLookMarginRight.TabIndex = 2;
			// 
			// txtLookMarginTop
			// 
			this.txtLookMarginTop.Location = new System.Drawing.Point(59, 45);
			this.txtLookMarginTop.Name = "txtLookMarginTop";
			this.txtLookMarginTop.Size = new System.Drawing.Size(60, 20);
			this.txtLookMarginTop.TabIndex = 1;
			// 
			// txtLookMarginLeft
			// 
			this.txtLookMarginLeft.Location = new System.Drawing.Point(59, 19);
			this.txtLookMarginLeft.Name = "txtLookMarginLeft";
			this.txtLookMarginLeft.Size = new System.Drawing.Size(60, 20);
			this.txtLookMarginLeft.TabIndex = 0;
			// 
			// lstLookAlign
			// 
			this.lstLookAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.lstLookAlign.FormattingEnabled = true;
			this.lstLookAlign.Items.AddRange(new object[] {
            "Left",
            "Center",
            "Right"});
			this.lstLookAlign.Location = new System.Drawing.Point(68, 70);
			this.lstLookAlign.Name = "lstLookAlign";
			this.lstLookAlign.Size = new System.Drawing.Size(121, 21);
			this.lstLookAlign.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 73);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Alignment:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(34, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Color:";
			// 
			// btnFont
			// 
			this.btnFont.Location = new System.Drawing.Point(276, 17);
			this.btnFont.Name = "btnFont";
			this.btnFont.Size = new System.Drawing.Size(75, 23);
			this.btnFont.TabIndex = 1;
			this.btnFont.Text = "Choose...";
			this.btnFont.UseVisualStyleBackColor = true;
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Font:";
			// 
			// txtLookFont
			// 
			this.txtLookFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtLookFont.Location = new System.Drawing.Point(68, 19);
			this.txtLookFont.Name = "txtLookFont";
			this.txtLookFont.ReadOnly = true;
			this.txtLookFont.Size = new System.Drawing.Size(202, 20);
			this.txtLookFont.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Text Type:";
			// 
			// lstLook
			// 
			this.lstLook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.lstLook.FormattingEnabled = true;
			this.lstLook.Items.AddRange(new object[] {
            "Echo",
            "Answer",
            "Info",
            "Error",
            "HelpTopic",
            "HelpBody"});
			this.lstLook.Location = new System.Drawing.Point(72, 6);
			this.lstLook.Name = "lstLook";
			this.lstLook.Size = new System.Drawing.Size(121, 21);
			this.lstLook.TabIndex = 0;
			this.lstLook.SelectedIndexChanged += new System.EventHandler(this.lstLook_SelectedIndexChanged);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(224, 360);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(305, 360);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// clrLookBkgnd1
			// 
			this.clrLookBkgnd1.Color = System.Drawing.SystemColors.Window;
			this.clrLookBkgnd1.Location = new System.Drawing.Point(55, 19);
			this.clrLookBkgnd1.Name = "clrLookBkgnd1";
			this.clrLookBkgnd1.Size = new System.Drawing.Size(40, 23);
			this.clrLookBkgnd1.TabIndex = 0;
			// 
			// clrLookBkgnd2
			// 
			this.clrLookBkgnd2.Color = System.Drawing.SystemColors.Control;
			this.clrLookBkgnd2.Location = new System.Drawing.Point(55, 48);
			this.clrLookBkgnd2.Name = "clrLookBkgnd2";
			this.clrLookBkgnd2.Size = new System.Drawing.Size(40, 23);
			this.clrLookBkgnd2.TabIndex = 1;
			// 
			// clrLookColor
			// 
			this.clrLookColor.Color = System.Drawing.SystemColors.WindowText;
			this.clrLookColor.Location = new System.Drawing.Point(68, 45);
			this.clrLookColor.Name = "clrLookColor";
			this.clrLookColor.Size = new System.Drawing.Size(36, 19);
			this.clrLookColor.TabIndex = 2;
			// 
			// btnResetLook
			// 
			this.btnResetLook.Location = new System.Drawing.Point(287, 4);
			this.btnResetLook.Name = "btnResetLook";
			this.btnResetLook.Size = new System.Drawing.Size(75, 23);
			this.btnResetLook.TabIndex = 9;
			this.btnResetLook.Text = "&Reset";
			this.btnResetLook.UseVisualStyleBackColor = true;
			this.btnResetLook.Click += new System.EventHandler(this.btnResetLook_Click);
			// 
			// SettingsDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(384, 395);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tabControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(400, 433);
			this.MinimumSize = new System.Drawing.Size(400, 433);
			this.Name = "SettingsDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Settings";
			this.Load += new System.EventHandler(this.SettingsDialog_Load);
			this.tabFormat.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.grpProgrammers.ResumeLayout(false);
			this.grpProgrammers.PerformLayout();
			this.grpFractions.ResumeLayout(false);
			this.grpFractions.PerformLayout();
			this.grpNumbers.ResumeLayout(false);
			this.grpNumbers.PerformLayout();
			this.grpTime.ResumeLayout(false);
			this.grpTime.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabPage tabFormat;
		private System.Windows.Forms.CheckBox chkTime12Hour;
		private System.Windows.Forms.TextBox txtNumDecimals;
		private System.Windows.Forms.Label lblNumDecimals;
		private System.Windows.Forms.CheckBox chkDigitGrouping;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.GroupBox grpNumbers;
		private System.Windows.Forms.GroupBox grpTime;
		private System.Windows.Forms.CheckBox chkShowMilliseconds;
		private System.Windows.Forms.GroupBox grpProgrammers;
		private System.Windows.Forms.GroupBox grpFractions;
		private System.Windows.Forms.CheckBox chkUseDivideForFractions;
		private System.Windows.Forms.CheckBox chkReduceFeet;
		private System.Windows.Forms.CheckBox chkShowMixedFractions;
		private System.Windows.Forms.CheckBox chkEnableXor;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox lstLook;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnFont;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtLookFont;
		private System.Windows.Forms.Label label3;
		private ColorPicker clrLookColor;
		private System.Windows.Forms.ComboBox lstLookAlign;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtLookMarginBottom;
		private System.Windows.Forms.TextBox txtLookMarginRight;
		private System.Windows.Forms.TextBox txtLookMarginTop;
		private System.Windows.Forms.TextBox txtLookMarginLeft;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radDegs;
		private System.Windows.Forms.RadioButton radRads;
		private System.Windows.Forms.GroupBox groupBox4;
		private ColorPicker clrLookBkgnd1;
		private ColorPicker clrLookBkgnd2;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnResetLook;

	}
}