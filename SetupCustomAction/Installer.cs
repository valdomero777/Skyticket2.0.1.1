using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;

using System.Drawing.Printing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
//using System.Windows.Forms;

namespace Skyticket
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Program Files\\Skyticket\\LicenseValidator.exe");
            Process p = Process.Start(startInfo);
            p.WaitForExit();// Have to hold the setup until the application is closed.

            //Application.EnableVisualStyles();
            //LicenseForm licenseForm = new LicenseForm();
            if (p.ExitCode != 987)
            {
                stateSaver.Clear();
                throw new Exception("El número de licencia no es válido. Por favor verifíquelo o contacte al equipo de soporte" + Environment.NewLine +
                                    "The license number is not valid. Please verify it or contact support");

            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start("Taskkill", "/IM Skyticket.exe /F");


                    AutorunHelper.AddToStartup();

                    base.Install(stateSaver);
                    LogHelper.Log("Install Started.");
                }
                catch (Exception ex)
                {
                    LogError(ex.StackTrace, ex.Message, ex.InnerException);
                    //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }

                try
                {
                    //PrinterInstaller installer = new PrinterInstaller();
                    //installer.UnInstallPrinter();
                    //bool resultPort = installer.CreatePrinterPort();

                    SpoolerHelper sh = new SpoolerHelper();
                    SpoolerHelper.GenericResult result = sh.AddVPrinter("YupioReceiptC", "YupioReceiptC");
                    if (result.Success == false)
                    {
                        LogError(result.Method, result.Message, result.Exception);
                        //throw new InstallException(string.Format("Source: {0}\nMessage: {1}", result.Method, result.Message), result.Exception);
                    }

                    //installer.MakeDefaultPrinter("POSVirtual");
                    //PaperSizeHelper.AddCustomPaperSize("Print.sa", "PPTtoPDF", 154.0f, 280.0f);
                }
                catch (Exception ex)
                {
                    LogError("AddVPrinter", ex.Message, ex);
                }

                //try
                //{
                //    PrinterInstaller installer = new PrinterInstaller();
                //    installer.InstallPrinterWMI();
                //    //bool resultPort = installer.CreatePrinterPort();
                //}
                //catch (Exception ex)
                //{
                //    LogError("AddVPrinter", ex.Message, ex);
                //}

                {
                    var process = new Process();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Verb = "runas";
                    process.StartInfo.Arguments = "/c bcdedit /set testsigning on";
                    process.Start();
                    process.WaitForExit();
                    process.Dispose();
                }

               // LaunchHelper.RunAsDesktopUser("C:\\Program Files\\Skyticket\\SetupSerial.exe");
                LaunchHelper.RunAsDesktopUser("C:\\Program Files\\Skyticket\\Skyticket.exe");
                //if (IntPtr.Size == 8)
                //    LaunchHelper.RunAsDesktopUser("C:\\Program Files\\Skyticket\\Skyticket.exe");
                //else
                //{
                //    LaunchHelper.RunAsDesktopUser("C:\\Program Files (x86)\\Skyticket\\SetupSerial.exe");
                //    LaunchHelper.RunAsDesktopUser("C:\\Program Files (x86)\\Skyticket\\Skyticket.exe");
                //}


                LogHelper.Log("Install Finished.");
            }
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            try
            {
                base.OnBeforeInstall(savedState);
                //MessageBox.Show("OnBeforeInstall");
                //Application.EnableVisualStyles();
                //LicenseForm licenseForm = new LicenseForm();
                //licenseForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UnInstallSerialPort()
        {
            string currentFolder = "";
            try
            {
                Process proc = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                currentFolder = "C:\\Program Files\\Skyticket\\serial";

                startInfo.WorkingDirectory = currentFolder;
                startInfo.FileName = "setupc.exe";
                startInfo.Arguments = "remove 0";
                proc.StartInfo = startInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message + currentFolder);
            }
        }

        public override void Uninstall(IDictionary savedState)
        {
            try
            {
                PrinterInstaller installer = new PrinterInstaller();
                installer.UnInstallPrinter("Skyticket");
                installer.UnInstallPrinter("SkyticketGen");
                AutorunHelper.RemoveFromStartup();

                System.Diagnostics.Process.Start("Taskkill", "/IM Skyticket.exe /F");
            }
            catch (Exception)// ex)
            {
                //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            try
            {
                UnInstallSerialPort();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            //MessageBox.Show("taskKill called");
            base.Uninstall(savedState);
        }

        private static void LogError(string exceptionSource, string message, Exception innerException)
        {
            string eventMessage = string.Format("Source: {0}\nMessage: {1}\nInnerException: {2}", exceptionSource, message, innerException);
            LogHelper.Log(eventMessage);
        }
    }
}
