using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing.Printing;
using Ghostscript.NET;
using Ghostscript.NET.Processor;
using System.Data.SQLite;
using Npgsql;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Xml.Linq;
using System.IO.Ports;


using ZXing;
using ZXing.Common;
using SnailDev.EscPosParser;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Skyticket.Classes;
using RestSharp;

namespace Skyticket
{
    public partial class MainForm : Form
    {
        static MainForm window;
        private static NotifyIcon trayIcon;
        private static ContextMenu trayMenu;

        string language = "";

        static string appPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Skyticket";
        static bool isRunning = false;
        static Thread ServerThread;
        static Thread UploadJobsThread;
        static List<string> queuedJobs = new List<string>();

        System.Timers.Timer couponsTimer = new System.Timers.Timer();
        static string previousCouponFileName = "";
        static string couponFileName = "";
        static string customHeader = "";

        static Socket ServerMainSocket = null;
        static Socket currentClient = null;
        static Thread PortThread;

        //*****
        SerialPort port = new SerialPort();
        List<byte> serialStream = new List<byte>();
        public static  List<SuperaUsers> UsersList = new List<SuperaUsers>();
        System.Timers.Timer serialProcessTimer = new System.Timers.Timer();
        private List<int> dataPort = new List<int>();

        public static int id_ticketr = 0;

        public static Boolean hasAlert = false;
        public static Boolean coupon = false;
        public static string clipPhone = "";


        CodiForm codiForm = new CodiForm();

        public MainForm()
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.CurrentSettings.Language))
                {
                    language = Settings.CurrentSettings.Language.Split('|')[1].Replace(" ", "");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                }
            }
            catch (Exception)
            {
            }

            InitializeComponent();
        }
        //***********************************//
        private void MainForm_Load(object sender, EventArgs e)
        {
            window = this;
            
            LoadTrayOptions();
            this.MinimizeBox = Settings.CurrentSettings.MinimizeToTray;

            #region version info
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            versionLabel.Text = "Version: " + version;
            #endregion
            //if (Settings.currentSettings.isConfigured)
            Task.Factory.StartNew(() =>
            {
                Start();
            });

            GetUsers();
           
        }
        //***********************************//
        #region Tray Icon stuff
        private void LoadTrayOptions()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            if (language.Contains("es"))
            {
                trayMenu.MenuItems.Add(TextsSpanish.Show, OnShow);
                trayMenu.MenuItems.Add(TextsSpanish.Exit, OnExit);
            }
            else
            {
                trayMenu.MenuItems.Add(Texts.Show, OnShow);
                trayMenu.MenuItems.Add(Texts.Exit, OnExit);
            }
            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Skyticket";
            //trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            trayIcon.Icon = new Icon(this.Icon, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (Settings.CurrentSettings.MinimizeToTray)
                if (WindowState == FormWindowState.Minimized)
            {
                Visible = false; // Hide form window.
                ShowInTaskbar = false; // Remove from taskbar.
                trayIcon.Visible = true;
            }
        }

        private void OnShow(object sender, EventArgs e)
        {
            Visible = true; // Hide form window.
            ShowInTaskbar = true; // Remove from taskbar.
            trayIcon.Visible = false;
            WindowState = FormWindowState.Normal;
            BringToFront();
        }

        private void OnExit(object sender, EventArgs e)
        {
            exitMenuItem_Click(null, null);
        }
        #endregion
        //***********************************//
        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            string message = Texts.Closing;
            if (language.Contains("es"))
                message = TextsSpanish.Closing;
            DialogResult dialogResult = DialogResult.No;

            if (sender != null)
                dialogResult = MessageBox.Show(message, "Skyticket", MessageBoxButtons.YesNo);
            else
                dialogResult = DialogResult.Yes;

            if (dialogResult == DialogResult.Yes)
            {
                trayIcon.Visible = false;
                try
                {
                    isRunning = false;
                    if (port.IsOpen)
                        port.Close();
                    //ServerThread.Abort();
                    
                }
                catch (Exception)
                {
                }

                try
                {
                    Environment.Exit(0);
                }
                catch (Exception)
                {
                }
            }
        }
        //***********************************//
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }
        //***********************************//
        private void updateMenuItem_Click(object sender, EventArgs e)
        {
            if (Updater.CheckUpdate() == DialogResult.Yes)
            {
                string originFilePath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "");
                //start with -u
                Updater.ExecuteAsAdmin(originFilePath, "-u");
                Environment.Exit(0);
            }
        }
        //***********************************//
        private void LicenseMenuItem_Click(object sender, EventArgs e)
        {
            ActivationForm activationForm = new ActivationForm();
            activationForm.ShowDialog();
        }
        //***********************************//
        private void ClearButton_Click(object sender, EventArgs e)
        {
            LogBox.Text = "";
            TimeSpan epochTime = DateTime.UtcNow.AddMinutes(10) - new DateTime(1970, 1, 1);

            var epoch = (long)epochTime.TotalMilliseconds;
        }
        //***********************************//
        private void LogBox_TextChanged(object sender, EventArgs e)
        {
            LogBox.SelectionStart = LogBox.Text.Length;
            LogBox.ScrollToCaret();
        }
        //***********************************//
        public static void UpdateLogBox(string Text, bool OverWrite = false)
        {
            try
            {
                Text = DateTime.Now.ToLongTimeString() + "> " + Text;
                if (OverWrite)
                    window.Invoke(new Action(() => { window.LogBox.Text = Text; }));
                else
                    window.Invoke(new Action(() => { window.LogBox.Text += Text + Environment.NewLine; }));
                UpdateLog(Text);
            }
            catch (Exception)
            {
            }
        }
        //***********************************//
        public static void UpdateLog(string Message)
        {
            try
            {
                Message = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "> " + Message;
                string filePath = appPath + "\\logs";
                string FileName = DateTime.Now.ToString("MM-dd-yyyy");
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                filePath = filePath + "\\Log " + FileName + ".txt";

                if (System.IO.File.Exists(filePath))
                {
                    using (StreamWriter writer = System.IO.File.AppendText(filePath))
                    {
                        writer.Write(Message + Environment.NewLine);
                    }
                }
                else
                {
                    using (StreamWriter writer = System.IO.File.CreateText(filePath))
                    {
                        writer.Write(Message + Environment.NewLine + Environment.NewLine);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        //***********************************//
        private void CouponButton_Click(object sender, EventArgs e)
        {
            LoadCoupon();
        }
        //***********************************//
        private void CouponsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            LoadCoupon();
            couponsTimer.Interval = 60 * 1000 * Settings.CurrentSettings.CouponLoadInterval;
        }
        //***********************************//
        private void LoadCoupon()
        {
            string fileName = "";
            try
            {
                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand Cmd = new NpgsqlCommand())
                    {
                        Cmd.CommandType = CommandType.Text;
                        Cmd.Connection = DBProvider.remoteConnection;

                        string query = "SELECT \"date\", \"imagepathftp\" FROM public.ct_coupon WHERE terminalid=@terminalid AND clientid=@clientid ORDER BY \"date\" DESC";

                        Cmd.CommandText = query;
                        Cmd.Parameters.AddWithValue("@terminalid", Settings.CurrentSettings.TerminalID);
                        Cmd.Parameters.AddWithValue("@clientid", Settings.CurrentSettings.ClientID);

                        using (NpgsqlDataReader reader = Cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                if (reader.Read())
                                {
                                    fileName = reader.GetString(1);
                                }
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                UpdateLogBox("LoadCoupon(): " + ex.Message);

                if (DBProvider.remoteConnection.State != ConnectionState.Open)
                    DBProvider.InitRemoteDB();
            }

            if (couponFileName.Length > 0)
                previousCouponFileName = couponFileName;

            if (fileName.Length > 0)
            {
                string sourcePath = Settings.CurrentSettings.FTPServer + ":" + Settings.CurrentSettings.FTPPort.ToString() +
                                        "/" + Settings.CurrentSettings.FTPCouponsFolder + "/" +
                                        Path.GetFileName(fileName);
                string destinationFile = Path.Combine(Settings.ConfigDirectory, "coupons");
                destinationFile = Path.Combine(destinationFile, fileName);
                if (FTP.FTPDownload(sourcePath, destinationFile))
                {
                    couponFileName = destinationFile;
                }
            }
            else
                couponFileName = "";

            {
                string sourcePath = Settings.CurrentSettings.FTPServer + ":" + Settings.CurrentSettings.FTPPort.ToString() +
                                            "/" + Settings.CurrentSettings.FTPCouponsFolder + "/" + "power.png";
                string destinationFile = Path.Combine(Settings.ConfigDirectory, "coupons");
                destinationFile = Path.Combine(destinationFile, "power.png");
                if (FTP.FTPDownload(sourcePath, destinationFile))
                {

                }
            }
        }
        //***********************************//
        private void LoadCustomHeader()
        {
            UpdateLog("cargando header");
            string fileName = "";
            try
            {
                lock (DBProvider.remoteDBLock)
                    using (NpgsqlCommand Cmd = new NpgsqlCommand())
                    {
                        Cmd.CommandType = CommandType.Text;
                        Cmd.Connection = DBProvider.remoteConnection;

                        string query = "SELECT \"image_path\" FROM public.ct_header WHERE id_terminal=@id_terminal AND id_client=@id_client";

                        Cmd.CommandText = query;
                        Cmd.Parameters.AddWithValue("@id_terminal", Convert.ToInt32(Settings.CurrentSettings.TerminalID));
                        Cmd.Parameters.AddWithValue("@id_client", Convert.ToInt32(Settings.CurrentSettings.ClientID));

                        using (NpgsqlDataReader reader = Cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                UpdateLog("hay header");

                                if (reader.Read())
                                {
                                    fileName = reader.GetString(0);
                                    UpdateLog(fileName);
                                }
                                else
                                {
                                    UpdateLog("error en la lectura del archivo");
                                }
                            }
                            else
                            {
                                UpdateLog("no hay header");
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                UpdateLogBox("LoadCustomHeader(): " + ex.Message);
                UpdateLog("LoadCustomHeader(): " + ex.Message);

                if (DBProvider.remoteConnection.State != ConnectionState.Open)
                    DBProvider.InitRemoteDB();
            }


            if (fileName.Length > 0)
            {
                try
                {
                    string sourcePath = Settings.CurrentSettings.FTPServer + ":" + Settings.CurrentSettings.FTPPort.ToString() +
                                            "/" + fileName;
                    string destinationFile = Path.Combine(Settings.ConfigDirectory, "headers");
                    if (!Directory.Exists(destinationFile))
                        Directory.CreateDirectory(destinationFile);
                    destinationFile = Path.Combine(destinationFile, Path.GetFileName(fileName));
                    if (FTP.FTPDownload(sourcePath, destinationFile))
                    {
                        customHeader = destinationFile;
                    }
                }catch (Exception ex)
                {
                    UpdateLog(ex.Message);
                }
            }
            else
            {
                customHeader = "";
                UpdateLog("no hay nada que mover");
            }
               
        }
        //***********************************//
        private void Start()
        {
            try
            {
                if (!isRunning)
                {
                    int clientID = Convert.ToInt32(Settings.CurrentSettings.ClientID);
                    string activationKey = Settings.CurrentSettings.ActivationKey;
                    Program.isActivated = true;
                    if (!Program.isActivated)
                    {
                        if (language.Contains("es"))
                            UpdateLogBox(TextsSpanish.UnRegistered);
                        else
                            UpdateLogBox(Texts.UnRegistered);
                    }

                    DBProvider.InitLocalDB();
                    DBProvider.InitRemoteDB();

                    if (ServerMainSocket == null)
                    {
                        ServerMainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        ServerMainSocket.Bind(new IPEndPoint(IPAddress.Loopback, 9300));
                    }
                    
                    ServerThread = new Thread(PrintJobThreadFunction);// ServerThreadFunction);
                    isRunning = true;
                    ServerThread.Priority = ThreadPriority.Highest;
                    ServerThread.SetApartmentState(ApartmentState.STA);
                    ServerThread.Start();

                    PortThread = new Thread(PortThreadFunction);
                    PortThread.Start();

                    UploadJobsThread = new Thread(UploadJobsThreadFunction);
                    UploadJobsThread.Start();

                    couponsTimer.Interval = 60 * 1000 * Settings.CurrentSettings.CouponLoadInterval;
                    couponsTimer.Elapsed += CouponsTimer_Elapsed;
                    couponsTimer.Enabled = true;

                    if (language.Contains("es"))
                        UpdateLogBox(TextsSpanish.WaitingJobs);
                    else
                        UpdateLogBox(Texts.WaitingJobs);

                    try
                    {
                        serialProcessTimer.Interval = 4000;
                        serialProcessTimer.Elapsed += SerialProcessTimer_Elapsed;
                        port = new SerialPort(Settings.CurrentSettings.SerialPort);
                        port.BaudRate = 9600;
                        port.DataBits = 8;
                        port.Parity = Parity.None;
                        port.StopBits = StopBits.One;
                        port.ReadBufferSize = 16 * 1024;
                        port.WriteBufferSize = 16 * 1024;

                        port.Open();

                        Thread.Sleep(500);
                        port.DiscardInBuffer();
                        if (port.IsOpen)
                        {
                            if (Settings.CurrentSettings.PosType == POSTypes.Others)
                                port.DataReceived += SerialPort_DataReceivedOthers;
                            else if (Settings.CurrentSettings.PosType == POSTypes.Micros)
                                port.DataReceived += SerialPort_DataReceivedMicros;
                            else if (Settings.CurrentSettings.PosType == POSTypes.Aloha)
                            {
                                port.DataReceived += SerialPort_DataReceivedAloha;
                            }
                        }
                            
                    }
                    catch (Exception ex)
                    {
                        UpdateLogBox("SerialPort: " + ex.Message);
                    }
                    
                    //UpdateLogBox("Service started, listening on port " + Settings.CurrentSettings.ListenPort.ToString());
                }
                
            }
            catch (Exception ex)
            {
                UpdateLogBox("Start(): " + ex.Message);
                UpdateLog("Start(): " + ex.Message);
            }
        }
        //***********************************//
        private void SerialPort_DataReceivedOthers(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialProcessTimer.Enabled)
            {
                serialProcessTimer.Stop();
                serialProcessTimer.Start();
            }
            else
                serialProcessTimer.Start();

            Thread.Sleep(50);

            int available = port.BytesToRead;

            if (available >0)
            {
                byte[] readBytes = new byte[available];
                port.Read(readBytes, 0, available);
                port.Write(new byte[] { 0x06 }, 0, 1);
                if (readBytes.Length > 0)
                    serialStream.AddRange(readBytes);
            }
        }
        //***********************************//
        private void SerialPort_DataReceivedAloha(object sender, SerialDataReceivedEventArgs e)
        {
            port.DataReceived -= SerialPort_DataReceivedAloha;
            UpdateLog("Recibiendo datos en puerto: Aloha \n Tamaño buffer " + port.ReadBufferSize + "\n Datos: " + port.ReadByte());
            int read = 0;
            int read2 = 0;
            //int lastRead = 0;
            bool ticketData = false;

            string dumpFileName = "aloha_dump_" + DateTime.Now.ToString("yyyyMMdd_HH") + ".hex";
            string lastDumpFileName = dumpFileName;


            string dumpFilePath = Path.Combine(Settings.processedDirectory, dumpFileName);


            //    fs.Write(serialStream.ToArray(), 0, serialStream.Count);
            //}


            var dumpFileStream = new FileStream(dumpFilePath, FileMode.Append, FileAccess.Write);

            while (isRunning)
            {
                try
                {
                    dumpFileName = "aloha_dump_" + DateTime.Now.ToString("yyyyMMdd_HH") + ".hex";
                    if (!dumpFileName.Equals(lastDumpFileName))
                    {
                        dumpFileStream.Flush();
                        dumpFileStream.Close();

                        lastDumpFileName = dumpFileName;
                        dumpFilePath = Path.Combine(Settings.processedDirectory, dumpFileName);
                        dumpFileStream = new FileStream(dumpFilePath, FileMode.Append, FileAccess.Write);
                    }

                    //Thread.Sleep(1);

                    ticketData = false;
                    // lastRead = read;
                    read = port.ReadByte();
                    Console.WriteLine(read);
                    dataPort.Add(read);
                    //UpdateLog("Data: " + read);
                    dumpFileStream.Write(new byte[] { (byte)read }, 0, 1);
                    dumpFileStream.Flush();
                    if (read == 0x1B || read == 0x76)
                    {
                        read2 = read;
                        read = port.ReadByte();
                        Console.WriteLine(read);
                        dumpFileStream.Write(new byte[] { (byte)read }, 0, 1);
                        dumpFileStream.Flush();
                        if (read == 0x76 || read == 0x1B)
                        {
                            //port.Write(new byte[] { 0x00 }, 0, 1);
                        }
                        else
                        {
                            ticketData = true;
                            serialStream.Add((byte)read2);
                        }
                    }
                    else
                        ticketData = true;

                    if (read == -1)
                        continue;

                    port.Write(new byte[] { 0x00 }, 0, 1);
                    if (ticketData)
                    {
                        serialStream.Add((byte)read);
                        Console.WriteLine("add " + read);
                        // UpdateLog("add: " + read);
                        //if (serialStream.Count > 10)
                        if (WaitingForm.window == null)
                        {
                            WaitingForm.window = new WaitingForm();
                            ThreadPool.QueueUserWorkItem(delegate
                            {
                                try
                                {
                                    Application.Run(WaitingForm.window);
                                }
                                catch (Exception)
                                {
                                }
                            });
                        }
                        if (serialProcessTimer.Enabled)
                        {
                            //for testing, do not start again
                            //serialProcessTimer.Stop();
                            //serialProcessTimer.Start();
                        }
                        else
                            serialProcessTimer.Start();
                    }
                }
                catch (Exception ex)
                {
                    if (isRunning)
                    {
                        UpdateLogBox("SerialPortMethod SerialPort_DataReceivedAloha: " + ex.Message);
                        UpdateLog("SerialPortMethod SerialPort_DataReceivedAloha: " + ex.Message);
                    }
                    else
                    { UpdateLog("SerialPort_DataReceivedAloha: " + ex.Message); }
                }
            }
            UpdateLog("SerialPortMethod Estatus de waiting form en evento aloha " + WaitingForm.window.ToString());
            UpdateLog("SerialPortMethod Datos de serial " + serialStream);

            //port.DataReceived += SerialPort_DataReceivedAloha;
        }
        //***********************************//
        private void SerialPort_DataReceivedMicros(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialProcessTimer.Enabled)
            {
                serialProcessTimer.Stop();
                serialProcessTimer.Start();
            }
            else
                serialProcessTimer.Start();

            Thread.Sleep(50);

            int available = port.BytesToRead;

            if (available > 0)
            {
                port.Write(new byte[] { 0x06 }, 0, 1);

                byte[] readBytes = new byte[available];
                port.Read(readBytes, 0, available);

                if (readBytes.Length > 0)
                    serialStream.AddRange(readBytes);
            }
        }
        //***********************************//
        private void SerialProcessTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (serialProcessTimer.Enabled)
                {
                    serialProcessTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                UpdateLogBox("SerialProcessTimer_Elapsed1: " + ex.Message);
            }

            try
            {
                if (serialStream.Count >= 100)
                {
                    //write serial stream to .ps file in jobs directory
                    string psfileName = "serial_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".ps";
                    string psFilePath = Path.Combine(Settings.processedDirectory, psfileName);

                    using (var fs = new FileStream(psFilePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(serialStream.ToArray(), 0, serialStream.Count);
                    }

                    //File.WriteAllBytes(psFilePath, serialStream.ToArray());
                    serialStream.Clear();

                    queuedJobs.Add(psFilePath);
                }
            }
            catch (Exception ex)
            {
                UpdateLogBox("SerialProcessTimer_Elapsed2: " + ex.Message);
            }

            try
            {
                if (WaitingForm.window != null)
                    WaitingForm.window.Invoke(new Action(() =>
                    {
                        WaitingForm.window.CloseForm();

                    }));
                WaitingForm.window = null;
            }
            catch (Exception ex)
            {
                UpdateLogBox("SerialProcessTimer_Elapsed3: " + ex.Message);
            }
        }
        //***********************************//
        private void PortThreadFunction()
        {
            #region Server thread
            while (isRunning)
            {
                try
                {
                    if (ServerMainSocket != null)
                    {
                        ServerMainSocket.Listen(10);
                        Socket TempSock = ServerMainSocket.Accept();
                        currentClient = TempSock;

                        UpdateLogBox("Socket incoming data...");

                        ClientFunction(TempSock);
                    }
                    else
                        break;
                }
                catch (Exception)// ex)
                {
                    //UpdateLogBox("in server function: " + ex.Message);
                }
            }
            #endregion
        }
        //***********************************//
        internal void ClientFunction(Socket ClientSocket)
        {
            int completeDataLength = 0;
            try
            {
                byte[] buffer = new byte[1000 * 1000];
                int loopCounter = 0;

                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();
                ClientSocket.Send(new byte[] { 0x06 });


                string psFileName = Settings.jobsDirectory + "\\" + DateTime.Now.ToString("yyyy_MM_dd_HHmmssff");
                psFileName += ".ps";

                if (File.Exists(psFileName))
                    File.Delete(psFileName);

                try
                {

                    MemoryStream memStream = new MemoryStream();

                    int failCount = 0;
                    while (ClientSocket.IsConnected() && isRunning)
                    {

                        loopCounter++;
                        int count = ClientSocket.Receive(buffer);

                        if (count > 0)
                        {
                            memStream.Write(buffer, 0, count);

                            completeDataLength += count;
                        }
                        else
                        {
                            failCount++;
                            if (failCount > 5)
                                break;
                        }
                    }

                    File.WriteAllBytes(psFileName, memStream.ToArray());

                }
                catch (Exception ex)
                {
                    if (ex.Message == "zero")
                    {
                        File.Delete(psFileName);
                    }
                }


                //all data is written to the file. it's PS file now.

                stopWatch.Stop();

                ClientSocket.Close();
                ClientSocket.Dispose();
                ClientSocket = null;

                GC.Collect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("in ClientFunction: {0} **** {1} **** {2}", ex.Message, ex.InnerException, ex.StackTrace));
            }
            finally
            {
                GC.Collect();
            }
        }
        //***********************************//
        private void PrintJobThreadFunction()
        {
            ThreadPool.QueueUserWorkItem(delegate { LoadCoupon(); });

            ThreadPool.QueueUserWorkItem(delegate { LoadCustomHeader(); });


            TicketDialog.contactsInfo = CustomerInfo.LoadCustomerInfo();

            if (Settings.CurrentSettings.CodiEnabled)
            {
                CodiAPI.codiInfo = CodiInfo.LoadCodiInfo();
                if (Settings.CurrentSettings.CodiEnabled)
                {
                    CodiPayment.StartStatusThread();
                    ThreadPool.QueueUserWorkItem(delegate { Application.Run(codiForm); });
                }
            }

            ThreadPool.QueueUserWorkItem(delegate
            {
                while (isRunning)
                {
                    try
                    {
                        string[] jobFiles = Directory.GetFiles(Settings.jobsDirectory, "*.ps", SearchOption.TopDirectoryOnly);

                        Thread.Sleep(500);

                        foreach (string jobFile in jobFiles)
                        {
                            FileStream receiptStream = WaitForFile(jobFile, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete);

                            if (receiptStream != null)
                                try { receiptStream.Close(); } catch (Exception) { continue; }

                            //string receiptText = File.ReadAllText(jobFile);

                            if (File.Exists(jobFile))
                            {
                                if (!queuedJobs.Contains(jobFile))
                                {
                                    queuedJobs.Add(jobFile);

                                    if (language.Contains("es"))
                                        UpdateLogBox(TextsSpanish.FoundJob + Path.GetFileName(jobFile));
                                    else
                                        UpdateLogBox(Texts.FoundJob + Path.GetFileName(jobFile));
                                }
                                //ProcessJob(jobFile);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateLogBox("PrintJobThreadFunction(): " + ex.Message);
                    }
                }
            });

            while (isRunning)
            {
                Thread.Sleep(250);
                while (queuedJobs.Count > 0)
                {
                    try
                    {
                        string jobFile = queuedJobs[0];
                        ProcessJob(jobFile);
                        if (queuedJobs.Count > 0)
                            queuedJobs.RemoveAt(0);

                        if (!isRunning)
                            break;
                    }
                    catch (Exception ex)
                    {
                        UpdateLogBox("queuedJobs: " + ex.Message);
                    }
                }
            }
        }
        //******************************//
        FileStream WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (int numTries = 0; numTries < 10; numTries++)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fullPath, mode, access, share);
                    return fs;
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                    Thread.Sleep(50);
                }
            }

            return null;
        }
        //******************************//
        private void ProcessJob(string psFilePath)
        {
            try
            {
                clipPhone = Clipboard.GetText();
            }
            catch (Exception ex)
            {

            }
            try
            {
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                string processedJobPath = Path.Combine(Settings.processedDirectory, Path.GetFileName(psFilePath));

                if (!psFilePath.Contains(Settings.processedDirectory))
                {
                    if (File.Exists(processedJobPath))
                        File.Delete(processedJobPath);

                    File.Move(psFilePath, processedJobPath);

                    
                    
                }

                ThreadPool.QueueUserWorkItem(delegate { PrintHelper.OpenCashDrawer(); });

                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                TicketChoice ticketChoice = null;

                if (Program.isActivated)
                {
                    //start processing while taking user's input

                    Thread ticketDialogThread = new Thread(() =>
                    {
                        ticketChoice = TicketDialog.ShowPopUp();
                    });
                    ticketDialogThread.Priority = ThreadPriority.Highest;
                    ticketDialogThread.SetApartmentState(ApartmentState.STA);
                    ticketDialogThread.Start();
                    //ThreadPool.QueueUserWorkItem(delegate {  });
                    //ticketChoice = TicketDialog.ShowPopUp();
                }
                else
                {
                    ticketChoice = new TicketChoice();
                    ticketChoice.printMethod = TicketMethod.Paper;
                }

                string pngfileName = timeStamp + ".png";
                string pngFilePath = Path.Combine(Settings.CurrentSettings.OutputPath, pngfileName);
                string pdfFilePath = Path.Combine(Settings.CurrentSettings.OutputPath, timeStamp + ".pdf");
                string psFileText = File.ReadAllText(processedJobPath);

                if (psFileText.Length <= 200)
                    return;

                int receiptHeight = 0;

                int ticketType = 0;

                List<byte> printBytes = new List<byte>();

                if (File.Exists(customHeader))
                {
                    Bitmap bitmap = PrintHelper.ConvertToBitmap(customHeader);
                    printBytes.AddRange(PrintHelper.GetImageBytes(bitmap));
                }

                if (psFileText.ToLower().Contains("%%targetdevice") || psFileText.ToLower().Contains("%!ps-adobe"))
                {
                    int pages = GetPageCount(processedJobPath);

                    if (pages > 1)
                    {
                        List<string> pngPages = new List<string>();

                        for (int i = 1; i <= pages; i++)
                        {
                            string pngPageName = Path.Combine(Settings.processedDirectory, pngfileName + "_" + i.ToString());
                            if (WritePSToPng(processedJobPath, pngPageName, i))
                            {
                                pngPages.Add(pngPageName);
                            }
                        }

                        stopwatch.Stop();
                        UpdateLogBox("multiple png files created in ms " + stopwatch.ElapsedMilliseconds);
                        stopwatch.Reset();
                        stopwatch.Start();

                        //to do: add pages to bytes separately, without combining
                        if (CombinePNGPages(pngPages, pngfileName))
                        {
                            try
                            {
                                Bitmap bitmap = PrintHelper.ConvertToBitmap(pngFilePath);
                                printBytes.AddRange(PrintHelper.GetImageBytes(bitmap));
                            }
                            catch (Exception ex)
                            {
                                UpdateLogBox("PngPages to printBytes: " + ex.Message);
                            }
                        }

                        stopwatch.Stop();
                        UpdateLogBox("png files combined created in ms " + stopwatch.ElapsedMilliseconds);
                        stopwatch.Reset();
                        stopwatch.Start();
                    }
                    else
                    {
                        if (WritePSToPng(processedJobPath, pngFilePath))
                        {
                            stopwatch.Stop();
                            UpdateLogBox("png file created in ms " + stopwatch.ElapsedMilliseconds);
                            stopwatch.Reset();
                            stopwatch.Start();

                            if (language.Contains("es"))
                                UpdateLogBox("PS " + TextsSpanish.TicketConverted + pngfileName);
                            else
                                UpdateLogBox("PS " + Texts.TicketConverted + pngfileName);

                            Bitmap trimmedImage = null;
                            using (Bitmap initial = new Bitmap(pngFilePath))
                            {
                                trimmedImage = ImageTrimWhite(initial);
                            }

                            try { File.Delete(pngFilePath); } catch (Exception) { }

                            timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            pngfileName = timeStamp + ".png";
                            pngFilePath = Path.Combine(Settings.CurrentSettings.OutputPath, pngfileName);

                            trimmedImage.Save(pngFilePath, ImageFormat.Png);

                            stopwatch.Stop();
                            UpdateLogBox("white trimmed in ms " + stopwatch.ElapsedMilliseconds);
                            stopwatch.Reset();
                            stopwatch.Start();

                            Bitmap bitmap = PrintHelper.ConvertToBitmap(pngFilePath);
                            printBytes.AddRange(PrintHelper.GetImageBytes(bitmap));
                        }
                    }
                }
                else//it's ESC/POS
                {
                    RemoveWhiteLines(processedJobPath, processedJobPath);
                    ticketType = 1;

                    ////if (Settings.CurrentSettings.EnableBarcodes)
                    ////    AddBarcodes(pngFilePath, psBytes, out receiptHeight);

                    byte[] posBytes = File.ReadAllBytes(processedJobPath);

                    if (Settings.CurrentSettings.PosType == POSTypes.Aloha)
                    {
                        string[] ticketLines = GetESCPOSText(processedJobPath);

                        for (int i = 0; i < ticketLines.Length; i++)
                        {
                            while (ticketLines[i].Length > 46)
                            {
                                if (ticketLines[i].Contains("   "))
                                    ticketLines[i] = ticketLines[i].Replace("   ", "  ");
                                else
                                    break;
                            }

                            
                                

                                if (ticketLines[i].Replace("\r", "").Length <= 1)
                                continue;
                            ticketLines[i] = ticketLines[i].Replace("\r", "");
                            printBytes.AddRange(Encoding.ASCII.GetBytes(ticketLines[i]));
                            printBytes.Add(0x0A);
                            
                        }

                       
                    }
                    else
                    {
                        string bytesStr = Converters.ByteArrayToHexString(posBytes);
                        bytesStr = bytesStr.ToUpper().Replace("1D564200", "");
                        bytesStr = bytesStr.ToUpper().Replace("1B69", "");
                        posBytes = Converters.HexStringToByteArray(bytesStr);
                        printBytes.AddRange(posBytes);
                    }
                    printBytes.AddRange(PrintHelper.initBytes);
                }
                //********************//

                //add coupon, powered logo and customer info to print data
                {
                    string couponToUse = "";
                    if (File.Exists(couponFileName))
                        couponToUse = couponFileName;
                    else if (File.Exists(previousCouponFileName))
                        couponToUse = previousCouponFileName;


                    if (File.Exists(couponToUse))
                    {
                        Bitmap bitmap = PrintHelper.ConvertToBitmap(couponToUse);
                        //printBytes.Clear();
                        //printBytes.AddRange(PrintHelper.initBytes);
                        printBytes.AddRange(PrintHelper.GetImageBytes(bitmap));
                    }

                    if (Settings.CurrentSettings.PoweredLogoEnabled)
                    {
                        string poweredLogo = Path.Combine(Settings.ConfigDirectory, "coupons");
                        poweredLogo = Path.Combine(poweredLogo, "power.png");

                        if (File.Exists(poweredLogo))
                        {
                            Bitmap bitmap = PrintHelper.ConvertToBitmap(poweredLogo);
                            printBytes.AddRange(PrintHelper.GetImageBytes(bitmap));

                        }
                    }
                }

                while (ticketChoice == null)
                {
                    Thread.Sleep(100);
                }

                stopwatch.Stop();
                stopwatch.Reset();
                stopwatch.Start();

                if (ticketChoice.printMethod == TicketMethod.None)
                    return;

                if (Settings.CurrentSettings.PrintCustomerInfo)
                {
                    string customerInfo = GetCustomerInfo(ticketChoice.targetInput);

                    if (!string.IsNullOrEmpty(customerInfo))
                    {
                        printBytes.AddRange(Encoding.ASCII.GetBytes(customerInfo));
                    }
                }

                if (ticketChoice.printMethod == TicketMethod.Batch)
                {
                    File.Move(processedJobPath, psFilePath);
                    ProcessBatch();
                    return;
                }
                //print PostScript/image ticket
                else if (ticketChoice.printMethod == TicketMethod.Paper ||
                        Settings.CurrentSettings.PrintPaperAlways)
                {
                    if (Settings.CurrentSettings.PosType == POSTypes.Siapa)
                    {
                        UpdateLogBox("Converting to PDF - " + POSTypes.Siapa.ToString());
                        if (WritePSToPDF(processedJobPath, pdfFilePath))
                        {
                            PrintPDFToPrinter(pdfFilePath);
                            UpdateLogBox("PDF print sent " + POSTypes.Siapa.ToString());
                            ThreadPool.QueueUserWorkItem(delegate { PrintHelper.Print(PrintHelper.cutBytes); });
                        }
                    }
                    else
                    {
                        printBytes.AddRange(PrintHelper.cutBytes);
                        ThreadPool.QueueUserWorkItem(delegate { PrintHelper.Print(printBytes.ToArray()); });
                        //PrintHelper.Print(pngFilePath, receiptHeight);
                    }
                    stopwatch.Stop();
                    UpdateLogBox("Print completed in ms " + stopwatch.ElapsedMilliseconds);
                    stopwatch.Reset();
                    stopwatch.Start();

                    if (ticketChoice.targetInput.Length <= 0)
                        ticketChoice.targetInput = "1";
                }

                if (ticketType == 1)
                {
                    stopwatch.Stop();
                    string[] ticketLines = GetESCPOSText(processedJobPath);

                    if (ticketLines.Length > 0)
                    {
                        bool result = WriteTextToPng(ticketLines, pngFilePath);
                    }
                    UpdateLogBox("(for upload) WriteTextToPng in ms " + stopwatch.ElapsedMilliseconds);
                    stopwatch.Reset();
                    stopwatch.Start();
                }

                if (File.Exists(customHeader))
                {
                    try
                    {
                        AddCustomHeader(pngFilePath, out receiptHeight);
                    }
                    catch (Exception ex)
                    {
                        UpdateLogBox("AddCustomHeader: " + ex.Message);
                    }
                    stopwatch.Stop();
                    UpdateLogBox("(for upload) CustomHeader added in ms " + stopwatch.ElapsedMilliseconds);
                    stopwatch.Reset();
                    stopwatch.Start();
                }

                try
                {
                    AddCouponImage(pngFilePath, out receiptHeight);
                    stopwatch.Stop();
                    UpdateLogBox("(for upload) coupon added in ms " + stopwatch.ElapsedMilliseconds);
                    stopwatch.Reset();
                    stopwatch.Start();
                }
                catch (Exception ex)
                {
                    UpdateLogBox("AddCouponHeader: " + ex.Message);
                }

                if (Settings.CurrentSettings.PoweredLogoEnabled)
                {
                    try
                    {
                        AddPoweredImage(pngFilePath, out receiptHeight);
                    }
                    catch (Exception ex)
                    {
                        UpdateLogBox("AddPowered: " + ex.Message);
                    }
                    stopwatch.Stop();
                    UpdateLogBox("(for upload) PoweredBy added in ms " + stopwatch.ElapsedMilliseconds);
                    stopwatch.Reset();
                    stopwatch.Start();
                }

                if (Settings.CurrentSettings.PrintCustomerInfo)
                {
                    try
                    {
                        AddCustomerInfo(pngFilePath, ticketChoice.targetInput, ticketType, ref receiptHeight);
                    }
                    catch (Exception ex)
                    {
                        UpdateLogBox("AddCustomerInfo: " + ex.Message);
                    }
                    stopwatch.Stop();
                    UpdateLogBox("(for upload) CustomerInfo added in ms " + stopwatch.ElapsedMilliseconds);
                    stopwatch.Reset();
                    stopwatch.Start();
                }

                if (Program.isActivated)
                    CreateJobLocal(pngfileName, processedJobPath, ticketChoice.printMethod, ticketChoice.targetInput);
            }
            catch (Exception ex)
            {
                UpdateLogBox("ProcessJob(): " + ex.Message + ex.StackTrace + ex.InnerException);
            }
            finally
            {
                GC.Collect();
            }

#if !DEBUG
            Processors.CleanOldJobs();
#endif
        }
        //******************************//
        private void ProcessBatch()
        {
            //DialogResult batchReady = DialogResult.None;
            //Thread ticketDialogThread = new Thread(() =>
            //{
            //    batchReady = MessageBox.Show("Select Yes to confirm when batch is ready", "Skyticket", MessageBoxButtons.YesNo);
            //});
            //ticketDialogThread.Priority = ThreadPriority.Highest;
            //ticketDialogThread.SetApartmentState(ApartmentState.STA);
            //ticketDialogThread.Start();

            //while (batchReady ==  DialogResult.None)
            //{
            //    Thread.Sleep(100);
            //}
            //if (batchReady != DialogResult.Yes)
            //    return;

            Stopwatch stopwatch = new Stopwatch();

            List<byte> printBytes = new List<byte>();

            while (queuedJobs.Count > 0)
            {
                string psFilePath = queuedJobs[0];
                stopwatch.Stop();
                stopwatch.Start();
                string processedPath = Path.Combine(Settings.processedDirectory, Path.GetFileName(psFilePath));

                if (!psFilePath.Contains(Settings.processedDirectory))
                {
                    if (File.Exists(processedPath))
                        File.Delete(processedPath);

                    File.Move(psFilePath, processedPath);
                }

                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string pngfileName = timeStamp + ".png";
                string pngFilePath = Path.Combine(Settings.CurrentSettings.OutputPath, pngfileName);

                //int ticketType = 0;
                string psFileText = File.ReadAllText(processedPath);

                if (psFileText.Length <= 200)
                    continue;

                if (psFileText.ToLower().Contains("%%targetdevice") || psFileText.ToLower().Contains("%!ps-adobe"))
                {
                    int pages = GetPageCount(processedPath);

                    if (pages > 1)
                    {
                        List<string> pngPages = new List<string>();

                        for (int i = 1; i <= pages; i++)
                        {
                            string pngPageName = Path.Combine(Settings.processedDirectory, pngfileName + "_" + i.ToString());
                            if (WritePSToPng(processedPath, pngPageName, i))
                            {
                                pngPages.Add(pngPageName);
                            }
                        }

                        stopwatch.Stop();
                        UpdateLogBox("multiple png files created in ms " + stopwatch.ElapsedMilliseconds);
                        stopwatch.Reset();
                        stopwatch.Start();

                        if (CombinePNGPages(pngPages, pngfileName))
                        {
                            Bitmap bitmap = PrintHelper.ConvertToBitmap(pngFilePath);
                            printBytes.AddRange(PrintHelper.GetImageBytes(bitmap));
                        }

                        stopwatch.Stop();
                        UpdateLogBox("png files combined created in ms " + stopwatch.ElapsedMilliseconds);
                        stopwatch.Reset();
                        stopwatch.Start();
                    }
                    else
                    {
                        if (WritePSToPng(processedPath, pngFilePath))
                        {
                            stopwatch.Stop();
                            UpdateLogBox("png file created in ms " + stopwatch.ElapsedMilliseconds);
                            stopwatch.Reset();
                            stopwatch.Start();

                            if (language.Contains("es"))
                                UpdateLogBox("PS " + TextsSpanish.TicketConverted + pngfileName);
                            else
                                UpdateLogBox("PS " + Texts.TicketConverted + pngfileName);

                            Bitmap trimmedImage = null;
                            using (Bitmap initial = new Bitmap(pngFilePath))
                            {
                                trimmedImage = ImageTrimWhite(initial);
                            }

                            try { File.Delete(pngFilePath); } catch (Exception e) { UpdateLogBox("trying delete original picture" + e); }

                            timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            pngfileName = timeStamp + ".png";
                            pngFilePath = Path.Combine(Settings.CurrentSettings.OutputPath, pngfileName);

                            trimmedImage.Save(pngFilePath, ImageFormat.Png);

                            stopwatch.Stop();
                            UpdateLogBox("white trimmed in ms " + stopwatch.ElapsedMilliseconds);
                            stopwatch.Reset();
                            stopwatch.Start();

                            CreateJobLocal(pngfileName, processedPath, TicketMethod.Batch, "");

                            Bitmap bitmap = PrintHelper.ConvertToBitmap(pngFilePath);
                            printBytes.AddRange(PrintHelper.GetImageBytes(bitmap));
                        }
                    }
                }
                else//it's ESC/POS
                {
                    //ticketType = 1;

                    ////if (Settings.CurrentSettings.EnableBarcodes)
                    ////    AddBarcodes(pngFilePath, psBytes, out receiptHeight);

                    if (Settings.CurrentSettings.PosType == POSTypes.Aloha)
                    {
                        string[] ticketLines = GetESCPOSText(processedPath);

                        for (int i = 0; i < ticketLines.Length; i++)
                        {
                            while (ticketLines[i].Length > 46)
                            {
                                ticketLines[i] = ticketLines[i].Replace("   ", "  ");
                            }

                            if (ticketLines[i].Replace("\r", "").Length <= 1)
                                continue;
                            ticketLines[i] = ticketLines[i].Replace("\r", "");
                            printBytes.AddRange(Encoding.ASCII.GetBytes(ticketLines[i]));
                            printBytes.Add(0x0A);
                        }
                    }
                    else
                    {
                        byte[] posBytes = File.ReadAllBytes(processedPath);
                        string bytesStr = Converters.ByteArrayToHexString(posBytes);
                        bytesStr = bytesStr.ToUpper().Replace("1D564200", "");
                        bytesStr = bytesStr.ToUpper().Replace("1B69", "");
                        posBytes = Converters.HexStringToByteArray(bytesStr);

                        printBytes.AddRange(posBytes);
                    }

                    string[] ticketLinesPNG = GetESCPOSText(processedPath);
                    if (ticketLinesPNG.Length > 0)
                    {
                        bool result = WriteTextToPng(ticketLinesPNG, pngFilePath);
                        if(result)
                            CreateJobLocal(pngfileName, processedPath, TicketMethod.Batch, "");
                    }
                    

                }

                var bytes = printBytes.ToArray();
                PrintHelper.Print(bytes);
                //ThreadPool.QueueUserWorkItem(delegate {

                //});
                printBytes.Clear();

                if (queuedJobs.Count > 0)
                    queuedJobs.RemoveAt(0);
            }

            printBytes.AddRange(PrintHelper.cutBytes);
            ThreadPool.QueueUserWorkItem(delegate { PrintHelper.Print(printBytes.ToArray()); });
        }
        //******************************//
        private void CreateJobLocal(string ticketFileName, string jobPath, TicketMethod method, string target)
        {
            try
            {
                lock (DBProvider.localDBLock)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = DBProvider.localConnection;
                        cmd.CommandType = CommandType.Text;


                        cmd.CommandText = "INSERT INTO printJobs (id_terminal, id_client, ticketImage, jobFileName, printMethod, email, mobilePhone, sent)" +
                            " VALUES (@id_terminal, @id_client, @ticketImage, @jobFileName, @printMethod, @email, @mobilePhone, @sent)";

                        cmd.Parameters.AddWithValue("@id_terminal", Settings.CurrentSettings.TerminalID);
                        cmd.Parameters.AddWithValue("@id_client", Settings.CurrentSettings.ClientID);
                        cmd.Parameters.AddWithValue("@ticketImage", ticketFileName);
                        cmd.Parameters.AddWithValue("@jobFileName", Path.GetFileName(jobPath));
                        cmd.Parameters.AddWithValue("@printMethod", method.ToString());

                        if (target.Contains("@"))
                        {
                            cmd.Parameters.AddWithValue("@email", target);
                            cmd.Parameters.AddWithValue("@mobilePhone", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@email", "");
                            cmd.Parameters.AddWithValue("@mobilePhone", target);
                        }
                        cmd.Parameters.AddWithValue("@sent", 0);

                        int count = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateLogBox("CreateJobLocal(): " + ex.Message);
            }
        }
        //******************************//
        private void UpdateJobLocal(int jobID)
        {
            try
            {
                lock (DBProvider.localDBLock)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = DBProvider.localConnection;
                        cmd.CommandType = CommandType.Text;


                        cmd.CommandText = "UPDATE printJobs SET sent=@sent, dateSent=julianday('now') WHERE id=" + jobID;

                        cmd.Parameters.AddWithValue("@sent", 1);
                        //cmd.CommandText = cmd.CommandText.Replace("@dateSent", "#" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "#");

                        int count = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateLogBox("in UpdateJobLoc(): " + ex.Message);
            }
        }
        //******************************//
        private static bool SaveJobRemoteDB(string ticketFileName, string jobFileName, TicketMethod method, string target)
        {
            bool result = false;
            try
            {
                Ticket ti = new Ticket();
                ti.id_terminal = Convert.ToInt32(Settings.CurrentSettings.TerminalID);
                ti.id_client = Convert.ToInt32(Settings.CurrentSettings.ClientID);
                ti.ticketimagepath = ticketFileName;
                ti.printmethod = method.ToString();
                if (target.Contains("@"))
                {
                    ti.email = target;
                    ti.mobilephone = "";
                }
                else
                {
                    ti.email = "";
                    ti.mobilephone = target;
                }

                ti.sent = true;
                ti.datesent = DateTime.Now;
                ti.details = SaveJobTextDB(jobFileName);

                result =  TicketRequest(ti);

                
            }
            catch (Exception ex)
            {
                UpdateLog("SaveJobRemoteDB(): " + ex.Message);

                if (DBProvider.remoteConnection.State != ConnectionState.Open)
                    DBProvider.InitRemoteDB();
            }

            return result;
        }
        //******************************//
        private static bool SaveJobService(byte[] ticketImage, string jobFileName, TicketMethod method, string target)
        {
            bool result = false;
            try
            {
                SkyticketWebService.ctTicketsBeans requestFields = new SkyticketWebService.ctTicketsBeans();
                requestFields.usuario = Settings.CurrentSettings.ServiceUser;
                requestFields.passw = Settings.CurrentSettings.ServicePassword;

                requestFields.imagen = ticketImage;

                requestFields.id_cliente = Convert.ToInt32(Settings.CurrentSettings.ClientID);
                requestFields.id_terminal = Convert.ToInt32(Settings.CurrentSettings.TerminalID);

                requestFields.printMethod = method.ToString();

                if (method == TicketMethod.Email)
                {
                    requestFields.email = target;
                    requestFields.mobilePhone = "";
                }
                else
                {
                    requestFields.mobilePhone = target;
                    requestFields.email = "";
                }

                requestFields.sent = "1";


                #region get ticket content
                string ticketContent = "";

                jobFileName = Path.Combine(Settings.processedDirectory, jobFileName);

                string psFileText = File.ReadAllText(jobFileName);
                if (psFileText.ToLower().Contains("%%targetdevice") || psFileText.ToLower().Contains("%!ps-adobe"))
                {

                }
                else
                {
                    string[] ticketLines = GetESCPOSText(jobFileName);

                    foreach (string ticketLine in ticketLines)
                    {
                        ticketContent += ticketLine + Environment.NewLine;
                    }

                    requestFields.ticket_content = ticketContent;
                }
                #endregion

                var ticketBeans = new SkyticketWebService.ctTicketsBeans[1];
                ticketBeans[0] = requestFields;
                SkyticketWebService.ingresa_ticketsRequest request = new SkyticketWebService.ingresa_ticketsRequest();
                request.ticket = ticketBeans;

                SkyticketWebService.ws_tickets_inClient client = new SkyticketWebService.ws_tickets_inClient();
                var temp = client.ingresa_tickets(request);

                UpdateLogBox("Service upload: " + temp.@return);
                result = true;
            }
            catch (Exception ex)
            {
                UpdateLogBox("SaveJobService(): " + ex.Message);
            }

            return result;
        }
        //******************************//
        private static string SaveJobTextDB(string psFileName)
        {
            string text = "";

            try
            {

                psFileName = Path.Combine(Settings.processedDirectory, psFileName);

                string psFileText = File.ReadAllText(psFileName);
                if (psFileText.ToLower().Contains("%%targetdevice") || psFileText.ToLower().Contains("%!ps-adobe"))
                    return "";

                string[] ticketLines = GetESCPOSText(psFileName);

                if (ticketLines.Length <= 0)
                    return "";

                foreach (string ticketLine in ticketLines)
                {
                    text += ticketLine + Environment.NewLine;
                }

                int ticketID = DBProvider.GetLastTicketID();

                



            }
            catch (Exception ex)
            {
                UpdateLog("SaveJobTextDB(): " + ex.Message);

                if (DBProvider.remoteConnection.State != ConnectionState.Open)
                    DBProvider.InitRemoteDB();
            }

            return text;
        }
        //******************************//
        private async void UploadJobsThreadFunction()
        {
            int JobWithoutPng;
            while (isRunning)
            {
                try
                {
                    Thread.Sleep(5000);
                    List<PrintJob> jobs = PrintJob.LoadPrintJobs();


                    foreach (var job in jobs)
                    {
                        string pngFilePath = Path.Combine(Settings.CurrentSettings.OutputPath, job.ticketImage);
                        if (!File.Exists(pngFilePath))
                        {
                            //send to FTP
                            UpdateLogBox("El archivo " + pngFilePath + " no existe" + "ID Job: " + job.id);
                            UpdateJobLocal(job.id);
                        }
                        else
                        {
                            byte[] imageBytes = File.ReadAllBytes(pngFilePath);
                            if (await Azure.UploadImageAsync(pngFilePath))
                            {

                                bool remoteResult = false;
                                //save to remote DB
                                if (job.printMethod == TicketMethod.Email.ToString())
                                    remoteResult = SaveJobRemoteDB(job.ticketImage, job.jobFileName, Converters.ParseEnum<TicketMethod>(job.printMethod), job.email);
                                else
                                {
                                    remoteResult = SaveJobRemoteDB(job.ticketImage, job.jobFileName, Converters.ParseEnum<TicketMethod>(job.printMethod), job.mobilePhone);
                                    if(TicketDialog.isCopied)
                                        remoteResult = SaveJobRemoteDB(job.ticketImage, job.jobFileName, Converters.ParseEnum<TicketMethod>("Whatsapp"), TicketDialog.phone);
                                }
                                    

                                //mark as sent in local DB
                                if (remoteResult)
                                {
                                    JobWithoutPng = job.id;
                                    UpdateJobLocal(job.id);

                                    if (Settings.CurrentSettings.ServiceEnabled)
                                    {
                                        if (job.printMethod == TicketMethod.Email.ToString())
                                            SaveJobService(imageBytes, job.jobFileName, Converters.ParseEnum<TicketMethod>(job.printMethod), job.email);
                                        else
                                            SaveJobService(imageBytes, job.jobFileName, Converters.ParseEnum<TicketMethod>(job.printMethod), job.mobilePhone);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    UpdateLog("UploadJobsThreadFunction(): " + ex.Message);

                }
            }
        }
        //******************************//
        private bool WritePSToPng(string psFile, string pngFile, int page = 1)
        {
            bool returnVal = false;
            string inputFile = psFile;

            try
            {
                string appFilePath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "");
                appFilePath = Path.GetDirectoryName(appFilePath);
                GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(@appFilePath+"\\gsdll32.dll");

                if (IntPtr.Size == 8)
                    gvi = new GhostscriptVersionInfo(@appFilePath + "\\gsdll64.dll");
                else
                    gvi = new GhostscriptVersionInfo(@appFilePath + "\\gsdll32.dll");

                using (GhostscriptProcessor processor = new GhostscriptProcessor(gvi))
                {
                    List<string> switches = new List<string>();
                    switches.Add("-dQUIET");
                    switches.Add("-dSAFER");
                    switches.Add("-dBATCH");
                    switches.Add("-dNOPAUSE");
                    switches.Add("-dNOPROMPT");
                    switches.Add("-dFirstPage=" + page);
                    switches.Add("-dLastPage=" + page);
                    //switches.Add("-dAutoRotatePages=/None");
                    switches.Add("-sDEVICE=pngmono");//png16m
                                                     //switches.Add("-dGraphicsAlphaBits=4");
                                                     //switches.Add("-dDownScaleFactor=2");
                    switches.Add("-sOutputFile=" + pngFile);
                    switches.Add("-q");
                    switches.Add("-f");

                    switches.Add(inputFile);

                    processor.StartProcessing(switches.ToArray(), null);
                    returnVal = true;

                }
            }
            catch (Exception ex)
            {
                if (language.Contains("es"))
                    UpdateLogBox(TextsSpanish.ConvertError + ex.Message);
                else
                    UpdateLogBox(Texts.ConvertError + ex.Message);

                if (ex.Message.ToLower().Contains("ghostscript native library could not be found"))
                {
                    string unprocessPath = Path.Combine(Settings.jobsDirectory, Path.GetFileName(psFile));

                    if (File.Exists(unprocessPath))
                        File.Delete(unprocessPath);

                    File.Move(psFile, unprocessPath);

                    //Thread.Sleep(1500);
                    TicketDialog.ClosePopup();
                    string originFilePath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "");
                    Updater.ExecuteSelf(originFilePath, "-d");

                    exitMenuItem_Click(null, null);
                }
            }
            return returnVal;
        }
        //***********************************//
        private bool WritePSToPDF(string psFile, string pdfFile)
        {
            bool returnVal = false;
            string inputFile = psFile;

            string appFilePath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "");
            appFilePath = Path.GetDirectoryName(appFilePath);
            GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(@appFilePath + "\\gsdll32.dll");

            if (IntPtr.Size == 8)
                gvi = new GhostscriptVersionInfo(@appFilePath + "\\gsdll64.dll");
            else
                gvi = new GhostscriptVersionInfo(@appFilePath + "\\gsdll32.dll");

            using (GhostscriptProcessor processor = new GhostscriptProcessor(gvi))
            {
                List<string> switches = new List<string>();
                switches.Add("-dQUIET");
                switches.Add("-dSAFER");
                switches.Add("-dBATCH");
                switches.Add("-dNOPAUSE");
                switches.Add("-dNOPROMPT");
                switches.Add("-sDEVICE=pdfwrite");
                switches.Add("-sOutputFile=" + pdfFile);
                switches.Add("-q");
                switches.Add("-f");
                switches.Add(inputFile);
                try
                {
                    processor.StartProcessing(switches.ToArray(), null);
                    returnVal = true;
                }
                catch (Exception ex)
                {
                    UpdateLogBox("while saving PDF: " + ex.Message + " **** " + ex.StackTrace + " *** " + ex.InnerException);
                }
            }

            return returnVal;
        }
        //***********************************//
        private void PrintPDFToPrinter(string pdfFile)
        {
            //added for Epson U200:
            //send PDF file

            //gswin32c.exe  -dPrinted -dBATCH -dNOPAUSE -dNOSAFER -q -dNumCopies=1 -sDEVICE=mswinpr2 -dNoCancel -sOutputFile="%printer%HP LaserJet 1320" "D:\Edu\VS Projects\GhostScriptTest\GhostScriptTest\bin\Debug\newPOS.pdf"

            string argument = "-dPrinted -dBATCH -dNOPAUSE -dNOSAFER -q -dNumCopies=1 -sDEVICE=mswinpr2 -dNoCancel -sOutputFile=\"%printer%{0}\" \"{1}\"";

            argument = string.Format(argument, Settings.CurrentSettings.PrinterName, pdfFile);

            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.RedirectStandardError = true;
            string appFilePath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "");
            appFilePath = Path.GetDirectoryName(appFilePath);
            process.StartInfo.FileName = appFilePath+"\\gswin32c.exe";
            process.StartInfo.Arguments = argument;
            //process.StartInfo.UserName = "Administrator";
            process.Start();
            //process.WaitForExit();
            //process.Dispose();
        }
        //***********************************//
        private void AddCustomHeader(string pngFile, out int receiptHeight)
        {
            receiptHeight = 0;
            Bitmap newImage = null;
            int headerHeight = 300;
            using (Image ticketImage = Image.FromFile(pngFile))
            {
                receiptHeight = ticketImage.Height;

                if (File.Exists(customHeader))
                {
                    using (Image headerImage = Image.FromFile(customHeader))
                    {
                        int width = ticketImage.Width;
                        int height = ticketImage.Height;

                        double proportion = (double)headerImage.Width / (double)headerImage.Height;
                        headerHeight = (int)((double)width / proportion);

                        newImage = new Bitmap(width, height + headerHeight);

                        Graphics g = Graphics.FromImage(newImage);

                        g.Clear(Color.White);

                        g.DrawImage(headerImage, 0, 0, ticketImage.Width, headerHeight);
                        g.DrawImage(ticketImage, 0, headerHeight, ticketImage.Width, ticketImage.Height);

                        g.Dispose();
                        headerImage.Dispose();
                    }
                }
                ticketImage.Dispose();
            }

            if (newImage != null)
            {
                newImage.Save(pngFile, ImageFormat.Png);
                receiptHeight = newImage.Height;
                newImage.Dispose();
            }
            GC.Collect();
        }
        //***********************************//
        private void AddCouponImage(string pngFile, out int receiptHeight)
        {
            receiptHeight = 0;
            Bitmap newImage = null;
            int couponHeight = 512;
            using (Image ticketImage = Image.FromFile(pngFile))
            {
                receiptHeight = ticketImage.Height;

                string couponToUse = "";
                if (File.Exists(couponFileName))
                    couponToUse = couponFileName;
                else if (File.Exists(previousCouponFileName))
                    couponToUse = previousCouponFileName;
                if (File.Exists(couponToUse))
                {

                    using (Image couponImage = Image.FromFile(couponToUse))
                    {
                        int width = ticketImage.Width;
                        int height = ticketImage.Height;

                        double proportion = (double)couponImage.Width / (double)couponImage.Height;
                        couponHeight = (int)((double)width / proportion);
                        //string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        //string pngfileName = timeStamp + ".png";
                        //string pngFilePath = Path.Combine(Settings.CurrentSettings.OutputPath, pngfileName);
                        newImage = new Bitmap(width, height + couponHeight);// couponImage.Height);

                        Graphics g = Graphics.FromImage(newImage);

                        g.Clear(Color.White);
                        g.DrawImage(ticketImage, 0, 0, ticketImage.Width, ticketImage.Height);

                        g.DrawImage(couponImage, 0, ticketImage.Height, ticketImage.Width, couponHeight);// couponImage.Height);

                        g.Dispose();
                    }
                }

                ticketImage.Dispose();
            }

            if (newImage != null)
            {
                newImage.Save(pngFile, ImageFormat.Png);
                receiptHeight = newImage.Height;
                newImage.Dispose();
            }

            //if (Settings.CurrentSettings.PoweredLogoEnabled)
            //{
            //    string destinationFile = Path.Combine(Settings.ConfigDirectory, "coupons");
            //    destinationFile = Path.Combine(destinationFile, "power.png");

            //    if (newImage != null)
            //    {

            //    }
            //    else
            //    {

            //    }

            //}

            GC.Collect();
        }
        //***********************************//
        private void AddPoweredImage(string pngFile, out int receiptHeight)
        {
            receiptHeight = 0;
            Bitmap newImage = null;
            int poweredHeight = 300;
            using (Image ticketImage = Image.FromFile(pngFile))
            {
                receiptHeight = ticketImage.Height;

                string poweredLogo = Path.Combine(Settings.ConfigDirectory, "coupons");
                poweredLogo = Path.Combine(poweredLogo, "power.png");

                if (File.Exists(poweredLogo))
                {
                    using (Image poweredImage = Image.FromFile(poweredLogo))
                    {
                        int width = ticketImage.Width;
                        int height = ticketImage.Height;

                        double proportion = (double)poweredImage.Width / (double)poweredImage.Height;
                        poweredHeight = (int)((double)width / proportion);
                        //poweredHeight = poweredImage.Height;

                        newImage = new Bitmap(width, height + poweredHeight);// couponImage.Height);

                        Graphics g = Graphics.FromImage(newImage);

                        g.Clear(Color.White);
                        g.DrawImage(ticketImage, 0, 0, ticketImage.Width, ticketImage.Height);

                        g.DrawImage(poweredImage, 0, ticketImage.Height, ticketImage.Width, poweredHeight);// couponImage.Height);

                        g.Dispose();
                        poweredImage.Dispose();
                    }
                }
                ticketImage.Dispose();
            }

            if (newImage != null)
            {
                newImage.Save(pngFile, ImageFormat.Png);
                receiptHeight = newImage.Height;
                newImage.Dispose();
            }
            GC.Collect();
        }
        //***********************************//
        private void AddBarcodes(string pngFile, byte[] receiptData, out int receiptHeight)
        {
            receiptHeight = 0;
            string dataHex = Converters.ByteArrayToHexString(receiptData).ToLower();

            dataHex = dataHex.Replace("0d0a", "0a");

            string[] dataLines = dataHex.Split(new string[] { "0a" }, StringSplitOptions.RemoveEmptyEntries);

            string barcodeContent = "";
            string qrCodeContent = "";
            foreach (string dataLine in dataLines)
            {
                if (dataLine.Contains("1d68") && dataLine.Contains("6b"))
                {
                    barcodeContent = dataLine.Substring(dataLine.IndexOf("6b") + 8);
                }
                if (dataLine.Contains("1d28") && dataLine.Contains("6b30"))
                {
                    int startIndex = dataLine.IndexOf("6b30") + 12;
                    int endIndex = dataLine.LastIndexOf("28")-2;
                    int length = endIndex - startIndex;
                    qrCodeContent = dataLine.Substring(startIndex, length);
                    
                }
            }

            if (!string.IsNullOrEmpty(barcodeContent))
                barcodeContent = Converters.HexStringToASCII(barcodeContent);

            if (!string.IsNullOrEmpty(qrCodeContent))
                qrCodeContent = Converters.HexStringToASCII(qrCodeContent);

            receiptHeight = 0;
            Bitmap newImage = null;
            int barcodeHeight = 100;
            int qrCodeHeight = 250;
            using (var ticketImage = Bitmap.FromFile(pngFile))
            {
                var trimmedTicket = ImageTrimWhite((Bitmap)ticketImage);

                receiptHeight = trimmedTicket.Height;

                int width = trimmedTicket.Width;
                int height = trimmedTicket.Height;

                if (!string.IsNullOrEmpty(qrCodeContent))
                    height += qrCodeHeight;

                if (!string.IsNullOrEmpty(barcodeContent))
                    height += barcodeHeight;

                newImage = new Bitmap(width, height+50);// couponImage.Height);

                Graphics g = Graphics.FromImage(newImage);

                g.Clear(Color.White);
                g.DrawImage(trimmedTicket, 0, 0, trimmedTicket.Width, trimmedTicket.Height);


                if (!string.IsNullOrEmpty(qrCodeContent))
                {
                    var qrBmp = GenerateQRcode(qrCodeContent);
                    g.DrawImage(qrBmp, trimmedTicket.Width / 2 - qrCodeHeight / 2, trimmedTicket.Height, qrCodeHeight, qrCodeHeight);// couponImage.Height);
                }

                if (!string.IsNullOrEmpty(barcodeContent))
                {
                    var barcodeBmp = GenerateBarcode(barcodeContent);
                    g.DrawImage(barcodeBmp, 50, trimmedTicket.Height+qrCodeHeight, trimmedTicket.Width-100, barcodeHeight);// couponImage.Height);
                }

                g.Dispose();
            }

            if (newImage != null)
            {
                newImage.Save(pngFile, ImageFormat.Png);
                receiptHeight = newImage.Height;
            }

            Console.WriteLine("");
        }
        //***********************************//
        private void AddCustomerInfo(string pngFile, string search, int ticketType, ref int receiptHeight)
        {
            if (search.Length <= 0)
                return;
            try
            {
              ContactService.ticketsValidaBeans searchParams = new ContactService.ticketsValidaBeans();
                searchParams.id_cliente = Settings.CurrentSettings.ClientID;

                searchParams.valor = search;

                ContactService.ws_ValidaDatos_ClieClient client = new ContactService.ws_ValidaDatos_ClieClient();
                var resp = client.Consulta(searchParams);

                int fontSize = 40;
                int lineHeight = 60;
                int offsetPix = 50;

                if (ticketType == 1)
                {
                    fontSize = 16;
                    lineHeight = 25;
                    offsetPix = 20;
                }

                if (resp.Length > 0)
                {
                    string name = resp[0].first_name + " " + resp[0].last_name;
                    string address = resp[0].primary_address_street;

                    Bitmap newImage = null;
                    using (var ticketImage = Bitmap.FromFile(pngFile))
                    {
                        int width = ticketImage.Width;
                        int height = ticketImage.Height;

                        newImage = new Bitmap(width, height + lineHeight * 7);

                        Graphics graphics = Graphics.FromImage(newImage);
                        graphics.Clear(Color.White);
                        graphics.DrawImage(ticketImage, 0, 0, ticketImage.Width, ticketImage.Height);

                        int startX = 25;
                        int startY = ticketImage.Height + 25;
                        int Offset = 5;


                        Font font = new Font("Consolas", fontSize);
                        var brush = new SolidBrush(Color.Black);

                        if (language.ToLower().Contains("es"))
                        {
                            graphics.DrawString("Informacion de cliente:", font, brush, startX, startY + Offset);
                            Offset = Offset + offsetPix;
                        }
                        else
                        {
                            graphics.DrawString("Customer Info:", font, brush, startX, startY + Offset);
                            Offset = Offset + offsetPix;
                        }

                        graphics.DrawString(name, font, brush, startX, startY + Offset);
                        Offset = Offset + offsetPix;

                        if (!string.IsNullOrEmpty(resp[0].primary_address_street))
                        {
                            graphics.DrawString(resp[0].primary_address_street, font, brush, startX, startY + Offset);
                            Offset = Offset + offsetPix;
                        }

                        if (!string.IsNullOrEmpty(resp[0].primary_address_city))
                        {
                            graphics.DrawString(resp[0].primary_address_city, font, brush, startX, startY + Offset);
                            Offset = Offset + offsetPix;
                        }

                        if (!string.IsNullOrEmpty(resp[0].primary_address_postalcode))
                        {
                            graphics.DrawString(resp[0].primary_address_postalcode, font, brush, startX, startY + Offset);
                            Offset = Offset + offsetPix;
                        }

                        if (!string.IsNullOrEmpty(resp[0].description))
                        {
                            graphics.DrawString(resp[0].description, font, brush, startX, startY + Offset);
                            Offset = Offset + offsetPix;
                        }

                        graphics.DrawString(search, font, brush, startX, startY + Offset);
                    }

                    if (newImage != null)
                    {
                        newImage.Save(pngFile, ImageFormat.Png);
                        receiptHeight = newImage.Height;
                        newImage.Dispose();
                    }
                }
                else
                {
                    Bitmap newImage = null;
                    using (var ticketImage = Bitmap.FromFile(pngFile))
                    {
                        int width = ticketImage.Width;
                        int height = ticketImage.Height;

                        newImage = new Bitmap(width, height + lineHeight * 3);

                        Graphics graphics = Graphics.FromImage(newImage);
                        graphics.Clear(Color.White);
                        graphics.DrawImage(ticketImage, 0, 0, ticketImage.Width, ticketImage.Height);

                        int startX = 25;
                        int startY = ticketImage.Height + 25;
                        int Offset = 5;

                        Font font = new Font("Consolas", fontSize);
                        var brush = new SolidBrush(Color.Black);

                        if (language.ToLower().Contains("es"))
                        {
                            graphics.DrawString("Informacion de cliente:", font, brush, startX, startY + Offset);
                            Offset = Offset + offsetPix;
                        }
                        else
                        {
                            graphics.DrawString("Customer Info:", font, brush, startX, startY + Offset);
                            Offset = Offset + offsetPix;
                        }

                        graphics.DrawString(search, font, brush, startX, startY + Offset);

                        ticketImage.Dispose();
                    }

                    if (newImage != null)
                    {
                        newImage.Save(pngFile, ImageFormat.Png);
                        receiptHeight = newImage.Height;
                        newImage.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }

            GC.Collect();
        }
        //***********************************//
        private string GetCustomerInfo(string search)
        {
            string customerInfo = "";

            if (search.Length <= 0)
                return customerInfo;
            try
            {
                ContactService.ticketsValidaBeans searchParams = new ContactService.ticketsValidaBeans();
                searchParams.id_cliente = Settings.CurrentSettings.ClientID;

                searchParams.valor = search;

                ContactService.ws_ValidaDatos_ClieClient client = new ContactService.ws_ValidaDatos_ClieClient();
                var resp = client.Consulta(searchParams);

                
                if (resp.Length > 0)
                {
                    string name = resp[0].first_name + " " + resp[0].last_name;

                    if (language.ToLower().Contains("es"))
                    {
                        customerInfo += "Informacion de cliente:";
                    }
                    else
                    {
                        customerInfo += "Customer Info:" + "\n";
                    }

                    customerInfo += name + "\n";

                    if (!string.IsNullOrEmpty(resp[0].primary_address_street))
                    {
                        customerInfo += resp[0].primary_address_street + "\n";
                    }

                    if (!string.IsNullOrEmpty(resp[0].primary_address_city))
                    {
                        customerInfo += resp[0].primary_address_city + "\n";
                    }

                    if (!string.IsNullOrEmpty(resp[0].primary_address_postalcode))
                    {
                        customerInfo += resp[0].primary_address_postalcode + "\n";
                    }

                    if (!string.IsNullOrEmpty(resp[0].description))
                    {
                        customerInfo += resp[0].description + "\n";
                    }
                }
                else
                {
                    if (language.ToLower().Contains("es"))
                    {
                        customerInfo += "Informacion de cliente:";
                    }
                    else
                    {
                        customerInfo += "Customer Info:" + "\n";
                    }

                    customerInfo += search + "\n";
                }
            }
            catch (Exception)
            {
            }

            return customerInfo;
        }
        //***********************************//
        private bool CombinePNGPages(List<string> pngPages, string pngFileName)
        {
            bool result = false;

            try
            {
                int width = 0;
                int totalHeight = 0;

                foreach (string pngPage in pngPages)
                {
                    try
                    {
                        using (Image pngPageImg = Image.FromFile(pngPage))
                        {
                            using (Bitmap origPageImg = new Bitmap(pngPageImg))
                            {
                                var trimmedPngPageImg = ImageTrimWhite(origPageImg);

                                origPageImg.Dispose();
                                
                                if (trimmedPngPageImg == null)
                                    continue;

                                if (trimmedPngPageImg.Width > width)
                                    width = trimmedPngPageImg.Width;

                                totalHeight += trimmedPngPageImg.Height;

                                trimmedPngPageImg.Dispose();
                            }
                            GC.Collect();
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateLogBox("in TrimPage: " + ex.Message);
                    }
                }

                GC.Collect();

                Bitmap newImage = new Bitmap(width, totalHeight);

                Graphics g = Graphics.FromImage(newImage);

                g.Clear(Color.White);

                int yLocation = 0;
                foreach (string pngPage in pngPages)
                {
                    try
                    {
                        using (Bitmap pngPageImg = new Bitmap(pngPage))
                        {
                            //using (Bitmap origPageImg = new Bitmap(pngPageImg))
                            {

                                var trimmedPngPageImg = ImageTrimWhite(pngPageImg);
                                if (trimmedPngPageImg == null)
                                    continue;

                                //g.DrawImage(pngPageImg, 0, yLocation, pngPageImg.Width, pngPageImg.Height);
                                g.DrawImage(trimmedPngPageImg, 0, yLocation, width, trimmedPngPageImg.Height);
                                yLocation += trimmedPngPageImg.Height;

                                pngPageImg.Dispose();
                                trimmedPngPageImg.Dispose();
                            }
                            GC.Collect();
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateLogBox("in CombinePage: " + ex.Message);
                    }
                    finally
                    {
                        GC.Collect();
                    }
                }

                //foreach (string pngPage in pngPages)
                //{
                //    File.Delete(pngPage);
                //}

                g.Dispose();

                newImage.Save(Path.Combine(Settings.CurrentSettings.OutputPath, pngFileName), ImageFormat.Png);
                newImage.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                UpdateLogBox("CombinePNGPages(): " + ex.Message);
            }

            GC.Collect();
            return result;
        }
        //***********************************//
        public static Bitmap ImageTrimWhite(Bitmap img)
        {
            
            //get image data
            BitmapData bd = img.LockBits(new Rectangle(Point.Empty, img.Size),
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int[] rgbValues = new int[img.Height * img.Width];
            Marshal.Copy(bd.Scan0, rgbValues, 0, rgbValues.Length);
            img.UnlockBits(bd);

#region determine bounds
            int left = bd.Width;
            int top = bd.Height;
            int right = 0;
            int bottom = 0;

            //determine top
            for (int i = 0; i < rgbValues.Length; i++)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    top = r;
                    break;
                }
            }

            //determine bottom
            for (int i = rgbValues.Length - 1; i >= 0; i--)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    break;
                }
            }

            if (bottom > top)
            {
                for (int r = top + 1; r < bottom; r++)
                {
                    //determine left
                    for (int c = 0; c < left; c++)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (left > c)
                            {
                                left = c;
                                break;
                            }
                        }
                    }

                    //determine right
                    for (int c = bd.Width - 1; c > right; c--)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (right < c)
                            {
                                right = c;
                                break;
                            }
                        }
                    }
                }
            }

            //int width = right - left + 1;//original
            int width = right - left + Settings.CurrentSettings.RightMargin;
            int height = bottom - top + 1 + 25;
#endregion

            //copy image data
            int[] imgData = new int[width * height];
            for (int r = top; r <= bottom; r++)
            {
                Array.Copy(rgbValues, r * bd.Width + left, imgData, (r - top) * width, width);
            }

            if (width <= 0 || height <= 0)
                return null;

            //create new image
            img.Dispose();

            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData nbd
                = newImage.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(imgData, 0, nbd.Scan0, imgData.Length);
            newImage.UnlockBits(nbd);

            return newImage;
        }
        //***********************************//
        private int GetPageCount(string psFile)
        {
            int pageCount = 0;

            try
            {
                // Read the file line by line.  
                using (StreamReader file = new StreamReader(psFile))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Contains("%%Pages: ") && !line.Contains("atend"))
                        {
                            line = line.Replace("%%Pages: ", "");
                            pageCount = Convert.ToInt32(line);
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return pageCount;
        }
        //***********************************//
        private bool WriteTextToPng(string[] ticketLines, string pngFile)
        {
            
            bool returnVal = false;
            string ticketText = "";

            try
            {
                if (ticketLines.Length <= 0)
                    return false;

                int width = 800;
                int height = 28 * ticketLines.Length;// linesToWriteOnImage.Count;
                Bitmap ticketImage = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(ticketImage);
                graphics.Clear(Color.White);

                int startX = 10;
                int startY = 15;
                int Offset = 5;

                foreach (string line in ticketLines)
                {
                    string textLine = line;

                    ticketText += line;

                   

                    if (Settings.CurrentSettings.PosType == POSTypes.Aloha)
                    {
                        if (textLine.Replace(" ", "").Length <= 1)
                            continue;
                    }
                    graphics.DrawString(textLine, new Font("Lucida Console", 16), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset = Offset + 25;
                }



                var trimmedImage = ImageTrimWhite(ticketImage);

                ticketImage.Dispose();

                trimmedImage.Save(pngFile, ImageFormat.Png);

                //ticketImage.Save(pngFile, ImageFormat.Png);

                returnVal = true;
                trimmedImage.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                UpdateLogBox("WriteTextToPng(): " + ex.Message);
            }
           
           


            return returnVal;
        }
        //***********************************//
        private static string[] GetESCPOSText(string filePath)
        {
            string[] ticketLines = new string[0];

            string ticketText = "";// ParseTicketString(ticketBytes);

            EscParser parser = new SnailDev.EscPosParser.EscParser();
            var commands = parser.GetCommands(filePath);

           

            foreach (var command in commands)
            {
               
                if (command.IsAvailableAs("ITextContainer"))
                {
                    string textLine = (command as TextCommand).GetContent();

                    textLine = Regex.Replace(textLine, @"[^\u0020-\u00FE]+", string.Empty);
                   

                    if (Settings.CurrentSettings.PosType == POSTypes.Aloha)
                    {
                        if (textLine.Contains((char)0x00))
                            textLine = textLine.Substring(textLine.LastIndexOf((char)0x00));
                        textLine = textLine.Replace("!!E", "");
                        textLine = textLine.Replace("!E", "");

                        if (textLine.Contains("$ì*!"))
                            continue;
                        if (textLine.Replace(" ", "").Length <= 1)
                            continue;
                    }
                    ticketText += textLine;
                }
                if (command.IsAvailableAs("ILineBreak"))
                {
                    ticketText += Environment.NewLine;
                }
            }

            if (ticketText.Length > 0)
            {
                ticketLines = ticketText.Split('\n');

                for (int i = 0; i < ticketLines.Length; i++)
                {
                    if (ticketLines[i].Contains((char)196))
                        ticketLines[i] = ticketLines[i].Replace((char)196, '=');
                }
            }

            
            return ticketLines;
        }
        //***********************************//
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        //***********************************//
        private void TestButton_Click(object sender, EventArgs e)
        {
            try
            {
                //GenerateBarcode("BT923000001");
                //GenerateQRcode("https://cw.pos.mx/factura/mumuso/XDTQZWPMXCH");

                //List<byte> bytes = new List<byte>();
                //for (int i = 0; i < 1000; i++)
                //{
                //    bytes.Add(0x1B);
                //    bytes.Add(0x76);
                //}

                //File.WriteAllBytes("1B76_1000", bytes.ToArray());
                string response = File.ReadAllText(@"C:\Users\zeeshan\Desktop\codiStatus.json");

                JObject responseObj = (JObject)JsonConvert.DeserializeObject(response);
                bool retVal = false;
                string status;
                string idcodi;

                if (responseObj != null)
                {
                    if (responseObj.ContainsKey("codigoResultado"))
                    {
                        status = responseObj["codigoResultado"].ToString();
                        retVal = true;
                    }
                    if (responseObj.ContainsKey("listQR"))
                    {
                        if(responseObj["listQR"].Count() > 0)
                        {
                            idcodi = responseObj["listQR"][0]["idTransaccion"].ToString();
                            retVal = true;
                        }
                        //JObject mcQRObj = (JObject)JsonConvert.DeserializeObject(mcQRjson);

                        //string QRjson = mcQRObj["ic"].ToString();
                        //idcodi = responseObj["idcodi"].ToString();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateLogBox(ex.Message);
            }
        }
        //***********************************//
        private Bitmap GenerateBarcode(string content)
        {
            var barcodeWriter = new BarcodeWriter();

            // set the barcode format
            barcodeWriter.Format = BarcodeFormat.CODE_39;

            // write text and generate a 1-D barcode as a bitmap
            var bitmap = barcodeWriter.Write(content);

            string name = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            //bitmap.Save("D:\\Edu\\VS Projects\\Print Skyticket v2\\jobs\\" + name + ".bmp");



            return bitmap;
        }
        //***********************************//
        private Bitmap GenerateQRcode(string content)
        {
            var barcodeWriter = new BarcodeWriter();

            // set the barcode format
            barcodeWriter.Format = BarcodeFormat.QR_CODE;

            // write text and generate a 2-D barcode as a bitmap
            var bitmap = barcodeWriter.Write(content);

            string name = DateTime.Now.ToString("yyyyMMddHHmmss");

            //bitmap.Save("D:\\Edu\\VS Projects\\Print Skyticket v2\\jobs\\" + name + ".bmp");

            return bitmap;
        }
        //***********************************//
        private void RemoveWhiteLines(string strSourcePath, string strDestinePath)
        {
            string[] strAllLines = File.ReadAllLines(strSourcePath);
            //Selecciona las lineas que no sean null o blancas
            //Guarda las nuevas lineas en el nuevo fichero
           
            File.WriteAllLines(strDestinePath, strAllLines.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());


        }

        private static bool TicketRequest(Ticket ti)
        {
            
            bool result = false;
            try
            {
                UpdateLogBox("TicketReq1");
                var ticket = new RestClient("https://skyticketapi.azurewebsites.net/");
                ticket.Timeout = -1;
                var request = new RestRequest("tickets", Method.POST);
                request.AddJsonBody(ti);

                IRestResponse response = ticket.Execute(request);

                var ticketr = JsonConvert.DeserializeObject<TicketRes>(response.Content);

                id_ticketr = ticketr.ticket.id;
                UpdateLogBox("" + ticketr.ticket.id);

                
                if (id_ticketr != 0)
                    result = true;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "modificacion proceso lealtad");
            }

            return result;

        }

        public static void FeedRequest( FeedInfo feed)
        {
            var feedback = new RestClient("https://skyticketapi.azurewebsites.net/");
            feedback.Timeout = -1;
            var request = new RestRequest("feedback", Method.POST);
            request.AddJsonBody(feed);

            IRestResponse response = feedback.Execute(request);
        }


        public void GetUsers()
        {

            var client = new RestClient("https://skyticketapi.azurewebsites.net/supera/users");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);



            UsersList = JsonConvert.DeserializeObject<List<SuperaUsers>>(response.Content);

           


        }


    }
}
