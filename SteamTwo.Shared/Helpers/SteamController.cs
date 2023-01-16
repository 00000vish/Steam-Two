using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTwo.Shared.Helpers
{
    public class SteamController
    {
        public static bool StartSteam(string username, string password)
        {
            StopSteam();
            if (CheckForSteam())
            {
                System.Threading.Thread.Sleep(2000);
                Process proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        //FileName = SteamTwoProperties.jsonSetting.steamLocation,
                        Arguments = "-login " + username + " " + password,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                return true;
            }
            return false;
        }

        //finds steam in disk
        private static bool CheckForSteam()
        {
            //if (!System.IO.File.Exists(SteamTwoProperties.jsonSetting.steamLocation))
            //{
            //    new SteamTwo.Settings().Show("find steam"); // if not ask to locate it
            //    return false;
            //}
            return true;
        }

        //kills steam
        private static void StopSteam()
        {
            Process[] proc = Process.GetProcesses();
            foreach (Process item in proc)
            {
                if (item.ProcessName.ToLower().Equals("steam") || item.ProcessName.Equals("gameOverlayui"))
                {
                    item.Kill();
                }
            }
        }
    }
}
