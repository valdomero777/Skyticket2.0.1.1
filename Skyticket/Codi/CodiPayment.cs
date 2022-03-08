using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyticket
{
    /// <summary>
    /// this is used for saving codi payment info to DB table ct_solicitudesdepago
    /// </summary>
    public class CodiPayment
    {
        public string idcobro { get; set; }
        public string idcodi { get; set; }
        public string qrimage { get; set; }
        public double monto { get; set; }
        public string phone { get; set; }
        public CodiPaymentType paymentType { get; set; }
        public string status { get; set; }

        static System.Timers.Timer StatusTimer = new System.Timers.Timer();

        public CodiPayment()
        {
            idcobro = "";
            idcodi = "";
        }
        //***********************************//
        public static void StartStatusThread()
        {
            StatusTimer.Interval = Settings.CurrentSettings.CodiProcTimer * 1000;
            StatusTimer.Elapsed += StatusTimer_Elapsed;
            StatusTimer.Enabled = true;
        }
        //***********************************//
        private static void StatusTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StatusTimer.Enabled = false;
            try
            {
                string tokenMessage = "";
                if (string.IsNullOrEmpty(CodiAPI.token))
                    CodiAPI.GetAccessToken(out tokenMessage);

                List<CodiPayment> codiPayments = GetPendingPayments();

                foreach (CodiPayment codiPayment in codiPayments)
                {
                    string status = "";
                    string idcodi = "";

                    bool result = CodiAPI.GetPaymentStatus(codiPayment.idcobro, codiPayment.paymentType, out status, out idcodi);

                    if (result)
                    {
                        CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, idcodi, status);

                        //if (status == "-1")
                        //    CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, idcodi, "Pending");
                        //else if (status == "0")
                        //    CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, idcodi, "Accepted");
                        //else if (status == "1")
                        //    CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, idcodi, "Paid");
                        //else if (status == "2")
                        //    CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, idcodi, "Rejected");
                        //else if (status == "3")
                        //    CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, idcodi, "Canceled");
                        //else if (status == "4")
                        //    CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, idcodi, "Postponed");
                        //else if (status == "5")
                        //    CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, idcodi, "Sent");
                        //else
                        //    CodiPayment.UpdatePaymentStatus(codiPayment.idcobro, idcodi, status);
                    }
                }

            }
            catch (Exception)
            {
            }

            try
            {
                StatusTimer.Interval = Settings.CurrentSettings.CodiProcTimer * 1000;
                StatusTimer.Enabled = true;
            }
            catch (Exception)
            {
            }
        }
        //***********************************//
        public bool SavePayment()
        {
            bool result = false;
            try
            {
                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand saveCmd = new NpgsqlCommand())
                    {
                        saveCmd.CommandType = CommandType.Text;
                        saveCmd.Connection = DBProvider.remoteConnection;

                        string query = "INSERT INTO public.ct_solicitudesdepago(" +
                                        "id_terminal, id_cliente, idcobro, idcomercio, idcodi, \"idCuentaBancaria\", fecha, phone, status, qrimage, monto, \"paymentType\")" +
                                        "VALUES(@id_terminal, @id_client, @idcobro, @idcomercio, @idcodi, @idCuentaBancaria, @fecha, @phone, @status, @qrimage, @monto, @paymentType)";

                        saveCmd.CommandText = query;
                        saveCmd.Parameters.AddWithValue("@id_terminal", Convert.ToInt32(Settings.CurrentSettings.TerminalID));
                        saveCmd.Parameters.AddWithValue("@id_client", Convert.ToInt32(Settings.CurrentSettings.ClientID));
                        saveCmd.Parameters.AddWithValue("@idcobro", idcobro);
                        saveCmd.Parameters.AddWithValue("@idcomercio", CodiAPI.codiInfo.idComercio);
                        saveCmd.Parameters.AddWithValue("@idcodi", idcodi);
                        saveCmd.Parameters.AddWithValue("@idCuentaBancaria", CodiAPI.codiInfo.idCuentaBancaria);
                        saveCmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                        saveCmd.Parameters.AddWithValue("@phone", phone);
                        saveCmd.Parameters.AddWithValue("@status", status);
                        saveCmd.Parameters.AddWithValue("@qrimage", qrimage);
                        saveCmd.Parameters.AddWithValue("@monto", monto);
                        saveCmd.Parameters.AddWithValue("@paymentType", (int)paymentType);

                        int temp = saveCmd.ExecuteNonQuery();
                        result = true;
                    }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("CodiPayment.1: " + ex.Message);

                if (DBProvider.remoteConnection.State != ConnectionState.Open)
                    DBProvider.InitRemoteDB();
            }

            return result;
        }
        //***********************************//
        public static List<CodiPayment> GetPendingPayments()
        {
            List<CodiPayment> pendingPayments = new List<CodiPayment>();
            try
            {
                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand saveCmd = new NpgsqlCommand())
                    {
                        saveCmd.CommandType = CommandType.Text;
                        saveCmd.Connection = DBProvider.remoteConnection;

                        string mainQuery = "SELECT idcobro, \"paymentType\" FROM public.ct_solicitudesdepago";
                        mainQuery += " WHERE status IN ('Creado','Enviado','Pending','Accepted') AND id_cliente=@id_cliente AND id_terminal=@id_terminal";

                        saveCmd.CommandText = mainQuery;
                        saveCmd.Parameters.AddWithValue("@id_terminal", Convert.ToInt32(Settings.CurrentSettings.TerminalID));
                        saveCmd.Parameters.AddWithValue("@id_cliente", Convert.ToInt32(Settings.CurrentSettings.ClientID));
                        //saveCmd.Parameters.AddWithValue("@status", "Pending");

                        using (NpgsqlDataReader reader = saveCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (!reader.IsDBNull(0))
                                    {
                                        CodiPayment codiPayment = new CodiPayment();
                                        codiPayment.idcobro = reader.GetString(0);
                                        if (!reader.IsDBNull(1))
                                        {
                                            codiPayment.paymentType = (CodiPaymentType)reader.GetInt32(1);
                                            pendingPayments.Add(codiPayment);
                                        }
                                    }
                                }
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("CodiPayment.2: " + ex.Message);

                if (DBProvider.remoteConnection.State != ConnectionState.Open)
                    DBProvider.InitRemoteDB();
            }

            return pendingPayments;
        }
        //***********************************//
        public static bool UpdatePaymentStatus(string idcobro, string idcodi, string status)
        {
            bool retVal = false;
            #region get contactInfo
            try
            {
                string mainQuery = "UPDATE public.ct_solicitudesdepago SET status=@status, idcodi=@idcodi";
                mainQuery += " WHERE idcobro=@idcobro AND id_cliente=@id_cliente AND id_terminal=@id_terminal";

                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand codiCmd = new NpgsqlCommand())
                    {
                        codiCmd.CommandType = CommandType.Text;
                        codiCmd.Connection = DBProvider.remoteConnection;
                        codiCmd.CommandText = mainQuery;
                        codiCmd.Parameters.AddWithValue("@idcobro", idcobro);
                        codiCmd.Parameters.AddWithValue("@idcodi", idcodi);
                        codiCmd.Parameters.AddWithValue("@id_cliente", Convert.ToInt32(Settings.CurrentSettings.ClientID));
                        codiCmd.Parameters.AddWithValue("@id_terminal", Convert.ToInt32(Settings.CurrentSettings.TerminalID));
                        codiCmd.Parameters.AddWithValue("@status", status);
                        codiCmd.ExecuteNonQuery();
                    }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("in CodiPayment.3 " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);

                if (DBProvider.remoteConnection.State != ConnectionState.Open)
                    DBProvider.InitRemoteDB();
            }
            #endregion

            return retVal;
        }
    }
}
