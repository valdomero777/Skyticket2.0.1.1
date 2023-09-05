
using System.Drawing;
using StarMicronics.StarIOExtension;
using System.Windows.Forms;
using Skyticket.Classes;
using System;

namespace Skyticket
{
    public class PrintStarHelper
    {

        public static byte[] PrintImage(Emulation emulation, string filePath, int paperSize)
        {
            try
            {


                ICommandBuilder builder = StarIoExt.CreateCommandBuilder(emulation);

                builder.BeginDocument();

                Bitmap rasterImage = (Bitmap)Image.FromFile(filePath);

                builder.AppendBitmap(rasterImage, true, paperSize, true);

                builder.AppendCutPaper(CutPaperAction.PartialCutWithFeed);

                builder.EndDocument();

                return builder.Commands;
            }catch(Exception ex)
            {
                MainForm.UpdateLog(ex.Message);
                MainForm.UpdateLogBox(ex.Message);
                return null;

            }
        }

        public static void Print(byte[] commands)
        {
            try
            {

                MainForm.UpdateLog("imprimiendo");
                MainForm.UpdateLogBox("imprimiendo");
                // Your printer PortName and PortSettings.
                string portName = $"LTP1:{Settings.CurrentSettings.PrinterName}";
                
                string portSettings = "";


                // Sending commands to printer sample is "Communication.SendCommands(byte[] commands, string portName, string portSettings, int timeout)".
                Communication.SendCommandsWithProgressBar(commands, portName, portSettings, 30000);
            }catch(Exception ex)
            {
                MainForm.UpdateLog(ex.Message);
                MainForm.UpdateLogBox(ex.Message);
                
            }
        }
    }

    
}
