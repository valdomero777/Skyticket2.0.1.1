using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Data.SqlClient;
using Microsoft.Win32;
using System.Drawing.Printing;
using Npgsql;
using System.Threading;
using System.Globalization;
using System.IO.Ports;

namespace Skyticket
{
    public partial class SettingForm : Form
    {
        string language = "";
        public SettingForm()
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
        //*******************************//
        private void OptionsForm_Load(object sender, EventArgs e)
        {
            OutputFolderBox.Text = Settings.CurrentSettings.OutputPath;

            PrinterBox.Text = Settings.CurrentSettings.PrinterIP;

            if (Settings.CurrentSettings.ConnectionType == ConnectionTypes.Network)
                NetworkRadioButton.Checked = true;
            else
                WinRadioButton.Checked = true;

            PrintersListBox.Items.Clear();
            foreach (String Printer in PrinterSettings.InstalledPrinters)
            {
                PrintersListBox.Items.Add(Printer.ToString());
            }

            if (Settings.CurrentSettings.PrinterName.Length > 0)
                PrintersListBox.SelectedItem = Settings.CurrentSettings.PrinterName;

            DocPrinterCheck.Checked = Settings.CurrentSettings.DocumentPrinter;
            PrintPaperCheck.Checked = Settings.CurrentSettings.PrintPaperAlways;
            PrintCustomerCheck.Checked = Settings.CurrentSettings.PrintCustomerInfo;

            
            TerminalIDBox.Text = Settings.CurrentSettings.TerminalID;
            ClientIDBox.Text = Settings.CurrentSettings.ClientID;
            LanguagesBox.SelectedItem = Settings.CurrentSettings.Language;
            PhoneSuffixBox.Text = Settings.CurrentSettings.PhoneSuffix;
            PhoneDigitsBox.Value = Settings.CurrentSettings.PhoneDigits;

            PortsListBox.DataSource = SerialPort.GetPortNames();
            PortsListBox.SelectedItem = Settings.CurrentSettings.SerialPort;
            BarcodesCheck.Checked = Settings.CurrentSettings.EnableBarcodes;

            

           

            MinimizeTrayBox.Checked = Settings.CurrentSettings.MinimizeToTray;
            CustFeedbackCheck.Checked = Settings.CurrentSettings.CustomerFeedback;

            cmbTypePos.SelectedItem = Settings.CurrentSettings.PosType.ToString();
            chckNoPrint.Checked = Settings.CurrentSettings.NoPrint;

        }
        //*******************************//
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(cmbTypePos.Text == "Aloha")
            {
                Settings.CurrentSettings.PosType = POSTypes.Aloha;
            }else if(cmbTypePos.Text == "Micros")
            {
                Settings.CurrentSettings.PosType = POSTypes.Micros;
            }
            else if (cmbTypePos.Text == "Siapa")
            {
                Settings.CurrentSettings.PosType = POSTypes.Siapa;
            }
            else if (cmbTypePos.Text == "Others")
            {
                Settings.CurrentSettings.PosType = POSTypes.Others;
            }
            else if (cmbTypePos.Text == "OPOS")
            {
                Settings.CurrentSettings.PosType = POSTypes.OPOS;
            }

            Settings.CurrentSettings.OutputPath = OutputFolderBox.Text;

            if (NetworkRadioButton.Checked)
                Settings.CurrentSettings.ConnectionType = ConnectionTypes.Network;
            else if (WinRadioButton.Checked)
                Settings.CurrentSettings.ConnectionType = ConnectionTypes.WinPrinter;

            Settings.CurrentSettings.PrinterIP = PrinterBox.Text;
            if (PrintersListBox.SelectedIndex >= 0)
                Settings.CurrentSettings.PrinterName = PrintersListBox.SelectedItem.ToString();

            Settings.CurrentSettings.DocumentPrinter = DocPrinterCheck.Checked;
            Settings.CurrentSettings.PrintPaperAlways = PrintPaperCheck.Checked;
            Settings.CurrentSettings.PrintCustomerInfo = PrintCustomerCheck.Checked;

           

            Settings.CurrentSettings.TerminalID = TerminalIDBox.Text;
            Settings.CurrentSettings.ClientID = ClientIDBox.Text;
            Settings.CurrentSettings.Language = LanguagesBox.SelectedItem.ToString();
            Settings.CurrentSettings.PhoneSuffix = PhoneSuffixBox.Text;
            Settings.CurrentSettings.PhoneDigits = (int)PhoneDigitsBox.Value;

            if (PortsListBox.SelectedIndex >= 0)
                Settings.CurrentSettings.SerialPort = PortsListBox.SelectedItem.ToString();
            Settings.CurrentSettings.EnableBarcodes = BarcodesCheck.Checked;

            
            Settings.CurrentSettings.PoweredLogoEnabled = PoweredCheck.Checked;


            Settings.CurrentSettings.MinimizeToTray = MinimizeTrayBox.Checked;
            Settings.CurrentSettings.CustomerFeedback = CustFeedbackCheck.Checked;
            Settings.CurrentSettings.NoPrint = chckNoPrint.Checked; 

            Settings.SaveSettings();
            this.Close();
        }
        //*******************************//
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                OutputFolderBox.Text = folderBrowser.SelectedPath;
            }
        }
        //*******************************//
      
        //*******************************//
        private void TestClientButton_Click(object sender, EventArgs e)
        {
            int clientID = Convert.ToInt32(ClientIDBox.Text);
            int terminalID = Convert.ToInt32(TerminalIDBox.Text);

            if (License.VerifyClientTerminal(clientID, terminalID))
                MessageBox.Show("Correct");
            else
                MessageBox.Show("Incorrect");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
               
            }
        }
        //*******************************//

    }
}
