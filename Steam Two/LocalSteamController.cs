using System;
using System.Diagnostics;

namespace SteamTwo
{
    class LocalSteamController
    {

        public static void startSteam(String username, String password)
        {
            killSteam();
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

        public static void killSteam()
        {
            Process.Start("taskkill", "/F /IM GameOverlayUI.exe");
            Process.Start("taskkill", "/F /IM Steam.exe");
            System.Threading.Thread.Sleep(2000);
        }
    }
}
