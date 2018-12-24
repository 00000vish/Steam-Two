using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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
            try
            {
                //since cant detect if user open or started with windows
                //this app will only open with windows and opens app with -startup let main know its open with windowws
                checkArgs();
            }
            catch (Exception D) { MessageBox.Show(D.ToString());}           
            Environment.Exit(0);
        }

        private static void checkArgs()
        {
            String args = "";
            try
            {
                args = Environment.GetCommandLineArgs()[1];
            }
            catch (Exception) { }

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

        //delte auto start reg key
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

        //create auto start reg key
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
        //start the main app
        private static void luanchSteamTwo()
        {
            string exepath = AppDomain.CurrentDomain.BaseDirectory + "\\Steam Two.exe";
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = exepath;           
            psi.WorkingDirectory = Path.GetDirectoryName(exepath);
            psi.Arguments = "startup";
            Process.Start(psi);          
        }
    }
}
