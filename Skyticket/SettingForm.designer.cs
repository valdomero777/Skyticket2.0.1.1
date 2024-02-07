namespace Skyticket
{
    partial class SettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.label4 = new System.Windows.Forms.Label();
            this.OutputFolderBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PoweredCheck = new System.Windows.Forms.CheckBox();
            this.PrintCustomerCheck = new System.Windows.Forms.CheckBox();
            this.PrintPaperCheck = new System.Windows.Forms.CheckBox();
            this.DocPrinterCheck = new System.Windows.Forms.CheckBox();
            this.PrintersListBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.WinRadioButton = new System.Windows.Forms.RadioButton();
            this.NetworkRadioButton = new System.Windows.Forms.RadioButton();
            this.PrinterBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chckNoPrint = new System.Windows.Forms.CheckBox();
            this.cmbTypePos = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.BarcodesCheck = new System.Windows.Forms.CheckBox();
            this.PortsListBox = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.PhoneDigitsBox = new System.Windows.Forms.NumericUpDown();
            this.PhoneSuffixBox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.TestClientButton = new System.Windows.Forms.Button();
            this.LanguagesBox = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.ClientIDBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TerminalIDBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.MinimizeTrayBox = new System.Windows.Forms.CheckBox();
            this.CustFeedbackCheck = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneDigitsBox)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // OutputFolderBox
            // 
            resources.ApplyResources(this.OutputFolderBox, "OutputFolderBox");
            this.OutputFolderBox.Name = "OutputFolderBox";
            // 
            // BrowseButton
            // 
            resources.ApplyResources(this.BrowseButton, "BrowseButton");
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PoweredCheck);
            this.groupBox1.Controls.Add(this.PrintCustomerCheck);
            this.groupBox1.Controls.Add(this.PrintPaperCheck);
            this.groupBox1.Controls.Add(this.DocPrinterCheck);
            this.groupBox1.Controls.Add(this.PrintersListBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.WinRadioButton);
            this.groupBox1.Controls.Add(this.NetworkRadioButton);
            this.groupBox1.Controls.Add(this.PrinterBox);
            this.groupBox1.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // PoweredCheck
            // 
            resources.ApplyResources(this.PoweredCheck, "PoweredCheck");
            this.PoweredCheck.Name = "PoweredCheck";
            this.PoweredCheck.UseVisualStyleBackColor = true;
            // 
            // PrintCustomerCheck
            // 
            resources.ApplyResources(this.PrintCustomerCheck, "PrintCustomerCheck");
            this.PrintCustomerCheck.Name = "PrintCustomerCheck";
            this.PrintCustomerCheck.UseVisualStyleBackColor = true;
            // 
            // PrintPaperCheck
            // 
            resources.ApplyResources(this.PrintPaperCheck, "PrintPaperCheck");
            this.PrintPaperCheck.Name = "PrintPaperCheck";
            this.PrintPaperCheck.UseVisualStyleBackColor = true;
            // 
            // DocPrinterCheck
            // 
            resources.ApplyResources(this.DocPrinterCheck, "DocPrinterCheck");
            this.DocPrinterCheck.Name = "DocPrinterCheck";
            this.DocPrinterCheck.UseVisualStyleBackColor = true;
            // 
            // PrintersListBox
            // 
            this.PrintersListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PrintersListBox.FormattingEnabled = true;
            resources.ApplyResources(this.PrintersListBox, "PrintersListBox");
            this.PrintersListBox.Name = "PrintersListBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // WinRadioButton
            // 
            resources.ApplyResources(this.WinRadioButton, "WinRadioButton");
            this.WinRadioButton.Name = "WinRadioButton";
            this.WinRadioButton.TabStop = true;
            this.WinRadioButton.UseVisualStyleBackColor = true;
            // 
            // NetworkRadioButton
            // 
            resources.ApplyResources(this.NetworkRadioButton, "NetworkRadioButton");
            this.NetworkRadioButton.Name = "NetworkRadioButton";
            this.NetworkRadioButton.TabStop = true;
            this.NetworkRadioButton.UseVisualStyleBackColor = true;
            // 
            // PrinterBox
            // 
            resources.ApplyResources(this.PrinterBox, "PrinterBox");
            this.PrinterBox.Name = "PrinterBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chckNoPrint);
            this.groupBox3.Controls.Add(this.cmbTypePos);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Controls.Add(this.BarcodesCheck);
            this.groupBox3.Controls.Add(this.PortsListBox);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.PhoneDigitsBox);
            this.groupBox3.Controls.Add(this.PhoneSuffixBox);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.TestClientButton);
            this.groupBox3.Controls.Add(this.LanguagesBox);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.ClientIDBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.TerminalIDBox);
            this.groupBox3.Controls.Add(this.label11);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // chckNoPrint
            // 
            resources.ApplyResources(this.chckNoPrint, "chckNoPrint");
            this.chckNoPrint.Name = "chckNoPrint";
            this.chckNoPrint.UseVisualStyleBackColor = true;
            // 
            // cmbTypePos
            // 
            this.cmbTypePos.FormattingEnabled = true;
            this.cmbTypePos.Items.AddRange(new object[] {
            resources.GetString("cmbTypePos.Items"),
            resources.GetString("cmbTypePos.Items1"),
            resources.GetString("cmbTypePos.Items2"),
            resources.GetString("cmbTypePos.Items3")});
            resources.ApplyResources(this.cmbTypePos, "cmbTypePos");
            this.cmbTypePos.Name = "cmbTypePos";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // BarcodesCheck
            // 
            resources.ApplyResources(this.BarcodesCheck, "BarcodesCheck");
            this.BarcodesCheck.Name = "BarcodesCheck";
            this.BarcodesCheck.UseVisualStyleBackColor = true;
            // 
            // PortsListBox
            // 
            this.PortsListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PortsListBox.FormattingEnabled = true;
            resources.ApplyResources(this.PortsListBox, "PortsListBox");
            this.PortsListBox.Name = "PortsListBox";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // PhoneDigitsBox
            // 
            resources.ApplyResources(this.PhoneDigitsBox, "PhoneDigitsBox");
            this.PhoneDigitsBox.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.PhoneDigitsBox.Name = "PhoneDigitsBox";
            this.PhoneDigitsBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // PhoneSuffixBox
            // 
            resources.ApplyResources(this.PhoneSuffixBox, "PhoneSuffixBox");
            this.PhoneSuffixBox.Name = "PhoneSuffixBox";
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // TestClientButton
            // 
            resources.ApplyResources(this.TestClientButton, "TestClientButton");
            this.TestClientButton.Name = "TestClientButton";
            this.TestClientButton.UseVisualStyleBackColor = true;
            this.TestClientButton.Click += new System.EventHandler(this.TestClientButton_Click);
            // 
            // LanguagesBox
            // 
            this.LanguagesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguagesBox.FormattingEnabled = true;
            this.LanguagesBox.Items.AddRange(new object[] {
            resources.GetString("LanguagesBox.Items"),
            resources.GetString("LanguagesBox.Items1")});
            resources.ApplyResources(this.LanguagesBox, "LanguagesBox");
            this.LanguagesBox.Name = "LanguagesBox";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // ClientIDBox
            // 
            resources.ApplyResources(this.ClientIDBox, "ClientIDBox");
            this.ClientIDBox.Name = "ClientIDBox";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // TerminalIDBox
            // 
            resources.ApplyResources(this.TerminalIDBox, "TerminalIDBox");
            this.TerminalIDBox.Name = "TerminalIDBox";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // MinimizeTrayBox
            // 
            resources.ApplyResources(this.MinimizeTrayBox, "MinimizeTrayBox");
            this.MinimizeTrayBox.Name = "MinimizeTrayBox";
            this.MinimizeTrayBox.UseVisualStyleBackColor = true;
            // 
            // CustFeedbackCheck
            // 
            resources.ApplyResources(this.CustFeedbackCheck, "CustFeedbackCheck");
            this.CustFeedbackCheck.Name = "CustFeedbackCheck";
            this.CustFeedbackCheck.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.button1);
            this.groupBox7.Controls.Add(this.CustFeedbackCheck);
            this.groupBox7.Controls.Add(this.MinimizeTrayBox);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.OutputFolderBox);
            this.tabPage1.Controls.Add(this.BrowseButton);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SettingForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneDigitsBox)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox OutputFolderBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox PrintersListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton WinRadioButton;
        private System.Windows.Forms.RadioButton NetworkRadioButton;
        private System.Windows.Forms.TextBox PrinterBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox ClientIDBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TerminalIDBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox LanguagesBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox DocPrinterCheck;
        private System.Windows.Forms.Button TestClientButton;
        private System.Windows.Forms.TextBox PhoneSuffixBox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.NumericUpDown PhoneDigitsBox;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox PortsListBox;
        private System.Windows.Forms.CheckBox BarcodesCheck;
        private System.Windows.Forms.CheckBox PrintPaperCheck;
        private System.Windows.Forms.CheckBox PrintCustomerCheck;
        private System.Windows.Forms.CheckBox PoweredCheck;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox cmbTypePos;
        private System.Windows.Forms.CheckBox MinimizeTrayBox;
        private System.Windows.Forms.CheckBox CustFeedbackCheck;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chckNoPrint;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}