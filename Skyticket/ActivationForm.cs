using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web;
using System.Threading;
using System.Globalization;

namespace Skyticket
{
    public partial class ActivationForm : Form
    {
        string language = "";

        public ActivationForm()
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
        private void ActivationForm_Load(object sender, EventArgs e)
        {
            int clientID = Convert.ToInt32(Settings.CurrentSettings.ClientID);
            string activationKey = Settings.CurrentSettings.ActivationKey;
            ActivationKeyBox.Text = activationKey;

            if (string.IsNullOrEmpty(activationKey))
            {
                if (language.Contains("es"))
                {
                    label_Name.Text = "Clave de Activación:";
                    ActivateButton.Text = "Activar";
                    label3.Text = TextsSpanish.UnRegistered;
                }
                else
                {
                    label_Name.Text = "Activation Key:";
                    ActivateButton.Text = "Activate";
                    label3.Text = Texts.UnRegistered;
                }
            }
            else
            {
                Program.isActivated = License.VerifyLicense(clientID, activationKey);
                if (Program.isActivated)
                {
                    
                    if (language.Contains("es"))
                    {
                        label3.Text = TextsSpanish.Activated;
                    }
                    else
                    {
                        label3.Text = Texts.Activated;
                    }
                }
            }
        }
        //***********************************//
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //***********************************//
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            int clientID = Convert.ToInt32(Settings.CurrentSettings.ClientID);
            string activationKey = ActivationKeyBox.Text;

            if (!string.IsNullOrEmpty(activationKey))
            {
                if (License.VerifyLicense(clientID, activationKey))
                {
                    Program.isActivated = true;
                    Settings.CurrentSettings.ActivationKey = activationKey;
                    Settings.SaveSettings();
                    if (language.Contains("es"))
                        MessageBox.Show(TextsSpanish.Activated);
                    else
                        MessageBox.Show(Texts.Activated);
                    this.Close();
                }
                else
                {
                    Program.isActivated = false;
                    Settings.CurrentSettings.ActivationKey = "";
                    Settings.SaveSettings();
                    if (language.Contains("es"))
                        MessageBox.Show(TextsSpanish.ValidKey);
                    else
                        MessageBox.Show(Texts.ValidKey);
                }
            }
            else
            {
                MessageBox.Show(Texts.ValidKey);
            }
        }
        //***********************************//
        private void PasteButton_Click(object sender, EventArgs e)
        {
            ActivationKeyBox.Text = Clipboard.GetText();
        }
        //***********************************//
    }
}
