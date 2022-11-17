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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TicketDialog));
            this.InputBox = new System.Windows.Forms.TextBox();
            this.InputLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtDestiny = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelSuc = new System.Windows.Forms.Label();
            this.cmbSuc = new System.Windows.Forms.ComboBox();
            this.usersResBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.usersResBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.usersResBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usersResBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usersResBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usersResBindingSource2)).BeginInit();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OKButton);
            this.groupBox1.Controls.Add(this.txtPhone);
            this.groupBox1.Controls.Add(this.txtDestiny);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.labelSuc);
            this.groupBox1.Controls.Add(this.cmbSuc);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txtPhone
            // 
            resources.ApplyResources(this.txtPhone, "txtPhone");
            this.txtPhone.Name = "txtPhone";
            // 
            // txtDestiny
            // 
            resources.ApplyResources(this.txtDestiny, "txtDestiny");
            this.txtDestiny.Name = "txtDestiny";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labelSuc
            // 
            resources.ApplyResources(this.labelSuc, "labelSuc");
            this.labelSuc.Name = "labelSuc";
            // 
            // cmbSuc
            // 
            this.cmbSuc.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.usersResBindingSource, "users", true));
            resources.ApplyResources(this.cmbSuc, "cmbSuc");
            this.cmbSuc.FormattingEnabled = true;
            this.cmbSuc.Name = "cmbSuc";
            this.cmbSuc.SelectedIndexChanged += new System.EventHandler(this.cmbSuc_SelectedIndexChanged);
            // 
            // usersResBindingSource
            // 
            this.usersResBindingSource.DataSource = typeof(Skyticket.Classes.UsersRes);
            // 
            // usersResBindingSource1
            // 
            this.usersResBindingSource1.DataSource = typeof(Skyticket.Classes.UsersRes);
            // 
            // usersResBindingSource2
            // 
            this.usersResBindingSource2.DataSource = typeof(Skyticket.Classes.UsersRes);
            // 
            // TicketDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.InputLabel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TicketDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TicketDialog_FormClosing);
            this.Load += new System.EventHandler(this.TicketDialog_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TicketDialog_KeyPress);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usersResBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usersResBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usersResBindingSource2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtDestiny;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelSuc;
        private System.Windows.Forms.ComboBox cmbSuc;
        private System.Windows.Forms.BindingSource usersResBindingSource;
        private System.Windows.Forms.BindingSource usersResBindingSource1;
        private System.Windows.Forms.BindingSource usersResBindingSource2;
    }
}