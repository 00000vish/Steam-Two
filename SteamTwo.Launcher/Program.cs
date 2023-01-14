// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");


void checkArgs()
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
void deleteRegistryKey()
{
    Microsoft.Win32.RegistryKey regKey = default(Microsoft.Win32.RegistryKey);
    regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
    try
    {
        regKey.DeleteValue("Steam Two", true);
    }
    catch (Exception) { }
    regKey.Close();
}

//create auto start reg key
void createRegistryKey()
{
    Microsoft.Win32.RegistryKey regKey = default(Microsoft.Win32.RegistryKey);
    regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
    try
    {
        string KeyName = "Steam Two";
        string KeyValue = Environment.CurrentDirectory + "\\SteamTwo Launcher.exe";
        regKey.SetValue(KeyName, KeyValue, Microsoft.Win32.RegistryValueKind.String);
    }
    catch (Exception e) { 
    }
    regKey.Close();
}
//start the main app
void luanchSteamTwo()
{
    string exepath = AppDomain.CurrentDomain.BaseDirectory + "\\Steam Two.exe";
    ProcessStartInfo psi = new ProcessStartInfo();
    psi.FileName = exepath;
    psi.WorkingDirectory = Path.GetDirectoryName(exepath);
    psi.Arguments = "startup";
    Process.Start(psi);
}