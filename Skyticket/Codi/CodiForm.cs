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
    public partial class CodiForm : Form
    {
        public CodiForm()
        {
            InitializeComponent();
        }
        //***********************************//
        private void CodiForm_Load(object sender, EventArgs e)
        {
            this.Location = Settings.CurrentSettings.codiPanelLocation;
            this.Move += CodiForm_Move;
        }
        //***********************************//
        private void CodiForm_Move(object sender, EventArgs e)
        {
            Settings.CurrentSettings.codiPanelLocation = this.Location;
            Settings.SaveSettings();
        }
        //***********************************//
        public void UpdateWindow()
        {
        }
        //***********************************//
        private void CodiForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        //***********************************//
        private void BankRequestButton_Click(object sender, EventArgs e)
        {
            CodiPaymentForm codiPayForm = new CodiPaymentForm();
            codiPayForm.paymentType = CodiPaymentType.BankAppPayment;
            codiPayForm.Show();
        }
        //***********************************//
        private void WhatsappButton_Click(object sender, EventArgs e)
        {
            CodiPaymentForm codiPayForm = new CodiPaymentForm();
            codiPayForm.paymentType = CodiPaymentType.WhatsappQR;
            codiPayForm.Show();
        }
        //***********************************//
        private void QRButton_Click(object sender, EventArgs e)
        {
            CodiPaymentForm codiPayForm = new CodiPaymentForm();
            codiPayForm.paymentType = CodiPaymentType.ScreenQR;
            codiPayForm.Show();
        }
        //***********************************//
        private void TxnHistoryButton_Click(object sender, EventArgs e)
        {
            TransactionsForm transactionsForm = new TransactionsForm();
            transactionsForm.Show();
        }
        //***********************************//
    }
}
