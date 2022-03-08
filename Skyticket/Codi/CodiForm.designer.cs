namespace Skyticket
{
    partial class CodiForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodiForm));
            this.BankRequestButton = new System.Windows.Forms.Button();
            this.QRButton = new System.Windows.Forms.Button();
            this.WhatsappButton = new System.Windows.Forms.Button();
            this.TxnHistoryButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BankRequestButton
            // 
            resources.ApplyResources(this.BankRequestButton, "BankRequestButton");
            this.BankRequestButton.Name = "BankRequestButton";
            this.BankRequestButton.UseVisualStyleBackColor = true;
            this.BankRequestButton.Click += new System.EventHandler(this.BankRequestButton_Click);
            // 
            // QRButton
            // 
            resources.ApplyResources(this.QRButton, "QRButton");
            this.QRButton.Name = "QRButton";
            this.QRButton.UseVisualStyleBackColor = true;
            this.QRButton.Click += new System.EventHandler(this.QRButton_Click);
            // 
            // WhatsappButton
            // 
            resources.ApplyResources(this.WhatsappButton, "WhatsappButton");
            this.WhatsappButton.Name = "WhatsappButton";
            this.WhatsappButton.UseVisualStyleBackColor = true;
            this.WhatsappButton.Click += new System.EventHandler(this.WhatsappButton_Click);
            // 
            // TxnHistoryButton
            // 
            resources.ApplyResources(this.TxnHistoryButton, "TxnHistoryButton");
            this.TxnHistoryButton.Name = "TxnHistoryButton";
            this.TxnHistoryButton.UseVisualStyleBackColor = true;
            this.TxnHistoryButton.Click += new System.EventHandler(this.TxnHistoryButton_Click);
            // 
            // CodiForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TxnHistoryButton);
            this.Controls.Add(this.WhatsappButton);
            this.Controls.Add(this.QRButton);
            this.Controls.Add(this.BankRequestButton);
            this.MaximizeBox = false;
            this.Name = "CodiForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodiForm_FormClosing);
            this.Load += new System.EventHandler(this.CodiForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BankRequestButton;
        private System.Windows.Forms.Button QRButton;
        private System.Windows.Forms.Button WhatsappButton;
        private System.Windows.Forms.Button TxnHistoryButton;
    }
}