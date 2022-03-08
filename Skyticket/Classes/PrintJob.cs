using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Skyticket
{
    public class PrintJob
    {
        public int id { get; set; }
        public string id_terminal { get; set; }
        public string id_client { get; set; }
        public string ticketImage { get; set; }
        public string jobFileName { get; set; }
        public string printMethod { get; set; }
        public string email { get; set; }
        public string mobilePhone { get; set; }

        public static List<PrintJob> LoadPrintJobs()
        {
            List<PrintJob> unsentJobs = new List<PrintJob>();

            #region get unsentJobs
            try
            {
                string mainQuery = "SELECT id, id_terminal, id_client, ticketImage, jobFileName, printMethod, email, mobilePhone FROM printJobs WHERE sent=0";

                lock (DBProvider.localDBLock)
                {
                    using (SQLiteCommand jobsCmd = new SQLiteCommand(mainQuery, DBProvider.localConnection))
                    using (SQLiteDataReader reader = jobsCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    PrintJob job = new PrintJob();
                                    job.id = reader.GetInt32(0);
                                    job.id_terminal = reader.GetString(1);
                                    job.id_client = reader.GetString(2);
                                    job.ticketImage = reader.GetString(3);
                                    job.jobFileName = reader.GetString(4);
                                    job.printMethod = reader.GetString(5);
                                    job.email = reader.GetString(6);
                                    job.mobilePhone = reader.GetString(7);
                                    unsentJobs.Add(job);
                                }
                                catch (Exception ex)
                                {
                                    MainForm.UpdateLogBox("in LoadPrintJobs(): " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("in LoadPrintJobs()2 " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
            }
            #endregion

            return unsentJobs;
        }

    }
}
