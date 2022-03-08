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

namespace Skyticket
{
    public partial class FeedbackForm : Form
    {
        string language = "";

        static FeedbackForm feedbackDialog;

        Regex numberRgx = new Regex("[^0-9]");

        int selectedAge = 30;

        public FeedbackForm()
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
        private void FeedbackForm_Load(object sender, EventArgs e)
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
                        this.TopMost = true;
                        this.Activate();

                        WindowHelper.SetForegroundWindow(this.Handle);
                        WindowHelper.SwitchToThisWindow(this.Handle, true);
                        this.BringToFront();
                        this.Focus();
                        WindowHelper.ActivateEx(this.Handle);

                        //string[] arr = contactsInfo.ToArray();
                        //InputBox.Invoke(new Action(() =>
                        //{
                        //    InputBox.AutoCompleteCustomSource.Clear();
                        //    InputBox.AutoCompleteCustomSource.AddRange(arr);
                        //}));
                    }
                    catch (Exception)
                    {
                    }
                }));
            });
        }
        //***************************************//
        public static void ShowPopUp()
        {
            feedbackDialog = new FeedbackForm();
            feedbackDialog.ShowDialog();
        }
        //***************************************//
        private void OKButton_Click(object sender, EventArgs e)
        {
            SaveFeedback();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        //***************************************//
        private bool SaveFeedback()
        {
            bool result = false;
            try
            {
                int ticketID = DBProvider.GetLastTicketID();

                lock (DBProvider.remoteDBLock)
                {
                    using (NpgsqlCommand saveCmd = new NpgsqlCommand())
                    {
                        saveCmd.CommandType = CommandType.Text;
                        saveCmd.Connection = DBProvider.remoteConnection;

                        string query = "INSERT INTO public.feedback(" +
                                        "\"id_ticket\", \"age_range\", \"gender\", \"comments\")" +
                                        "VALUES(@id_ticket, @age_range, @gender, @comments)";

                        saveCmd.CommandText = query;
                        saveCmd.Parameters.AddWithValue("@id_ticket", ticketID);
                        saveCmd.Parameters.AddWithValue("@age_range", selectedAge);

                        string gender = "";
                        if (MaleButton.Checked)
                            gender = MaleButton.Tag.ToString();
                        else if (FemaleButton.Checked)
                            gender = FemaleButton.Tag.ToString();
                        else if (OtherButton.Checked)
                            gender = OtherButton.Tag.ToString();

                        saveCmd.Parameters.AddWithValue("@gender", gender);
                        saveCmd.Parameters.AddWithValue("@comments", CommentsBox.Text);

                        int temp = saveCmd.ExecuteNonQuery();
                        result = true;
                    }
                }
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
        private void AgeButton_Click(object sender, EventArgs e)
        {
            this.KeyPress -= FeedbackForm_KeyPress;
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
        private void FeedbackForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '1':
                    e.Handled = true;
                    AgeButton_Click(button1, null);
                    break;

                case '2':
                    e.Handled = true;
                    AgeButton_Click(button2, null);
                    break;

                case '3':
                    e.Handled = true;
                    AgeButton_Click(button3, null);
                    break;

                case '4':
                    e.Handled = true;
                    AgeButton_Click(button4, null);
                    break;

                case '5':
                    e.Handled = true;
                    AgeButton_Click(button5, null);
                    break;

                case '6':
                    e.Handled = true;
                    AgeButton_Click(button6, null);
                    break;

                case '7':
                    e.Handled = true;
                    AgeButton_Click(button7, null);
                    break;

                case '8':
                    e.Handled = true;
                    AgeButton_Click(button8, null);
                    break;
            }

        }
        //***************************************//
    }
}
