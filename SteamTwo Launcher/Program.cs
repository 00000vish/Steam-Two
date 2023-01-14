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

       
    }
}
