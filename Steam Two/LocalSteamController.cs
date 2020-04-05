using System;
using System.Diagnostics;

namespace SteamTwo
{
    class LocalSteamController
    {
        //starts steam
        public static bool startSteam(String username, String password)
        {
            killSteam();
            if (checkForSteam())
            {
                System.Threading.Thread.Sleep(2000);
                Process proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = SteamTwoProperties.jsonSetting.steamLocation,
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
        private static bool checkForSteam()
        {
            if(!System.IO.File.Exists(SteamTwoProperties.jsonSetting.steamLocation)){
                new SteamTwo.Settings().Show("find steam"); // if not ask to locate it
                return false;
            }
            return true;
        }

        //kills steam
        private  static void killSteam()
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
