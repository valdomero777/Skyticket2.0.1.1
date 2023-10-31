using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace Skyticket
{
    [Serializable()]
    public class Settings
    {
        internal static string ConfigDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Skyticket";
        internal static string jobsDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Skyticket\\jobs";

        internal static string processedDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Skyticket\\jobs\\processed";

        private static string dbStringTemplate = @"Host={0};Username={1};Password={2};Database={3}";
        public static string DBString { get; set; }

        public static Settings CurrentSettings = null;
        public bool isConfigured = false;

        public string Language { get; set; }
        public string ActivationKey { get; set; }


        public string OutputPath { get; set; }
        public int ListenPort { get; set; }
        public string PrinterIP { get; set; }
        public string PrinterName { get; set; }
        public ConnectionTypes ConnectionType { get; set; }
        public bool DocumentPrinter { get; set; }
        public int DocumentPrinterDPI { get; set; }
        public int PrinterDots { get; set; }
        public int RightMargin { get; set; }
        public string DrawerCommand { get; set; }

        public bool PrintPaperAlways { get; set; }
        public bool PrintCustomerInfo { get; set; }

        public string SerialPort { get; set; }
        public string SerialPortWrite { get; set; }
        public bool EnableBarcodes { get; set; }
        public string DBServer { get; set; }
        public string DBUsername { get; set; }
        public string DBPassword { get; set; }
        public string DBName { get; set; }

        public string TerminalID { get; set; }
        public string ClientID { get; set; }
        public string PhoneSuffix { get; set; }
        public int PhoneDigits { get; set; }

        public POSTypes PosType { get; set; }

        public string FTPServer { get; set; }
        public string FTPUser { get; set; }
        public string FTPPassword { get; set; }
        public int FTPPort { get; set; }

        public string FTPTicketsFolder { get; set; }
        public string FTPCouponsFolder { get; set; }
        public int CouponLoadInterval { get; set; }
        public bool PoweredLogoEnabled { get; set; }

        public bool ServiceEnabled { get; set; }
        public string ServiceUser { get; set; }
        public string ServicePassword { get; set; }

        public Point codiPanelLocation { get; set; }
        public Point codiPaymentLocation { get; set; }
        public Point codiQRLocation { get; set; }
        public Size codiQRSize { get; set; }
        public bool CodiEnabled { get; set; }
        public int CodiWinTimer { get; set; }
        public int CodiProcTimer { get; set; }

        public int CodiRefSequence { get; set; }

        public bool MinimizeToTray { get; set; }

        public bool CustomerFeedback { get; set; }

        public bool NoPrint { get; set; }

        public Settings()
        {
            if (!Directory.Exists(ConfigDirectory))
                Directory.CreateDirectory(ConfigDirectory);

            if (!Directory.Exists(jobsDirectory))
                Directory.CreateDirectory(jobsDirectory);

            if (!Directory.Exists(processedDirectory))
                Directory.CreateDirectory(processedDirectory);

            ActivationKey = "";
            Language = "Spanish | es-ES";
            ListenPort = 9300;

            PrinterIP = "";
            PrinterName = "";
            DocumentPrinter = false;
            DocumentPrinterDPI = 60;
            PrinterDots = 570;
            RightMargin = 1;
            DrawerCommand = "1B,70,00,19,FA";

            DBServer = "";
            DBUsername = "";
            DBPassword = "";
            DBName = "";

            TerminalID = "";
            ClientID = "";
            PhoneSuffix = "521";
            PhoneDigits = 10;
            PosType = POSTypes.Others;

            FTPServer = "";
            FTPPort = 24;
            FTPUser = "";
            FTPPassword = "";
            FTPTicketsFolder = "";
            FTPCouponsFolder = "";
            CouponLoadInterval = 600;

            ServiceUser = "";
            ServicePassword = "";

            CodiWinTimer = 7;
            CodiProcTimer = 30;
            CodiRefSequence = 1;

            MinimizeToTray = true;
        }

        //-------------------------------------//
        public static void LoadSettings()
        {
            if (File.Exists(ConfigDirectory + "\\config.xml"))
            {
                CurrentSettings = ReadFromXmlFile<Settings>(ConfigDirectory + "\\config.xml");

                DBString = String.Format(dbStringTemplate,
                                        CurrentSettings.DBServer,
                                     CurrentSettings.DBUsername,
                                     CurrentSettings.DBPassword,
                                     CurrentSettings.DBName);
            }
            else
            {
                CurrentSettings = new Settings();
            }
        }
        //-------------------------------------//
        public static void SaveSettings()
        {
            CurrentSettings.isConfigured = true;
            WriteToXmlFile<Settings>(ConfigDirectory + "\\config.xml", CurrentSettings, false);

            DBString = String.Format(dbStringTemplate,
                                    CurrentSettings.DBServer,
                                     CurrentSettings.DBUsername,
                                     CurrentSettings.DBPassword,
                                     CurrentSettings.DBName);
        }
        //-------------------------------------//
        private static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append) where T : new()
        {
            TextWriter writer = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        //-------------------------------------//
        private static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        //-------------------------------------//
    }

    public enum ConnectionTypes
    {
        WinPrinter = 0,
        Network = 1,
    }

    public enum POSTypes
    {
        Others = 0,
        Aloha = 1,
        Siapa = 2,
        Micros =3,
        OPOS = 4,
        Star = 5,

    }
}
