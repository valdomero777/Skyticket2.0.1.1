using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skyticket
{
    internal class FTP
    {
        internal static bool FTPUpload(string fileName, byte[] FileBytes)
        {
            bool result = false;
            try
            {
                // Get the object used to communicate with the server.
                string DestinationPath = Settings.CurrentSettings.FTPServer + ":" + Settings.CurrentSettings.FTPPort.ToString() +
                                        "/" + Settings.CurrentSettings.FTPTicketsFolder + "/" +
                                        Path.GetFileName(fileName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(DestinationPath);

                request.Credentials = new NetworkCredential(Settings.CurrentSettings.FTPUser, Settings.CurrentSettings.FTPPassword);

                request.Timeout = 900000;
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.ContentLength = FileBytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {

                    for (int i = 0; i < FileBytes.Length; i += (1024 * 1024))
                    {
                        if (FileBytes.Length - i >= (1024 * 1024))
                            requestStream.Write(FileBytes, i, (1024 * 1024));
                        else
                            requestStream.Write(FileBytes, i, FileBytes.Length - i);
                    }
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                MainForm.UpdateLogBox("FTP: " + response.StatusDescription);
                if (response.StatusDescription.ToLower().Contains("successfully transferred"))
                    result = true;
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("FTPUpload(): " + ex.Message);
            }

            return result;
        }

        internal static bool FTPDownload(string sourcePath, string destinationFile)
        {
            bool result = false;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sourcePath);

                request.Credentials = new NetworkCredential(Settings.CurrentSettings.FTPUser, Settings.CurrentSettings.FTPPassword);

                request.Timeout = 900000;
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();


                using (Stream ftpStream = response.GetResponseStream())
                using (Stream fileStream = File.Create(destinationFile))
                {
                    ftpStream.CopyTo(fileStream);
                }

                MainForm.UpdateLogBox("FTP Download: " + response.StatusDescription);
                if (response.StatusDescription.ToLower().Contains("successfully transferred"))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                MainForm.UpdateLogBox("FTPDownload(): " + ex.Message);
            }

            return result;
        }
    }
}
