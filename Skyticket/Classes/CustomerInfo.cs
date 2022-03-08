using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyticket
{
    public class CustomerInfo
    {
        public static List<string> LoadCustomerInfo()
        {
            List<string> contactInfo = new List<string>();

            #region get contactInfo
            try
            {
                string mainQuery = "SELECT contactInfo FROM customerInfo";

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
                                    string contact = reader.GetString(0);
                                    contactInfo.Add(contact);
                                }
                                catch (Exception ex)
                                {
                                    MainForm.UpdateLogBox("in LoadCustomerInfo(): " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("in LoadCustomerInfo()2 " + ex.Message + " **** " + ex.StackTrace + " **** " + ex.InnerException);
            }
            #endregion

            return contactInfo;
        }

        public static void SaveCustomerInfo(string target)
        {
            try
            {
                lock (DBProvider.localDBLock)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = DBProvider.localConnection;
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = "INSERT INTO customerInfo (contactInfo)" +
                            " VALUES (@contactInfo)";

                        cmd.Parameters.AddWithValue("@contactInfo", target);
                        int count = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("unique"))
                    return;
                MainForm.UpdateLogBox("SaveCustomerInfo(): " + ex.Message);
            }
        }
    }
}
