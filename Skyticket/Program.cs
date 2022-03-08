using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Skyticket
{
    static class Program
    {
        internal static string language = "";
        static Mutex mutex = new Mutex(true, "{948C0702-815A-4F19-8CAC-88863721D696}");
        internal static List<string> languagesPath = new List<string>() { "es-ES" };
        public static bool isActivated = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Settings.LoadSettings();
            if (!string.IsNullOrEmpty(Settings.CurrentSettings.Language))
            {
                language = Settings.CurrentSettings.Language.Split('|')[1].Replace(" ", "");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }

            if (args.Length > 0)
            {
                if (args[0].ToLower().Contains("-u"))
                {
                    string originFilePath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "");
                    //MessageBox.Show("Update called");
                    Thread.Sleep(2000);
                    Updater.UpdateSelf();

                    //start with -s
                    Updater.ExecuteSelf(originFilePath, "-s");

                    Environment.Exit(0);
                }
                if (args[0].ToLower().Contains("-s"))
                {
                    //MessageBox.Show("Start called");
                    Thread.Sleep(2000);
                }
                if (args[0].ToLower().Contains("-d"))
                {
                    Thread.Sleep(3000);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.Run(new MainForm());
            }
            else
            {
                if (!args[0].ToLower().Contains("-q"))
                {
                    if (language.Contains("es"))
                        MessageBox.Show(TextsSpanish.OneInstance, "Skyticket");
                    else
                        MessageBox.Show(Texts.OneInstance, "Skyticket");
                }
            }
        }
    }
}
