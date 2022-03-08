using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Skyticket
{
    internal class Updater
    {
        internal static DialogResult CheckUpdate()
        {
            //get current version:
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;

                var platForm = IntPtr.Size;


                string FTPdirectory = Settings.CurrentSettings.FTPServer + ":" + Settings.CurrentSettings.FTPPort.ToString() +
                                        "/";

                if (platForm == 4)
                    FTPdirectory += "skyticketupdatex86" + "/";
                else if (platForm == 8)
                    FTPdirectory += "skyticketupdate" + "/";

                //check version info file on web server
                string versionSourcePath = FTPdirectory + "versionInfo.xml";
                string versionFile = Path.Combine(Settings.ConfigDirectory, "versionInfo.xml");
                if (File.Exists(versionFile))
                    File.Delete(versionFile);
                if (FTP.FTPDownload(versionSourcePath, versionFile))
                {
                    XElement xmlDoc = XElement.Load(versionFile);
                    string newVersion = xmlDoc.Element("version").Value;

                    //if not equal, rename current file to FileName.bak
                    if (newVersion == version)
                    {
                        MessageBox.Show("Application is up-to-date");
                    }
                    else
                    {
                        string message = Texts.UpdateApp;
                        if (Program.language.Contains("es"))
                            message = TextsSpanish.UpdateApp;


                        DialogResult dialogResult = MessageBox.Show(message, "UPDATE AVAILABLE", MessageBoxButtons.YesNo);

                        return dialogResult;

                    }
                }

            return DialogResult.No;
        }

        internal static void UpdateSelf()
        {
            string originFilePath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "");
            string originFileName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", ""));

            string FTPdirectory = Settings.CurrentSettings.FTPServer + ":" + Settings.CurrentSettings.FTPPort.ToString() +
                                "/";

            var platForm = IntPtr.Size;
            if (platForm == 4)
                FTPdirectory += "skyticketupdatex86" + "/";
            else if (platForm == 8)
                FTPdirectory += "skyticketupdate" + "/";

            if (File.Exists(originFilePath + ".bak"))
                File.Delete(originFilePath + ".bak");
            File.Move(originFilePath, originFilePath + ".bak");
            //download new file

            FTP.FTPDownload(FTPdirectory + originFileName, originFilePath);

            #region download language resources
            foreach (string languageRes in Program.languagesPath)
            {
                string languageResDir = Path.GetDirectoryName(originFilePath);
                languageResDir = Path.Combine(languageResDir, languageRes);
                string[] resourceFiles = System.IO.Directory.GetFiles(languageRes);
                string languageFileName = "";
                foreach (string resFilePath in resourceFiles)
                {
                    if (!resFilePath.EndsWith(".bak"))
                    {
                        if (File.Exists(resFilePath + ".bak"))
                            File.Delete(resFilePath + ".bak");
                        File.Move(resFilePath, resFilePath + ".bak");

                        languageFileName = Path.GetFileName(resFilePath);

                        string destinationFile = Path.Combine(languageResDir, languageFileName);
                        FTP.FTPDownload(FTPdirectory + languageRes + "/" + originFileName, destinationFile);
                    }
                }
            }
            #endregion
        }

        public static void ExecuteAsAdmin(string fileName, string argument = "")
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = true;
            //process.StartInfo.CreateNoWindow = false;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = fileName;
            process.StartInfo.Verb = "runas";
            process.StartInfo.Arguments = argument;
            //process.StartInfo.UserName = "Administrator";
            process.Start();
            //process.WaitForExit();
            //process.Dispose();
        }

        public static void ExecuteSelf(string fileName, string argument = "")
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = true;
            //process.StartInfo.CreateNoWindow = false;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = argument;
            //process.StartInfo.UserName = "Administrator";
            process.Start();
            //process.WaitForExit();
            //process.Dispose();
        }
    }
}
