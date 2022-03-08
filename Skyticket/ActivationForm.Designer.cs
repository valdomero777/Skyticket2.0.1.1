namespace Skyticket
{
    partial class ActivationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActivationForm));
            this.groupBox_R = new System.Windows.Forms.GroupBox();
            this.label_Name = new System.Windows.Forms.Label();
            this.ActivateButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ActivationKeyBox = new System.Windows.Forms.TextBox();
            this.groupBox_R.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_R
            // 
            resources.ApplyResources(this.groupBox_R, "groupBox_R");
            this.groupBox_R.Controls.Add(this.label_Name);
            this.groupBox_R.Controls.Add(this.ActivateButton);
            this.groupBox_R.Controls.Add(this.label3);
            this.groupBox_R.Controls.Add(this.ActivationKeyBox);
            this.groupBox_R.Name = "groupBox_R";
            this.groupBox_R.TabStop = false;
            // 
            // label_Name
            // 
            resources.ApplyResources(this.label_Name, "label_Name");
            this.label_Name.Name = "label_Name";
            // 
            // ActivateButton
            // 
            resources.ApplyResources(this.ActivateButton, "ActivateButton");
            this.ActivateButton.Name = "ActivateButton";
            this.ActivateButton.UseVisualStyleBackColor = true;
            this.ActivateButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label3.Name = "label3";
            // 
            // ActivationKeyBox
            // 
            resources.ApplyResources(this.ActivationKeyBox, "ActivationKeyBox");
            this.ActivationKeyBox.Name = "ActivationKeyBox";
            // 
            // ActivationForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_R);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActivationForm";
            this.Load += new System.EventHandler(this.ActivationForm_Load);
            this.groupBox_R.ResumeLayout(false);
            this.groupBox_R.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_R;
        private System.Windows.Forms.Label label_Name;
        private System.Windows.Forms.Button ActivateButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ActivationKeyBox;
    }
}