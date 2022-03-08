using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyticket
{
    public partial class WaitingForm : Form
    {
        public static WaitingForm window;
        string language = "";

        public WaitingForm()
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.CurrentSettings.Language))
                {
                    language = Settings.CurrentSettings.Language.Split('|')[1].Replace(" ", "");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                }
            }
            catch (Exception)
            {
            }

            InitializeComponent();
        }
        //***********************************//
        private void WaitingForm_Load(object sender, EventArgs e)
        {
            if (language.Contains("es"))
                label1.Text = TextsSpanish.PreparingWait;
            else
                label1.Text = Texts.PreparingWait;

            this.TopLevel = true;
            this.TopMost = true;
            this.Activate();

            WindowHelper.SetForegroundWindow(this.Handle);
            WindowHelper.SwitchToThisWindow(this.Handle, true);
            this.BringToFront();
            this.Focus();
            WindowHelper.ActivateEx(this.Handle);
        }
        //***********************************//
        private void WaitingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        //***********************************//
        public void CloseForm()
        {
            FormClosing -= WaitingForm_FormClosing;
            this.TopLevel = false;
            this.TopMost = false;
            this.Close();
        }
        //***********************************//
    }
}
