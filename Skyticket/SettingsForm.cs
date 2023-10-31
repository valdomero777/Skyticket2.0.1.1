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
    public partial class SettingsForm : Form
    {
        string language = "";
        public SettingsForm()
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

            DBServerBox.Text = Settings.CurrentSettings.DBServer;
            DBUsernameBox.Text = Settings.CurrentSettings.DBUsername;
            DBPasswordBox.Text = Settings.CurrentSettings.DBPassword;
            DBNameBox.Text = Settings.CurrentSettings.DBName;

            TerminalIDBox.Text = Settings.CurrentSettings.TerminalID;
            ClientIDBox.Text = Settings.CurrentSettings.ClientID;
            LanguagesBox.SelectedItem = Settings.CurrentSettings.Language;
            PhoneSuffixBox.Text = Settings.CurrentSettings.PhoneSuffix;
            PhoneDigitsBox.Value = Settings.CurrentSettings.PhoneDigits;

            PortsListBox.DataSource = SerialPort.GetPortNames();
            Port2Write.DataSource = SerialPort.GetPortNames();
            PortsListBox.SelectedItem = Settings.CurrentSettings.SerialPort;
            Port2Write.SelectedItem = Settings.CurrentSettings.SerialPortWrite;
            BarcodesCheck.Checked = Settings.CurrentSettings.EnableBarcodes;

            FTPHostBox.Text = Settings.CurrentSettings.FTPServer;
            FTPPortBox.Value = Settings.CurrentSettings.FTPPort;
            FTPUserBox.Text = Settings.CurrentSettings.FTPUser;
            FTPPasswordBox.Text = Settings.CurrentSettings.FTPPassword;
            FTPTicketsFolderBox.Text = Settings.CurrentSettings.FTPTicketsFolder;
            FTPCouponsFolderBox.Text = Settings.CurrentSettings.FTPCouponsFolder;
            CouponIntervalBox.Value = Settings.CurrentSettings.CouponLoadInterval;
            PoweredCheck.Checked = Settings.CurrentSettings.PoweredLogoEnabled;

            ServiceEnableCheck.Checked = Settings.CurrentSettings.ServiceEnabled;
            ServiceUserBox.Text = Settings.CurrentSettings.ServiceUser;
            ServicePasswordBox.Text = Settings.CurrentSettings.ServicePassword;

            CODICheck.Checked = Settings.CurrentSettings.CodiEnabled;
            CodiWinTimerBox.Value = Settings.CurrentSettings.CodiWinTimer;
            CodiProcTimerBox.Value = Settings.CurrentSettings.CodiProcTimer;

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
            else if (cmbTypePos.Text == "Star")
            {
                Settings.CurrentSettings.PosType = POSTypes.Star;
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

            Settings.CurrentSettings.DBServer = DBServerBox.Text;
            Settings.CurrentSettings.DBUsername = DBUsernameBox.Text;
            Settings.CurrentSettings.DBPassword = DBPasswordBox.Text;
            Settings.CurrentSettings.DBName = DBNameBox.Text;

            Settings.CurrentSettings.TerminalID = TerminalIDBox.Text;
            Settings.CurrentSettings.ClientID = ClientIDBox.Text;
            Settings.CurrentSettings.Language = LanguagesBox.SelectedItem.ToString();
            Settings.CurrentSettings.PhoneSuffix = PhoneSuffixBox.Text;
            Settings.CurrentSettings.PhoneDigits = (int)PhoneDigitsBox.Value;

            if (PortsListBox.SelectedIndex >= 0)
                Settings.CurrentSettings.SerialPort = PortsListBox.SelectedItem.ToString(); 
            if (Port2Write.SelectedIndex >= 0)
                Settings.CurrentSettings.SerialPortWrite = Port2Write.SelectedItem.ToString();
            Settings.CurrentSettings.EnableBarcodes = BarcodesCheck.Checked;

            Settings.CurrentSettings.FTPServer = FTPHostBox.Text;
            Settings.CurrentSettings.FTPPort = (int)FTPPortBox.Value;
            Settings.CurrentSettings.FTPUser = FTPUserBox.Text;
            Settings.CurrentSettings.FTPPassword = FTPPasswordBox.Text;
            Settings.CurrentSettings.FTPTicketsFolder = FTPTicketsFolderBox.Text;
            Settings.CurrentSettings.FTPCouponsFolder = FTPCouponsFolderBox.Text;
            Settings.CurrentSettings.CouponLoadInterval = (int)CouponIntervalBox.Value;
            Settings.CurrentSettings.PoweredLogoEnabled = PoweredCheck.Checked;

            Settings.CurrentSettings.ServiceEnabled = ServiceEnableCheck.Checked;
            Settings.CurrentSettings.ServiceUser = ServiceUserBox.Text;
            Settings.CurrentSettings.ServicePassword = ServicePasswordBox.Text;

            Settings.CurrentSettings.CodiEnabled = CODICheck.Checked;
            Settings.CurrentSettings.CodiWinTimer = (int)CodiWinTimerBox.Value;
            Settings.CurrentSettings.CodiProcTimer = (int)CodiProcTimerBox.Value;

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
        private void TestDBButton_Click(object sender, EventArgs e)
        {
            //string dbStringTemplate = @"Host={0};Username={1};Password={2};Database={3}";
            string connString = @"Host={0};Username={1};Password={2};Database={3}";
            connString = String.Format(connString,
                                    DBServerBox.Text,
                                    DBUsernameBox.Text,
                                    DBPasswordBox.Text,
                                    DBNameBox.Text);

            using (NpgsqlConnection connection = new NpgsqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        if (language.Contains("es"))
                            MessageBox.Show(TextsSpanish.ConnectionSuccess);
                        else
                            MessageBox.Show(Texts.ConnectionSuccess);
                        //SaveButton_Click(null, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
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
        //*******************************//

    }
}
