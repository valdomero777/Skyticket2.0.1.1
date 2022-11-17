using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using Npgsql;
using System.Data;
using Skyticket.Classes;
using System.Linq;
using RestSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.ComponentModel;

namespace Skyticket
{

    public partial class TicketDialog : Form
    {
        string language = "";

        static TicketDialog dialog;

        private static TicketChoice choice = new TicketChoice();

        internal static List<string> contactsInfo = new List<string>();

        Regex numberRgx = new Regex("[^0-9]");
        public static bool isCopied = false;
        public static string phone = "";

        public TicketDialog()
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
        
        //***************************************//
        private void TicketDialog_Load(object sender, EventArgs e)
        {
         

            ThreadPool.QueueUserWorkItem(delegate
            {
                Thread.Sleep(100);

                this.Invoke(new Action(() =>
                {
                    try
                    {
                        this.WindowState = FormWindowState.Normal;
                        this.TopLevel = true;
                        this.TopMost = false;
                        this.TopMost = true;
                        this.Activate();

                        WindowHelper.SetForegroundWindow(this.Handle);
                        WindowHelper.SwitchToThisWindow(this.Handle, true);
                        this.BringToFront();
                        this.Focus();
                        WindowHelper.ActivateEx(this.Handle);

                       


                       

                        this.Height = 150;

                        string[] arr = contactsInfo.ToArray();
                        InputBox.Invoke(new Action(() =>
                        {
                            InputBox.AutoCompleteCustomSource.Clear();
                            InputBox.AutoCompleteCustomSource.AddRange(arr);
                        }));
                    }
                    catch (Exception)
                    {
                    }
                    MainForm.UsersList.ForEach(user => cmbSuc.Items.Add(user.sucursal));
                }));
            });

            CountDownTimer timer = new CountDownTimer();
            timer.Start();
            timer.StepMs = 50;
           
        }
        //***************************************//
        private void TicketDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        //***************************************//
        public static TicketChoice ShowPopUp()
        {
            choice = new TicketChoice();

            dialog = new TicketDialog();

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                choice = new TicketChoice();
            }

            return choice;
        }
        //***************************************//
        public static void ClosePopup()
        {
            
            

            try
            {
                dialog.Invoke(new Action(()=>
                {
                    dialog.FormClosing -= dialog.TicketDialog_FormClosing;
                    dialog.Close();
                }));
            }
            catch (Exception)
            {
            }
        }
        //***************************************//
        private void TicketDialog_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch('2')
            {
                case '1':
                    e.Handled = true;
                    PaperButton_Click(null, null);
                    break;

                case '2':
                    e.Handled = true;
                    WhatsappButton_Click(null, null);
                    break;

                case '3':
                    e.Handled = true;
                    TelegramButton_Click(null, null);
                    break;

                case '4':
                    e.Handled = true;
                    SMSButton_Click(null, null);
                    break;

                case '5':
                    e.Handled = true;
                    EmailButton_Click(null, null);
                    break;

                case '6':
                    e.Handled = true;
                    BatchButton_Click(null, null);
                    break;
            }
            
        }
        //***************************************//
        private void PaperButton_Click(object sender, EventArgs e)
        {
            this.KeyPress -= TicketDialog_KeyPress;
            choice.printMethod = TicketMethod.Paper;
            this.DialogResult = DialogResult.OK;
            this.FormClosing -= TicketDialog_FormClosing;
            this.Close();
        }
        //***************************************//
        private void WhatsappButton_Click(object sender, EventArgs e)
        {
            this.KeyPress -= TicketDialog_KeyPress;
            choice.printMethod = TicketMethod.Whatsapp;
            //InputLabel.Text = "Mobile phone:";

           
            this.AcceptButton = OKButton;

           
            
            InputBox.SelectionStart = InputBox.Text.Length;
            InputBox.Focus();
        }
        //***************************************//
        private void TelegramButton_Click(object sender, EventArgs e)
        {
            this.KeyPress -= TicketDialog_KeyPress;
            choice.printMethod = TicketMethod.NoPrint;

            this.Height = 545;
            this.AcceptButton = OKButton;

            
            InputBox.SelectionStart = InputBox.Text.Length;
            InputBox.Focus();
        }
        //***************************************//
        private void SMSButton_Click(object sender, EventArgs e)
        {
            this.KeyPress -= TicketDialog_KeyPress;
            choice.printMethod = TicketMethod.SMS;
            InputLabel.Text = "Mobile phone:";

            this.Height = 545;
            this.AcceptButton = OKButton;

            
            InputBox.SelectionStart = InputBox.Text.Length;
            InputBox.Focus();
        }
        //***************************************//
        private void EmailButton_Click(object sender, EventArgs e)
        {
            this.KeyPress -= TicketDialog_KeyPress;
            choice.printMethod = TicketMethod.Email;
            InputLabel.Text = "Email:";

            this.Height = 545;
            this.AcceptButton = OKButton;

           
            InputBox.Focus();
        }
        //***************************************//
        private void OKButton_Click(object sender, EventArgs e)
        {
            choice.printMethod = TicketMethod.Whatsapp;

            choice.targetInput = InputBox.Text;

            if (choice.printMethod == TicketMethod.SMS ||
                choice.printMethod == TicketMethod.Whatsapp)
            {
                string input = Settings.CurrentSettings.PhoneSuffix + choice.targetInput;
                if (input.Length < Settings.CurrentSettings.PhoneDigits)
                {
                    if (language.Contains("es"))
                        MessageBox.Show(String.Format(TextsSpanish.InvalidNumber, Settings.CurrentSettings.PhoneDigits));
                    else
                        MessageBox.Show(String.Format(Texts.InvalidNumber, Settings.CurrentSettings.PhoneDigits));
                    return;
                }

                if (CheckPhoneBlacklist(choice.targetInput))
                {
                    choice.printMethod = TicketMethod.Paper;
                    choice.targetInput = "";
                }
                else
                    choice.targetInput = input;
            }

            if (InputBox.Text.Length > 0)
            {
                CustomerInfo.SaveCustomerInfo(InputBox.Text);
                contactsInfo = CustomerInfo.LoadCustomerInfo();
            }

           

            this.DialogResult = DialogResult.OK;
            this.FormClosing -= TicketDialog_FormClosing;
            this.Close();
        }
        //***************************************//
        private void InputBox_TextChanged(object sender, EventArgs e)
        {
            if (choice.printMethod == TicketMethod.SMS ||
                choice.printMethod == TicketMethod.Whatsapp)
            {
                InputBox.Text = numberRgx.Replace(InputBox.Text, "");
                InputBox.SelectionStart = InputBox.Text.Length;
            }
        }
        //***************************************//
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        //***************************************//
        private void BatchButton_Click(object sender, EventArgs e)
        {
            this.KeyPress -= TicketDialog_KeyPress;
            choice.printMethod = TicketMethod.Batch;

            this.DialogResult = DialogResult.OK;
            this.FormClosing -= TicketDialog_FormClosing;
            this.Close();
        }
        //***************************************//
        public static bool CheckPhoneBlacklist(string phone)
        {
            bool isBlackListed = false;

            #region get contactInfo
            try
            {
                string mainQuery = "SELECT * FROM ct_phone_blacklist WHERE phone_number LIKE '%{0}%'";

                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand codiCmd = new NpgsqlCommand())
                    {
                        codiCmd.CommandType = CommandType.Text;
                        codiCmd.Connection = DBProvider.remoteConnection;
                        codiCmd.CommandText = string.Format(mainQuery, phone);

                        using (NpgsqlDataReader reader = codiCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                isBlackListed = true;
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("in CheckPhoneBlacklist: " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
            }
            #endregion

            return isBlackListed;
        }
        //***************************************//
       
        //***************************************//
        
        //***************************************//
        private void CommentsBox_Enter(object sender, EventArgs e)
        {
            this.KeyPress -= TicketDialog_KeyPress;
            this.AcceptButton = null;
        }
        //***************************************//
        private void CommentsBox_Leave(object sender, EventArgs e)
        {
            this.KeyPress += TicketDialog_KeyPress;
            this.AcceptButton = OKButton;
        }

        private void cmbSuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbSuc.SelectedIndex;
            txtDestiny.Text = MainForm.UsersList[index].nombre;
            txtPhone.Text = MainForm.UsersList[index].telefono.ToString();
            phone = MainForm.UsersList[index].telefono.ToString();

            isCopied = true;

        }
        //***************************************//
    }
}
