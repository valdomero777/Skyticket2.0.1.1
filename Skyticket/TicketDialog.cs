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

namespace Skyticket
{

    public partial class TicketDialog : Form
    {
        string language = "";

        static TicketDialog dialog;

        private static TicketChoice choice = new TicketChoice();

        internal static List<string> contactsInfo = new List<string>();

        Regex numberRgx = new Regex("[^0-9]");

        static int selectedAge = 0;
        static string Gender = "";
        static string Comments = "";
        public static string clipPhone = "";
        public static Boolean coupon = false;
        public static Boolean hasAlert = false;

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
            selectedAge = 0;
            Gender = "";
            Comments = "";
            string res = "";
            string phone = "";

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

                        TicketLabel.Location = new Point(this.Width / 2 - TicketLabel.Location.X / 2, TicketLabel.Location.Y);


                        if (Settings.CurrentSettings.ConnectionType == ConnectionTypes.WinPrinter)
                            if (string.IsNullOrEmpty(Settings.CurrentSettings.PrinterName))
                                PaperButton.Enabled = false;

                        if (Settings.CurrentSettings.ConnectionType == ConnectionTypes.Network)
                            if (string.IsNullOrEmpty(Settings.CurrentSettings.PrinterIP))
                                PaperButton.Enabled = false;

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
                }));
            });

            CountDownTimer timer = new CountDownTimer();
            timer.Start();
            timer.StepMs = 50;
            BatchButton.Enabled = false;
            //update label text
            timer.TimeChanged += () =>
            {
                if (language.Contains("es"))
                    BatchButton.Text = TextsSpanish.ReceivingData + " " + timer.TimeLeftStr.Replace("m","");
                else
                    BatchButton.Text = Texts.ReceivingData + " " + timer.TimeLeftStr.Replace("m", "");
            };

            // show messageBox on timer = 00:00.000
            timer.CountDownFinished += () =>
            {
                BatchButton.Enabled = true;
                if (language.Contains("es"))
                    BatchButton.Text = "Corte";
                else
                    BatchButton.Text = "Corte";
            };

            MethodLabel.Visible = false;    
            InputLabel.Visible = false;
            InputBox.Visible = false;   
            OKButton.Visible = false;
            btnNoPrint.Visible = Settings.CurrentSettings.NoPrint;
            
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
            switch(e.KeyChar)
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
                   

                case '6':
                    e.Handled = true;
                    BatchButton_Click(null, null);
                    break;
            }
            
        }
        //***************************************//
        private void PaperButton_Click(object sender, EventArgs e)
        {
            if (MaleButton.Checked)
                Gender = MaleButton.Tag.ToString();
            else if (FemaleButton.Checked)
                Gender = FemaleButton.Tag.ToString();
            else if (OtherButton.Checked)
                Gender = OtherButton.Tag.ToString();
            Comments = $"Comentarios: {CommentsBox.Text}, Nombre: {txtName.Text}, Correo: {txtEmail.Text}, Ciudad: {txtCity.Text}";
            this.KeyPress -= TicketDialog_KeyPress;
            choice.printMethod = TicketMethod.Paper;
            this.DialogResult = DialogResult.OK;
            this.FormClosing -= TicketDialog_FormClosing;
            this.Close();
        }
        //***************************************//
        private void WhatsappButton_Click(object sender, EventArgs e)
        {
            MethodLabel.Visible =true;
            InputLabel.Visible =true;
            InputBox.Visible = true;
            OKButton.Visible = true;
            this.KeyPress -= TicketDialog_KeyPress;
            choice.printMethod = TicketMethod.Whatsapp;
            //InputLabel.Text = "Mobile phone:";

            this.Height = 650;
            this.AcceptButton = OKButton;

            MethodLabel.Text = "Whatsapp";
            
            InputBox.SelectionStart = InputBox.Text.Length;
            InputBox.Focus();
        }
        //***************************************//
        private void TelegramButton_Click(object sender, EventArgs e)
        {
            this.KeyPress -= TicketDialog_KeyPress;
            choice.printMethod = TicketMethod.NoPrint;
            this.DialogResult = DialogResult.OK;
            this.FormClosing -= TicketDialog_FormClosing;
            this.Close();
        }
        //***************************************//
        private void SMSButton_Click(object sender, EventArgs e)
        {

        }
        //***************************************//

        //***************************************//
        private void OKButton_Click(object sender, EventArgs e)
        {
            if (choice.printMethod == TicketMethod.Email)
            {
                if (!IsValidEmail(InputBox.Text))
                {
                    if (language.Contains("es"))
                        MessageBox.Show(TextsSpanish.InvalidEmail);
                    else
                        MessageBox.Show(Texts.InvalidEmail);
                    return;
                }
            }

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

            if (MaleButton.Checked)
                Gender = MaleButton.Tag.ToString();
            else if (FemaleButton.Checked)
                Gender = FemaleButton.Tag.ToString();
            else if (OtherButton.Checked)
                Gender = OtherButton.Tag.ToString();
            Comments =$"Comentarios: {CommentsBox.Text}, Nombre: {txtName.Text}, Correo: {txtEmail.Text}, Ciudad: {txtCity.Text}";

            

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
        private void AgeButton_Click(object sender, EventArgs e)
        {
            string name = ((Button)sender).Name.Replace("button", "");

            switch (name)
            {
                case "1":
                    selectedAge = 10;
                    break;

                case "2":
                    selectedAge = 15;
                    break;

                case "3":
                    selectedAge = 20;
                    break;

                case "4":
                    selectedAge = 30;
                    break;

                case "5":
                    selectedAge = 40;
                    break;

                case "6":
                    selectedAge = 50;
                    break;

                case "7":
                    selectedAge = 60;
                    break;

                case "8":
                    selectedAge = 80;
                    break;
            }

            button1.BackColor = SystemColors.Control;
            button2.BackColor = SystemColors.Control;
            button3.BackColor = SystemColors.Control;
            button4.BackColor = SystemColors.Control;
            button5.BackColor = SystemColors.Control;
            button6.BackColor = SystemColors.Control;
            button7.BackColor = SystemColors.Control;
            button8.BackColor = SystemColors.Control;
            ((Button)sender).BackColor = SystemColors.InactiveCaption;
        }
        //***************************************//
        public static bool SaveFeedback()
        {

            FeedInfo feed = new FeedInfo();

            bool result = false;
            try
            {

                
                feed.id_ticket = MainForm.id_ticketr;
                feed.age_range = selectedAge;
                feed.gender = Gender;
                feed.comments = Comments ;

                MainForm.FeedRequestAsync(feed);


            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("SaveFeedback(): " + ex.Message);

                if (DBProvider.remoteConnection.State != ConnectionState.Open)
                    DBProvider.InitRemoteDB();
            }

            return result;
        }
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

        private void label7_Click(object sender, EventArgs e)
        {

        }

       


        //***************************************//
    }
}
