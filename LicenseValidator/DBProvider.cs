using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;


namespace Skyticket
{
    public class DBProvider
    {
        public static object remoteDBLock = new object();
        public static NpgsqlConnection remoteConnection;

        //***********************************//
        public static void InitRemoteDB()
        {
            try
            {
                string DBString = @"Host={0};Username={1};Password={2};Database={3}";
                DBString = string.Format(DBString,
                                        "skyticketdelivery.com",
                                        "postgres",
                                        "Webmaster3d",
                                        "virtualprinter");
                remoteConnection = new NpgsqlConnection(DBString);
                remoteConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("InitRemoteDB: " + ex.Message);
                //MainForm.UpdateLogBox("InitRemoteDB() " + ex.Message);
            }

            Console.WriteLine("remote DB Connection is: " + remoteConnection.State.ToString());
        }
        //***********************************//
    }
}
