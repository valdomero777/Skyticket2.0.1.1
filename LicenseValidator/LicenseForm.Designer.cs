
namespace Skyticket
{
    partial class LicenseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));
            this.LicenseKeyBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ValidateButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SucursalList = new System.Windows.Forms.ComboBox();
            this.SucursalLbl = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TerminalList = new System.Windows.Forms.ComboBox();
            this.SupportLink = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.ProceedButton = new System.Windows.Forms.Button();
            this.SkipButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LicenseKeyBox
            // 
            this.LicenseKeyBox.Location = new System.Drawing.Point(13, 64);
            this.LicenseKeyBox.Margin = new System.Windows.Forms.Padding(4);
            this.LicenseKeyBox.Name = "LicenseKeyBox";
            this.LicenseKeyBox.Size = new System.Drawing.Size(293, 22);
            this.LicenseKeyBox.TabIndex = 4;
            this.LicenseKeyBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(258, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Por favor escriba su número de licencia";
            // 
            // ValidateButton
            // 
            this.ValidateButton.Location = new System.Drawing.Point(318, 51);
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.Size = new System.Drawing.Size(100, 48);
            this.ValidateButton.TabIndex = 6;
            this.ValidateButton.Text = "Load";
            this.ValidateButton.UseVisualStyleBackColor = true;
            this.ValidateButton.Click += new System.EventHandler(this.ValidateButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(318, 390);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(100, 48);
            this.CloseButton.TabIndex = 7;
            this.CloseButton.Text = "Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // SucursalList
            // 
            this.SucursalList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SucursalList.FormattingEnabled = true;
            this.SucursalList.Location = new System.Drawing.Point(12, 181);
            this.SucursalList.Name = "SucursalList";
            this.SucursalList.Size = new System.Drawing.Size(169, 24);
            this.SucursalList.TabIndex = 8;
            // 
            // SucursalLbl
            // 
            this.SucursalLbl.AutoSize = true;
            this.SucursalLbl.Location = new System.Drawing.Point(10, 161);
            this.SucursalLbl.Name = "SucursalLbl";
            this.SucursalLbl.Size = new System.Drawing.Size(63, 17);
            this.SucursalLbl.TabIndex = 9;
            this.SucursalLbl.Text = "Sucursal";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Terminal";
            // 
            // TerminalList
            // 
            this.TerminalList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TerminalList.FormattingEnabled = true;
            this.TerminalList.Location = new System.Drawing.Point(12, 256);
            this.TerminalList.Name = "TerminalList";
            this.TerminalList.Size = new System.Drawing.Size(169, 24);
            this.TerminalList.TabIndex = 10;
            // 
            // SupportLink
            // 
            this.SupportLink.AutoSize = true;
            this.SupportLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SupportLink.Location = new System.Drawing.Point(8, 418);
            this.SupportLink.Name = "SupportLink";
            this.SupportLink.Size = new System.Drawing.Size(209, 20);
            this.SupportLink.TabIndex = 12;
            this.SupportLink.TabStop = true;
            this.SupportLink.Text = "soporte@skyticket.com.mx";
            this.SupportLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SupportLink_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(315, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Por favor seleccione una sucursal y una terminal";
            // 
            // ProceedButton
            // 
            this.ProceedButton.Enabled = false;
            this.ProceedButton.Location = new System.Drawing.Point(318, 282);
            this.ProceedButton.Name = "ProceedButton";
            this.ProceedButton.Size = new System.Drawing.Size(100, 48);
            this.ProceedButton.TabIndex = 14;
            this.ProceedButton.Text = "Proceed";
            this.ProceedButton.UseVisualStyleBackColor = true;
            this.ProceedButton.Click += new System.EventHandler(this.ProceedButton_Click);
            // 
            // SkipButton
            // 
            this.SkipButton.Location = new System.Drawing.Point(318, 336);
            this.SkipButton.Name = "SkipButton";
            this.SkipButton.Size = new System.Drawing.Size(100, 48);
            this.SkipButton.TabIndex = 15;
            this.SkipButton.Text = "Skip";
            this.SkipButton.UseVisualStyleBackColor = true;
            this.SkipButton.Click += new System.EventHandler(this.SkipButton_Click);
            // 
            // LicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 450);
            this.Controls.Add(this.SkipButton);
            this.Controls.Add(this.ProceedButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SupportLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TerminalList);
            this.Controls.Add(this.SucursalLbl);
            this.Controls.Add(this.SucursalList);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ValidateButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LicenseKeyBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Skyticket";
            this.Load += new System.EventHandler(this.LicenseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LicenseKeyBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ValidateButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ComboBox SucursalList;
        private System.Windows.Forms.Label SucursalLbl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox TerminalList;
        private System.Windows.Forms.LinkLabel SupportLink;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ProceedButton;
        private System.Windows.Forms.Button SkipButton;
    }
}

