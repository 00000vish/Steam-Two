using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamTwo_Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            checkArgs();
            Environment.Exit(0);
        }

        private static void checkArgs()
        {
            String args = Environment.GetCommandLineArgs().ToString();
            switch (args)
            {
                case "on":
                    createRegistryKey();
                    break;
                case "off":
                    deleteRegistryKey();
                    break;
                default:
                    luanchSteamTwo();
                    break;
            }
        }

        private static void deleteRegistryKey()
        {
            Microsoft.Win32.RegistryKey regKey = default(Microsoft.Win32.RegistryKey);
            regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            try
            {
                regKey.DeleteValue("Steam Two", true);
            }
            catch (Exception) { }
            Properties.Settings.Default.Save();
            regKey.Close();
        }

        private static void createRegistryKey()
        {
            Microsoft.Win32.RegistryKey regKey = default(Microsoft.Win32.RegistryKey);
            regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            try
            {
                string KeyName = "Steam Two";
                string KeyValue = Environment.CurrentDirectory + "\\SteamTwo Launcher.exe";
                regKey.SetValue(KeyName, KeyValue, Microsoft.Win32.RegistryValueKind.String);
            }
            catch (Exception e) { System.Windows.Forms.MessageBox.Show(e.ToString()); }
            Properties.Settings.Default.Save();
            regKey.Close();
        }

        private static void luanchSteamTwo()
        {
            using (Process p = new Process())
            {
                p.StartInfo = new ProcessStartInfo { FileName = "\\Steam Two.exe", Arguments= "startup"};
                p.Start();
            }
        }
    }
}
