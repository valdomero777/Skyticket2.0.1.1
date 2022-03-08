using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyticket
{
    public partial class CodiPaymentForm : Form
    {
        string language = "";
        Regex numberRgx = new Regex("[^0-9.]");

        System.Timers.Timer WindowTimer = new System.Timers.Timer();

        CodiPayment codiPayment = new CodiPayment();
        public CodiPaymentType paymentType = CodiPaymentType.BankAppPayment;
        QRForm qrForm = new QRForm();

        public CodiPaymentForm()
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
        private void CodiPaymentForm_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;

            if (paymentType == CodiPaymentType.ScreenQR)
            {
                phoneLbl.Visible = false;
                PhoneBox.Visible = false;
                PhoneBox.Text = "";
                this.Text = "CODI Payment QR in Screen";
            }
            else if (paymentType == CodiPaymentType.WhatsappQR)
            {
                this.Text = "CODI Payment QR via WhatsApp";
            }

            this.Location = Settings.CurrentSettings.codiPaymentLocation;
            this.Move += CodiPaymentForm_Move;

            WindowTimer.Interval = Settings.CurrentSettings.CodiWinTimer*1000;
            WindowTimer.Elapsed += StatusTimer_Elapsed;

            AmountBox.Focus();
        }
        //***********************************//
        private void CodiPaymentForm_Move(object sender, EventArgs e)
        {
            Settings.CurrentSettings.codiPaymentLocation = this.Location;
            Settings.SaveSettings();
        }
        //***********************************//
        private void CodiPaymentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WindowTimer.Enabled = false;
        }
        //***********************************//
        private async void SendButton_Click(object sender, EventArgs e)
        {
            if (codiPayment.idcobro.Length > 0)
                return;

            this.Invoke(new Action(() =>
            {
                SendButton.Enabled = false;
            }));
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    double amount = 0;
                    this.Invoke(new Action(() =>
                    {
                        amount = (double)AmountBox.Value;
                    }));

                    if (amount <= 0)
                    {
                        if (language.Contains("es"))
                            MessageBox.Show(TextsSpanish.CodiValidAmount, "Skyticket");
                        else
                            MessageBox.Show(Texts.CodiValidAmount, "Skyticket");
                        this.Invoke(new Action(() =>
                        {
                            SendButton.Enabled = true;
                        }));
                        return;
                    }

                    string message = "";

                    bool result = false;
                    if (string.IsNullOrEmpty(CodiAPI.token))
                        result = CodiAPI.GetAccessToken(out message);
                    else
                        result = true;

                    if (result)
                    {
                        string qrStr = "";
                        string paymentID = "";

                        string phoneToSend = PhoneBox.Text;
                        codiPayment.paymentType = paymentType;
                        if (paymentType == CodiPaymentType.WhatsappQR)
                        {
                            if (PhoneBox.Text.Length < Settings.CurrentSettings.PhoneDigits)
                            {
                                string messageText = "";
                                if (language.Contains("es"))
                                    messageText = TextsSpanish.InvalidNumber;
                                else
                                    messageText = Texts.InvalidNumber;
                                messageText = string.Format(messageText, Settings.CurrentSettings.PhoneDigits);
                                MessageBox.Show(messageText, "Skyticket");
                                return;
                            }
                            phoneToSend = "";
                        }
                        result = CodiAPI.GenerateCodiPayment(amount, phoneToSend, paymentType, out paymentID, out qrStr);

                        if (result)
                        {
                            codiPayment.idcodi = "";
                            codiPayment.status = "";
                            codiPayment.qrimage = qrStr;
                            this.Invoke(new Action(() =>
                            {
                                codiPayment.phone = PhoneBox.Text;
                                StatusLbl.Text = codiPayment.status;
                            }));
                            
                            codiPayment.monto = amount;

                            codiPayment.idcobro = paymentID;

                            this.Invoke(new Action(() =>
                            {
                                IDCobroBox.Text = codiPayment.idcobro;
                            }));

                            codiPayment.SavePayment();
                            WindowTimer.Enabled = true;

                            if (!string.IsNullOrEmpty(qrStr))
                            {
                                if (paymentType == CodiPaymentType.ScreenQR)
                                {
                                    qrForm.qrString = qrStr;
                                    this.Invoke(new Action(() =>
                                    {
                                        qrForm.Show();
                                    }));
                                }
                                else if(paymentType == CodiPaymentType.WhatsappQR)
                                {
                                    SaveQRDB(paymentID, qrStr, amount, PhoneBox.Text);
                                }
                            }
                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                SendButton.Enabled = true;
                                StatusLbl.Text = paymentID;
                            }));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error " + message);
                        this.Invoke(new Action(() =>
                        {
                            SendButton.Enabled = true;
                        }));
                    }
                }
                catch (Exception)
                {
                }
            });
        }
        //***********************************//
        private void StatusTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            WindowTimer.Enabled = false;
            try
            {
                string status = "";
                string idcodi = "";
                bool result = CodiAPI.GetPaymentStatus(codiPayment.idcobro, codiPayment.paymentType, out status, out idcodi);

                if (result)
                    this.Invoke(new Action(() =>
                    {
                        codiPayment.idcodi = idcodi;
                        this.Invoke(new Action(() =>
                        {
                            IDCodiBox.Text = idcodi;
                            //if (status == "-1")
                            //    StatusLbl.Text = codiPayment.status;
                            //else
                            StatusLbl.Text = status;
                            CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, codiPayment.idcodi, status);
                        }));

                        /*
                        if (status == "-1")
                        {
                            CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, codiPayment.idcodi, "Pending");

                            StatusLbl.Text = "Pending";
                        }
                        else if (status == "0")
                        {
                            CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, codiPayment.idcodi, "Accepted");

                            if (language.Contains("es"))
                            {
                                //MessageBox.Show(TextsSpanish.CodiAccepted, "Skyticket");
                                StatusLbl.Text = "Pago Enviado";
                            }
                            else
                            {
                                //MessageBox.Show(Texts.CodiAccepted, "Skyticket");
                                StatusLbl.Text = "Payment Sent";
                            }
                            //CloseButton_Click(null, null);
                        }
                        else if (status == "1")
                        {
                            CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, codiPayment.idcodi, "Paid");
                            if (language.Contains("es"))
                                MessageBox.Show(TextsSpanish.CodiAccepted, "Skyticket");
                            else
                                MessageBox.Show(Texts.CodiAccepted, "Skyticket");
                            CloseButton_Click(null, null);
                        }
                        else if (status == "2")
                        {
                            CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, codiPayment.idcodi, "Rejected");
                            if (language.Contains("es"))
                                MessageBox.Show(TextsSpanish.CodiDeclined, "Skyticket");
                            else
                                MessageBox.Show(Texts.CodiDeclined, "Skyticket");
                            //CloseButton_Click(null, null);
                        }
                        else if (status == "3")
                        {
                            CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, codiPayment.idcodi, "Canceled");
                            if (language.Contains("es"))
                                MessageBox.Show(TextsSpanish.CodiDeclined, "Skyticket");
                            else
                                MessageBox.Show(Texts.CodiDeclined, "Skyticket");
                            //CloseButton_Click(null, null);
                        }
                        else if (status == "4")
                        {
                            CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, codiPayment.idcodi, "Postponed");
                            CloseButton_Click(null, null);
                        }
                        else if (status == "5")
                        {
                            CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, codiPayment.idcodi, "Sent");
                            //CloseButton_Click(null, null);
                        }
                        */
                    }));
            }
            catch (Exception)
            {
            }

            try
            {
                WindowTimer.Interval = Settings.CurrentSettings.CodiWinTimer * 1000;
                WindowTimer.Enabled = true;
            }
            catch (Exception)
            {
            }
        }
        //***********************************//
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            if(qrForm.Visible)
                qrForm.Close();
            this.Close();
        }
        //***********************************//
        private static bool SaveQRDB(string paymentID, string qrStr, double amount, string mobilephone)
        {
            bool result = false;

            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            try
            {
                
                byte[] base64Bytes = Convert.FromBase64String(qrStr);
                FTP.FTPUpload(timeStamp + ".png", base64Bytes);
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("SaveQRDB.1: " + ex.Message);
            }
                try
            {
                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand saveCmd = new NpgsqlCommand())
                    {
                        saveCmd.CommandType = CommandType.Text;
                        saveCmd.Connection = DBProvider.remoteConnection;

                        string query = "INSERT INTO public.tickets(" +
                                        "id_terminal, id_client, ticketimagepath, printmethod, email, mobilephone, sent, datesent, totalpos, type)" +//
                                        "VALUES(@id_terminal, @id_client, @ticketimagepath, @printmethod, @email, @mobilephone, @sent, @datesent, @totalpos, @type)";//

                        saveCmd.CommandText = query;
                        saveCmd.Parameters.AddWithValue("@id_terminal", Convert.ToInt32(Settings.CurrentSettings.TerminalID));
                        saveCmd.Parameters.AddWithValue("@id_client", Convert.ToInt32(Settings.CurrentSettings.ClientID));
                        saveCmd.Parameters.AddWithValue("@ticketimagepath", timeStamp + ".png");
                        saveCmd.Parameters.AddWithValue("@printmethod", TicketMethod.Whatsapp.ToString());
                        saveCmd.Parameters.AddWithValue("@email", "");
                        saveCmd.Parameters.AddWithValue("@mobilephone", Settings.CurrentSettings.PhoneSuffix + mobilephone);
                        saveCmd.Parameters.AddWithValue("@sent", true);
                        saveCmd.Parameters.AddWithValue("@datesent", DateTime.Now);
                        saveCmd.Parameters.AddWithValue("@totalpos", amount);
                        saveCmd.Parameters.AddWithValue("@type", "Payment");

                        int temp = saveCmd.ExecuteNonQuery();
                        result = true;
                    }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("SaveQRDB.2: " + ex.Message);

                if (DBProvider.remoteConnection.State != ConnectionState.Open)
                    DBProvider.InitRemoteDB();
            }

            return result;
        }
    }
}
