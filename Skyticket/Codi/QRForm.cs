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
    public partial class QRForm : Form
    {
        string language = "";

        public string qrString = "";
        public QRForm()
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
        private void QRForm_Load(object sender, EventArgs e)
        {
            this.Location = Settings.CurrentSettings.codiQRLocation;
            this.Size = Settings.CurrentSettings.codiQRSize;

            this.Move += QRForm_Move;
            this.Resize += QRForm_Resize;

            byte[] base64Bytes = Convert.FromBase64String(qrString);
            var ms = new MemoryStream(base64Bytes);
            var imageBitmap = new Bitmap(ms);
            this.Invoke(new Action(() =>
            {
                pictureBox1.Image = MainForm.ImageTrimWhite(imageBitmap);
            }));
        }
        //***********************************//
        private void QRForm_Move(object sender, EventArgs e)
        {
            Settings.CurrentSettings.codiQRLocation = this.Location;
            Settings.SaveSettings();
        }
        //***********************************//
        private void QRForm_Resize(object sender, EventArgs e)
        {
            Settings.CurrentSettings.codiQRSize = this.Size;
            Settings.SaveSettings();
        }
        //***********************************//
        private void CodiPaymentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        //***********************************//
    }
}
