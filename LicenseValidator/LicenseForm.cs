using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyticket
{
    public partial class LicenseForm : Form
    {
        List<TerminalInfo> infos = new List<TerminalInfo>();
        public LicenseForm()
        {
            InitializeComponent();
        }
        //***********************************//
        private void LicenseForm_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        //***********************************//
        private async void ValidateButton_Click (object sender, EventArgs e)
        {
            ValidateButton.Enabled = false;
            ProceedButton.Enabled = false;
            try
            {

                infos = await TerminalInfo.LoadTerminals(LicenseKeyBox.Text);

               

                if (infos.Count > 0)
                {
                    foreach (TerminalInfo info in infos)
                    {
                        SucursalList.Items.Add(info.nombreSucursal);
                        TerminalList.Items.Add(info.nombreTerminal);
                        
                    }

                    ProceedButton.Enabled = true;
                }
                else
                {
                    MessageBox.Show("El número de licencia no es válido. Por favor verifíquelo o contacte al equipo de soporte");
                }
            }
            catch (Exception)
            {

            }
            ValidateButton.Enabled = true;
        }
        //***********************************//
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Environment.Exit(0);
            this.Close();
        }
        //***********************************//
        private void SupportLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:" + SupportLink.Text);
        }
        //***********************************//
        private void ProceedButton_Click(object sender, EventArgs e)
        {
            try
            {

              
                Settings.CurrentSettings = new Settings();

                int sucursalIndex = SucursalList.SelectedIndex;
                int terminalIndex = TerminalList.SelectedIndex;
               
                Settings.CurrentSettings.ActivationKey = LicenseKeyBox.Text;

                Settings.CurrentSettings.ClientID = infos[sucursalIndex].id_cliente.ToString();
                Settings.CurrentSettings.TerminalID = infos[terminalIndex].idterminal.ToString();

                Settings.CurrentSettings.OutputPath = Settings.processedDirectory;
                Settings.CurrentSettings.PoweredLogoEnabled = true;

                Settings.CurrentSettings.FTPServer = "ftp://104.197.8.205";
                Settings.CurrentSettings.FTPPort = 24;
                Settings.CurrentSettings.FTPUser = "ftpuser";
                Settings.CurrentSettings.FTPPassword = "1Web$t3r3";
                Settings.CurrentSettings.FTPTicketsFolder = "virtualprinter";
                Settings.CurrentSettings.FTPCouponsFolder = "coupons";

                Settings.CurrentSettings.DBServer = "104.197.8.205";
                Settings.CurrentSettings.DBName = "virtualprinter";
                Settings.CurrentSettings.DBUsername = "postgres";
                Settings.CurrentSettings.DBPassword = "Webmaster3d";

                Settings.CurrentSettings.ServiceEnabled = true;
                Settings.CurrentSettings.ServiceUser = "PruebasMx";
                Settings.CurrentSettings.ServicePassword = "1qazxd";

                Settings.CurrentSettings.ConnectionType = ConnectionTypes.Network;
                Settings.CurrentSettings.PrinterIP = "127.0.0.1";

                Settings.SaveSettings();
                Environment.Exit(987);
                //this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            //Settings.CurrentSettings.FTPServer
        }
        //***********************************//
        private void SkipButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(987);
        }
        //***********************************//
    }
}
