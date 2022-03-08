using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Data.SQLite;
using Npgsql;

namespace Skyticket
{
    public partial class TransactionsForm : Form
    {
        string language = "";

        private BindingSource bindingSource1 = new BindingSource();
        private NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter();

        string ticketsQuery = "SELECT ID, idcobro, idcomercio, idcodi, \"idCuentaBancaria\", fecha, phone, status, qrimage, monto FROM public.ct_solicitudesdepago";

        public TransactionsForm()
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
        private void TransactionsForm_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;

#if DEBUG
            this.TopMost = false;
#endif
            try
            {
                string query = ticketsQuery;
                query += string.Format(" WHERE id_cliente={0} AND id_terminal={1} ORDER BY fecha DESC",
                                        Settings.CurrentSettings.ClientID,
                                        Settings.CurrentSettings.TerminalID);
                
                this.Invoke(new Action(() =>
                {
                    LoadDataGridView(query);
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check settings\n\r" + ex.Message);
            }
        }
        //***********************************//
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                string query = ticketsQuery;
                query += string.Format(" WHERE id_cliente={0} AND id_terminal={1} ORDER BY fecha DESC",
                                        Settings.CurrentSettings.ClientID,
                                        Settings.CurrentSettings.TerminalID);

                lock (DBProvider.remoteDBLock)
                    this.Invoke(new Action(() =>
                    {
                        LoadDataGridView(query);
                    }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check settings\n\r" + ex.Message);
            }
        }
        //***********************************//
        private void LoadDataGridView(string DBQuery)
        {
            try
            {
                dataGridView1.RowTemplate.Height = 35;
                dataGridView1.DataSource = bindingSource1;

                dataAdapter = new NpgsqlDataAdapter(DBQuery, DBProvider.remoteConnection);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                NpgsqlCommandBuilder commandBuilder = new NpgsqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable table = new DataTable();
                table.Locale = CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;

                //dataGridView1.Columns["DateSent"].Visible = false;
                if (language.ToLower().Contains("es"))
                {
                    //dataGridView1.Columns["TicketImage"].HeaderText = "Ticket";
                    //dataGridView1.Columns["PrintMethod"].HeaderText = "Método Envio";
                    //dataGridView1.Columns["MobilePhone"].HeaderText = "Número de Teléfono";
                    //dataGridView1.Columns["Date"].HeaderText = "Fecha de Envio";
                    //dataGridView1.Columns["Time"].HeaderText = "Hora de Envio";
                }

                //dataGridView1.Columns["TicketImage"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                //dataGridView1.Columns["PrintMethod"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                //dataGridView1.Columns["MobilePhone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                //dataGridView1.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns["id"].Visible = false;
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns["qrimage"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                for (int i = 0; i < dataGridView1.RowCount; i++)
                    dataGridView1.Rows[i].Height = 40;

                dataGridView1.Refresh();
                statusLabel.Text = String.Format("Found {0} records", dataGridView1.RowCount);
                
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //***********************************//
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //LoadQRPreview();
        }
        //***********************************//
        private void LoadQRPreview()
        {
            //pictureBox1.Image = null;
            //if (dataGridView1.SelectedRows != null)
            //{
            //    if (dataGridView1.SelectedRows.Count > 0)
            //    {
            //        string ticketImage = dataGridView1.SelectedRows[0].Cells["ticketImage"].Value.ToString();
            //        string picturePath = Path.Combine(Settings.CurrentSettings.TicketsFolder, ticketImage);

            //        pictureBox1.Tag = "";
            //        if (File.Exists(picturePath))
            //        {
            //            pictureBox1.Image = Image.FromFile(picturePath);
            //            pictureBox1.Tag = picturePath;
            //        }

            //    }
            //}
            //System.Diagnostics.Process.Start(pictureBox1.Tag.ToString());
        }

        
    }
}
