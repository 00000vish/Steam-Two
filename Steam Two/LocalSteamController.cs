using System;
using System.Diagnostics;

namespace SteamTwo
{
    class LocalSteamController
    {
        //starts steam
        public static void startSteam(String username, String password)
        {
            killSteam();
            System.Threading.Thread.Sleep(2000);
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "E:\\Program Files (x86)\\Steam\\Steam.exe",
                    Arguments = "-login " + username + " " + password,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
        }

        //kills steam
        private  static void killSteam()
        {
            Process[] proc = Process.GetProcesses();
            foreach (Process item in proc)
            {
                if (item.ProcessName.Equals("Steam") || item.ProcessName.Equals("GameOverlayUI"))
                {
                    item.Kill();
                }
            }
        }
    }
}
