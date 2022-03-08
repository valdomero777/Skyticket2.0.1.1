using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Npgsql;

using System.Data.SQLite;

namespace Skyticket
{
    public class DBProvider
    {
        public static object remoteDBLock = new object();
        public static NpgsqlConnection remoteConnection;

        public static object localDBLock = new object();
        public static SQLiteConnection localConnection;

        public static void InitRemoteDB()
        {
            try
            {
                remoteConnection = new NpgsqlConnection(Settings.DBString);
                remoteConnection.Open();
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("InitRemoteDB() " + ex.Message);
            }
            Console.WriteLine("remote DB Connection is: " + remoteConnection.State.ToString());
        }


        public static void InitLocalDB()
        {
            try
            {
                string DBPath = Path.Combine(Settings.ConfigDirectory, "db.s3db");
                string localDBString = "Data Source={0};Version=3;Pooling=true;FailIfMissing=true";
                localDBString = string.Format(localDBString, DBPath);
                localConnection = new SQLiteConnection(localDBString);
                localConnection.Open();
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("InitLocalDB() " + ex.Message);
            }
            Console.WriteLine("new local DB Connection is: " + localConnection.State.ToString());
        }

        public static int GetLastTicketID()
        {
            int ticketID = -1;

            #region get contactInfo
            try
            {
                string mainQuery = "SELECT max(id) FROM public.tickets";

                mainQuery += " WHERE \"id_client\"=@id_client AND \"id_terminal\"=@id_terminal";

                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand codiCmd = new NpgsqlCommand())
                    {
                        codiCmd.CommandType = System.Data.CommandType.Text;
                        codiCmd.Connection = DBProvider.remoteConnection;
                        codiCmd.CommandText = mainQuery;
                        codiCmd.Parameters.AddWithValue("@id_client", Convert.ToInt32(Settings.CurrentSettings.ClientID));
                        codiCmd.Parameters.AddWithValue("@id_terminal", Convert.ToInt32(Settings.CurrentSettings.TerminalID));

                        using (NpgsqlDataReader reader = codiCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                if (reader.Read())
                                {
                                    ticketID = reader.GetInt32(0);
                                }
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("in GetLastTicketID() " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
            }
            #endregion

            return ticketID;
        }
    }
}
