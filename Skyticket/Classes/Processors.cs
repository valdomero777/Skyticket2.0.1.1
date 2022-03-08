using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Skyticket
{
    public class Processors
    {
        public static void CleanOldJobs()
        {
            try
            {
                string[] files = Directory.GetFiles(Settings.processedDirectory);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.CreationTime < DateTime.Now.AddDays(-7))
                        fi.Delete();
                }
            }
            catch (Exception)
            {
            }

            try
            {
                string[] files = Directory.GetFiles(Settings.CurrentSettings.OutputPath);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.CreationTime < DateTime.Now.AddDays(-7))
                        fi.Delete();
                }
            }
            catch (Exception)
            {
            }

        }
    }
}
