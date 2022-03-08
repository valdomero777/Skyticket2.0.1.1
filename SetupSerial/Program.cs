using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SetupSerial
{
    class Program
    {
        static void Main(string[] args)
        {
            InstallSerialPort();
        }

        private static void UnInstallSerialPort()
        {
            string currentFolder = "";
            try
            {
                Process proc = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                currentFolder = "C:\\Program Files (x86)\\Skyticket\\serial";

                startInfo.WorkingDirectory = currentFolder;
                startInfo.FileName = "setupc.exe";
                startInfo.Arguments = "remove 0";
                proc.StartInfo = startInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception)
            {

            }
        }

        private static void InstallSerialPort()
        {
            string currentFolder = "";

            try
            {
                //bcdedit /set testsigning on

                Process proc = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                currentFolder = "C:\\Program Files\\Skyticket\\serial";

                UnInstallSerialPort();

                proc = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = currentFolder;
                startInfo.FileName = "setupc.exe";
                startInfo.Arguments = "install Portname=COM#,EmuBR=yes,EmuOverrun=yes Portname=COM#,EmuBR=yes,EmuOverrun=yes";
                proc.StartInfo = startInfo;
                proc.Start();
                proc.WaitForExit();

                {
                    var process = new Process();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.FileName = "cmd.exe";

                    process.StartInfo.Arguments = "/c C:\\Windows\\System32\\InfDefaultInstall.exe \"" + currentFolder + "\\comport.inf\"";
                    process.Start();
                    process.WaitForExit();
                    process.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
