using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyticket
{
    public class TerminalInfo
    {
        public string nombreSucursal { get; set; }
        public string nombreTerminal { get; set; }
        public bool Estado { get; set; }
        public int idsucursal { get; set;  }
        public int idterminal { get; set; }
        public int id_cliente { get; set; }
        //***********************************//
        public static List<TerminalInfo> LoadTerminals(string licenseKey)
        {
            List<TerminalInfo> infos = new List<TerminalInfo>();

            #region get Terminals
            try
            {
                string mainQuery = @"
                                    SELECT
                                    nombresucursal,
                                    nombreterminal,
                                    estado,
                                    idsucursal,
                                    idterminal,
                                    id_cliente
                                FROM
                                    vista_licencias_terminal
                                WHERE
                                    keylicencia=@keylicencia
                                ORDER BY
                                    idterminal;";

                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand codiCmd = new NpgsqlCommand())
                    {
                        codiCmd.CommandType = CommandType.Text;
                        codiCmd.Connection = DBProvider.remoteConnection;
                        codiCmd.CommandText = mainQuery;
                        codiCmd.Parameters.AddWithValue("@keylicencia", licenseKey);
                        //codiCmd.Parameters.AddWithValue("@idterminal", Convert.ToInt32(Settings.CurrentSettings.TerminalID));

                        using (NpgsqlDataReader reader = codiCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    try
                                    {
                                        TerminalInfo info = new TerminalInfo();

                                        {
                                            info.nombreSucursal = reader.GetString(0);
                                            info.nombreTerminal = reader.GetString(1);
                                            info.Estado = reader.GetBoolean(2);
                                            info.idsucursal = reader.GetInt32(3);
                                            info.idterminal = reader.GetInt32(4);
                                            info.id_cliente = reader.GetInt32(5);
                                            infos.Add(info);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("in LoadTerminals(): " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
                                    }
                                }
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("remote DB Connection is: " + remoteConnection.State.ToString());
                Console.WriteLine("in LoadTerminals()2 " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
            }
            #endregion

            return infos;
        }
        //***********************************//

    }
}
