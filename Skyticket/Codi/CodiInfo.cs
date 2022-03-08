using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyticket
{
    public class CodiInfo
    {
        public string codiurlws { get; set; }
        public string user_codi { get; set; }
        public string pass_codi { get; set; }
        public string idComercio { get; set; }
        public string idCuentaBancaria { get; set; }
        public string referencia { get; set; }

        public string user_body { get; set; }
        public string password_body { get; set; }
        public string grant_body { get; set; }


        public static CodiInfo LoadCodiInfo()
        {
            CodiInfo codiInfo = new CodiInfo();

            #region get contactInfo
            try
            {
                string mainQuery = "SELECT codiurlws,user_codi,pass_codi,\"idComercio\",\"idCuentaBancaria\",referencia, user_body, pass_body, grant_body FROM vista_terminales_datos";

                mainQuery += " WHERE id_cliente=@id_cliente AND idterminal=@idterminal";

                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand codiCmd = new NpgsqlCommand())
                    {
                        codiCmd.CommandType = CommandType.Text;
                        codiCmd.Connection = DBProvider.remoteConnection;
                        codiCmd.CommandText = mainQuery;
                        codiCmd.Parameters.AddWithValue("@id_cliente", Convert.ToInt32(Settings.CurrentSettings.ClientID));
                        codiCmd.Parameters.AddWithValue("@idterminal", Convert.ToInt32(Settings.CurrentSettings.TerminalID));

                        using (NpgsqlDataReader reader = codiCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    try
                                    {
                                        codiInfo.codiurlws = reader.GetString(0);
                                        codiInfo.user_codi = reader.GetString(1);
                                        codiInfo.pass_codi = reader.GetString(2);
                                        codiInfo.idComercio = reader.GetString(3);
                                        //codiInfo.idComercio = "00000938519479230872";
                                        codiInfo.idCuentaBancaria = reader.GetString(4);
                                        //codiInfo.idCuentaBancaria = "RzESjxftQCXitFuFp152";
                                        codiInfo.referencia = reader.GetString(5);
                                        codiInfo.user_body = reader.GetString(6);
                                        codiInfo.password_body = reader.GetString(7);
                                        codiInfo.grant_body = reader.GetString(8);
                                    }
                                    catch (Exception ex)
                                    {
                                        MainForm.UpdateLogBox("in LoadCodiInfo(): " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
                                    }
                                }
                            }
                            else
                                Settings.CurrentSettings.CodiEnabled = false;
                        }
                    }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("in LoadCodiInfo()2 " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
            }
            #endregion

            return codiInfo;
        }


    }
}
