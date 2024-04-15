namespace Skyticket
{
    partial class TicketDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TicketDialog));
            this.InputBox = new System.Windows.Forms.TextBox();
            this.InputLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.MethodLabel = new System.Windows.Forms.Label();
            this.PaperButton = new System.Windows.Forms.Button();
            this.WhatsappButton = new System.Windows.Forms.Button();
            this.BatchButton = new System.Windows.Forms.Button();
            this.TicketLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InputBox
            // 
            this.InputBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.InputBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            resources.ApplyResources(this.InputBox, "InputBox");
            this.InputBox.Name = "InputBox";
            this.InputBox.TextChanged += new System.EventHandler(this.InputBox_TextChanged);
            // 
            // InputLabel
            // 
            resources.ApplyResources(this.InputLabel, "InputLabel");
            this.InputLabel.Name = "InputLabel";
            // 
            // OKButton
            // 
            this.OKButton.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.OKButton, "OKButton");
            this.OKButton.Name = "OKButton";
            this.OKButton.UseVisualStyleBackColor = false;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // MethodLabel
            // 
            resources.ApplyResources(this.MethodLabel, "MethodLabel");
            this.MethodLabel.Name = "MethodLabel";
            // 
            // PaperButton
            // 
            resources.ApplyResources(this.PaperButton, "PaperButton");
            this.PaperButton.BackColor = System.Drawing.Color.White;
            this.PaperButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(92)))), ((int)(((byte)(44)))));
            this.PaperButton.Name = "PaperButton";
            this.PaperButton.UseVisualStyleBackColor = false;
            this.PaperButton.Click += new System.EventHandler(this.PaperButton_Click);
            // 
            // WhatsappButton
            // 
            resources.ApplyResources(this.WhatsappButton, "WhatsappButton");
            this.WhatsappButton.BackColor = System.Drawing.Color.White;
            this.WhatsappButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(92)))), ((int)(((byte)(44)))));
            this.WhatsappButton.Name = "WhatsappButton";
            this.WhatsappButton.UseVisualStyleBackColor = false;
            this.WhatsappButton.Click += new System.EventHandler(this.WhatsappButton_Click);
            // 
            // BatchButton
            // 
            resources.ApplyResources(this.BatchButton, "BatchButton");
            this.BatchButton.BackColor = System.Drawing.Color.White;
            this.BatchButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(92)))), ((int)(((byte)(44)))));
            this.BatchButton.Name = "BatchButton";
            this.BatchButton.UseVisualStyleBackColor = false;
            this.BatchButton.Click += new System.EventHandler(this.BatchButton_Click);
            // 
            // TicketLabel
            // 
            resources.ApplyResources(this.TicketLabel, "TicketLabel");
            this.TicketLabel.Name = "TicketLabel";
            // 
            // TicketDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Controls.Add(this.TicketLabel);
            this.Controls.Add(this.BatchButton);
            this.Controls.Add(this.MethodLabel);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.InputLabel);
            this.Controls.Add(this.PaperButton);
            this.Controls.Add(this.WhatsappButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TicketDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TicketDialog_FormClosing);
            this.Load += new System.EventHandler(this.TicketDialog_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TicketDialog_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button WhatsappButton;
        private System.Windows.Forms.Button PaperButton;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label MethodLabel;
        private System.Windows.Forms.Button BatchButton;
        private System.Windows.Forms.Label TicketLabel;
    }
}