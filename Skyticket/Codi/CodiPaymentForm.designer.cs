namespace Skyticket
{
    partial class CodiPaymentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodiPaymentForm));
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.IDCodiBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.IDCobroBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StatusLbl = new System.Windows.Forms.Label();
            this.AmountBox = new System.Windows.Forms.NumericUpDown();
            this.AbortButton = new System.Windows.Forms.Button();
            this.PhoneBox = new System.Windows.Forms.TextBox();
            this.phoneLbl = new System.Windows.Forms.Label();
            this.QuitButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AmountBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Name = "label2";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.IDCodiBox);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.IDCobroBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.StatusLbl);
            this.panel1.Controls.Add(this.AmountBox);
            this.panel1.Controls.Add(this.AbortButton);
            this.panel1.Controls.Add(this.PhoneBox);
            this.panel1.Controls.Add(this.phoneLbl);
            this.panel1.Controls.Add(this.QuitButton);
            this.panel1.Controls.Add(this.SendButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Name = "panel1";
            // 
            // IDCodiBox
            // 
            resources.ApplyResources(this.IDCodiBox, "IDCodiBox");
            this.IDCodiBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.IDCodiBox.Name = "IDCodiBox";
            this.IDCodiBox.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Name = "label4";
            // 
            // IDCobroBox
            // 
            resources.ApplyResources(this.IDCobroBox, "IDCobroBox");
            this.IDCobroBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.IDCobroBox.Name = "IDCobroBox";
            this.IDCobroBox.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Name = "label1";
            // 
            // StatusLbl
            // 
            resources.ApplyResources(this.StatusLbl, "StatusLbl");
            this.StatusLbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.StatusLbl.Name = "StatusLbl";
            // 
            // AmountBox
            // 
            this.AmountBox.DecimalPlaces = 2;
            resources.ApplyResources(this.AmountBox, "AmountBox");
            this.AmountBox.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.AmountBox.Name = "AmountBox";
            // 
            // AbortButton
            // 
            resources.ApplyResources(this.AbortButton, "AbortButton");
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // PhoneBox
            // 
            resources.ApplyResources(this.PhoneBox, "PhoneBox");
            this.PhoneBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.PhoneBox.Name = "PhoneBox";
            // 
            // phoneLbl
            // 
            resources.ApplyResources(this.phoneLbl, "phoneLbl");
            this.phoneLbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.phoneLbl.Name = "phoneLbl";
            // 
            // QuitButton
            // 
            resources.ApplyResources(this.QuitButton, "QuitButton");
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // SendButton
            // 
            resources.ApplyResources(this.SendButton, "SendButton");
            this.SendButton.Name = "SendButton";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // CodiPaymentForm
            // 
            this.AcceptButton = this.SendButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CodiPaymentForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodiPaymentForm_FormClosing);
            this.Load += new System.EventHandler(this.CodiPaymentForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AmountBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox PhoneBox;
        private System.Windows.Forms.Label phoneLbl;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.NumericUpDown AmountBox;
        private System.Windows.Forms.Label StatusLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox IDCobroBox;
        private System.Windows.Forms.TextBox IDCodiBox;
        private System.Windows.Forms.Label label3;
    }
}