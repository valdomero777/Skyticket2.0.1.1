using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Drawing;

namespace Skyticket
{
    public class PrintHelper
    {
        public static string _newline = "\r";
        public static byte[] initBytes = new byte[] { 0x1B, (byte)'@' };
        public static byte[] cutBytes = new byte[] { 0x1D, 0x56, 0x42, 0x00 };
        public static byte[] AlignLeft = new byte[] { 0x1b, 0x61, 0x00 };
        public static byte[] AlignCenter = new byte[] { 0x1b, 0x61, 0x01 };
        public static byte[] AlignRight = new byte[] { 0x1b, 0x61, 0x02 };

        public static byte[] RegularText = new byte[] { 0x1b, 0x21, 0x00 };
        public static byte[] LargeText = new byte[] { 0x1b, 0x21, 0x20 };

        static string receiptImagePath = "";
        public static void Print(string pngFilePath, int receiptHeight)
        {
            Bitmap bitmap = ConvertToBitmap(pngFilePath);
            List<byte> finalBytes = new List<byte>();

            finalBytes.AddRange(PrintHelper.initBytes);
            finalBytes.AddRange(PrintHelper.AlignCenter);

            finalBytes.AddRange(GetImageBytes(bitmap));
            finalBytes.Add(0x0A);
            finalBytes.Add(0x0A);
            //finalBytes.AddRange(PrintHelper.cutBytes);

            if (Settings.CurrentSettings.ConnectionType == ConnectionTypes.Network)
            {
                //Task.Factory.StartNew(() =>
                {
                    SendToNetworkPrinter(finalBytes.ToArray());
                    Thread.Sleep(1000);
                }
            }
            else if (Settings.CurrentSettings.ConnectionType == ConnectionTypes.WinPrinter)
            {
                //RawPrinter.SendStringToPrinter(Settings.CurrentSettings.PrinterName,
                //                    Encoding.Default.GetString(printData.ToArray()));
                if (Settings.CurrentSettings.DocumentPrinter)
                {
                    receiptImagePath = pngFilePath;
                    PrintDocument pd = new PrintDocument();
                    pd.DefaultPageSettings.PrinterSettings.PrinterName = Settings.CurrentSettings.PrinterName;
                    int heightInches = 100 * receiptHeight / Settings.CurrentSettings.DocumentPrinterDPI;
                    pd.DefaultPageSettings.PaperSize = new PaperSize("custom", 315, heightInches);
                    pd.PrintPage += PrintPage;
                    pd.Print();
                }
                else
                {
                    RawPrinter.SendBytesToPrinter(Settings.CurrentSettings.PrinterName,
                                    finalBytes.ToArray());
                }
            }
        }
        //*******************************//
        public static void Print(byte[] data)
        {
            if (Settings.CurrentSettings.ConnectionType == ConnectionTypes.Network)
            {
                //Task.Factory.StartNew(() =>
                {
                    SendToNetworkPrinter(data);
                    Thread.Sleep(2000);
                }
            }
            else if (Settings.CurrentSettings.ConnectionType == ConnectionTypes.WinPrinter)
            {
                if (Settings.CurrentSettings.DocumentPrinter)
                {
                    //receiptImagePath = pngFilePath;
                    //PrintDocument pd = new PrintDocument();
                    //pd.DefaultPageSettings.PrinterSettings.PrinterName = Settings.CurrentSettings.PrinterName;
                    //int heightInches = 100 * receiptHeight / Settings.CurrentSettings.DocumentPrinterDPI;
                    //pd.DefaultPageSettings.PaperSize = new PaperSize("custom", 315, heightInches);
                    //pd.PrintPage += PrintPage;
                    //pd.Print();
                }
                else
                {
                    RawPrinter.SendBytesToPrinter(Settings.CurrentSettings.PrinterName,
                                    data);
                }
            }
        }
        //*******************************//
        public static void OpenCashDrawer()
        {
            List<byte> finalBytes = new List<byte>();

            string drawerCmd = Settings.CurrentSettings.DrawerCommand;
            drawerCmd = drawerCmd.Replace(",", "");
            drawerCmd = drawerCmd.Replace(" ", "");

            if (!string.IsNullOrEmpty(drawerCmd))
            {
                byte[] drawer = Converters.HexStringToByteArray(drawerCmd);
                finalBytes.AddRange(drawer);
            }

            if (Settings.CurrentSettings.ConnectionType == ConnectionTypes.Network)
            {
                //Task.Factory.StartNew(() =>
                {
                    SendToNetworkPrinter(finalBytes.ToArray());
                }
            }
            else if (Settings.CurrentSettings.ConnectionType == ConnectionTypes.WinPrinter)
            {
                //RawPrinter.SendStringToPrinter(Settings.CurrentSettings.PrinterName,
                //                    Encoding.Default.GetString(printData.ToArray()));
                if (Settings.CurrentSettings.DocumentPrinter)
                {

                }
                else
                {
                    RawPrinter.SendBytesToPrinter(Settings.CurrentSettings.PrinterName,
                                    finalBytes.ToArray());
                }
            }
        }
        //*******************************//
        private static void PrintPage(object o, PrintPageEventArgs e)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromFile(receiptImagePath))
            {
                Point loc = new Point(0, 0);

                int heightInches = 100 * img.Height / Settings.CurrentSettings.DocumentPrinterDPI;
                e.Graphics.DrawImage(img, 0, 0, 315, heightInches);
            }
            receiptImagePath = "";
        }
        //*******************************//
        private static void SendToNetworkPrinter(byte[] data)
        {
            try
            {
                Socket pSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                pSocket.SendTimeout = 1500;
                pSocket.Connect(Settings.CurrentSettings.PrinterIP, 9100);

                pSocket.Send(data);

                pSocket.Shutdown(SocketShutdown.Both);
                pSocket.Close();
            }
            catch (Exception)// ex)
            {
                //UpdateLogBox(String.Format("in SendToRealPrinter(b: {0} **** {1} **** {2}", ex.Message, ex.InnerException, ex.StackTrace));
            }
        }
        //*******************************//
        public static Bitmap ConvertToBitmap(string fileName)
        {
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                Image image = Image.FromStream(bmpStream);

                bitmap = new Bitmap(image);

            }
            return bitmap;
        }
        //*******************************//
        public static byte[] GetImageBytes(Bitmap bitmap)
        {
            //string logo = "";
            BitMapData data = GetBitmapData(bitmap);
            BitArray dots = data.Dots;
            byte[] width = BitConverter.GetBytes(data.Width);

            int offset = 0;
            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream);

            //bw.Write(0x1B);
            //bw.Write(0x40);
            //bw.Write(0x03);

            //bw.Write(0x1B);
            //bw.Write(0x47);
            //bw.Write(0x00);
            //bw.Write(0x03);

            //bw.Write(0x1B);
            //bw.Write(0x61);
            //bw.Write(0x00);

            bw.Write((char)0x1B);
            bw.Write('3');
            bw.Write((byte)24);

            while (offset < data.Height)
            {
                bw.Write((char)0x1B);
                bw.Write('*');         // bit-image mode
                bw.Write((byte)33);    // 24-dot double-density
                bw.Write(width[0]);  // width low byte
                bw.Write(width[1]);  // width high byte

                for (int x = 0; x < data.Width; ++x)
                {
                    for (int k = 0; k < 3; ++k)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; ++b)
                        {
                            int y = (((offset / 8) + k) * 8) + b;
                            // Calculate the location of the pixel we want in the bit array.
                            // It'll be at (y * width) + x.
                            int i = (y * data.Width) + x;

                            // If the image is shorter than 24 dots, pad with zero.
                            bool v = false;
                            if (i < dots.Length)
                            {
                                v = dots[i];
                            }
                            slice |= (byte)((v ? 1 : 0) << (7 - b));
                        }

                        bw.Write(slice);
                    }
                }
                offset += 24;
                bw.Write((char)0x0A);
            }
            // Restore the line spacing to the default of 30 dots.
            bw.Write((char)0x1B);
            bw.Write('3');
            bw.Write((byte)30);
            //bw.Write((char)0x1D);
            //bw.Write((char)0x21);
            //bw.Write((byte)00);

            bw.Flush();
            byte[] bytes = stream.ToArray();
            return bytes;
        }
        //***********************************//
        private static BitMapData GetBitmapData(Bitmap bitmap)
        {
            var threshold = 127;//127
            var index = 0;
            double multiplier = Settings.CurrentSettings.PrinterDots;// 570;// //570 was default// this depends on your printer model. for Beiyang you should use 1000
            double scale = (double)(multiplier / (double)bitmap.Width);
            int xheight = (int)(bitmap.Height * scale);
            int xwidth = (int)(bitmap.Width * scale);
            var dimensions = xwidth * xheight;
            var dots = new BitArray(dimensions);

            for (var y = 0; y < xheight; y++)
            {
                for (var x = 0; x < xwidth; x++)
                {
                    var _x = (int)(x / scale);
                    var _y = (int)(y / scale);
                    var color = bitmap.GetPixel(_x, _y);
                    var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    dots[index] = (luminance < threshold);
                    index++;
                }
            }

            return new BitMapData()
            {
                Dots = dots,
                Height = (int)(bitmap.Height * scale),
                Width = (int)(bitmap.Width * scale)
            };
        }
        //***********************************//
    }

    public class BitMapData
    {
        public BitArray Dots
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }
    }

    #region raw print helper

    public class RawPrinter
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        [DllImport("winspool.drv", EntryPoint = "ReadPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int ReadPrinter(IntPtr hPrinter, out IntPtr pBytes, Int32 dwCount, out Int32 dwNoBytesRead);
        //public static extern bool ReadPrinter(IntPtr hPrinter, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pBytes, Int32 dwCount, ref Int32 dwNReadBytes);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "POS-Receipt";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // Open the file.
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);


            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

        public static bool SendBytesToPrinter(string szPrinterName, byte[] data)
        {
            IntPtr pBytes = Marshal.AllocHGlobal(data.Length);
            Int32 dwCount;

            dwCount = data.Length;

            Marshal.Copy(data, 0, pBytes, data.Length);

            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

        public static bool ReadPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.


            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "Skyticket-Receipt";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);

                        int bytesRead = 0;
                        Int32 maxRead = 256;
                        IntPtr readBytes;
                        var temp = ReadPrinter(hPrinter, out readBytes, maxRead, out bytesRead);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            //if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }
    }

    #endregion
}
