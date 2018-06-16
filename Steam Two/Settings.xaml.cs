using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;


class SettingJson
{
    public bool encryptedSetting { get; set; }
    public bool autoAddFriendSetting { get; set; }
    public bool chatComSetting { get; set; }
    public String encryptedKeySetting { get; set; }
    public bool closeStemLaunchSetting { get; set; }
    public bool alwayRunSetting { get; set; }
    public bool copyPasswordSetting { get; set; }
    public bool multipleBotSetting { get; set; }
    public bool badAttemptSetting { get; set; }
    public bool chatSetting { get; set; }
    public bool autoStartSetting { get; set; }
}

static class SteamTwoProperties
{
    private const String SETTING_FILE = "SteamTwoSetting.config";
    public static SettingJson jsonSetting = null;

    static SteamTwoProperties()
    {
        if (File.Exists(SETTING_FILE))
        {
            try
            {
                readSettingFile();
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Error reading settings, settings have been reset");
                reset();
                updateSettingFile();
            }
        }
        else
        {
            reset();
            updateSettingFile();
        }
    }

    public static void reset()
    {
        jsonSetting = new SettingJson()
        {
            encryptedSetting = false,
            autoAddFriendSetting = false,
            chatComSetting = false,
            encryptedKeySetting = "FteUuLPNgH2K7YjGhHbPGw==",
            closeStemLaunchSetting = false,
            alwayRunSetting = false,
            copyPasswordSetting = false,
            multipleBotSetting = false,
            badAttemptSetting = true,
            chatSetting = false,
            autoStartSetting = false
        };
    }

    public static void updateSettingFile()
    {
        string json = JsonConvert.SerializeObject(jsonSetting);
        System.IO.File.WriteAllText(SETTING_FILE, json);
    }

    public static void readSettingFile()
    {
        var JsonData = JsonConvert.DeserializeObject<SettingJson>(File.ReadAllText(SETTING_FILE));
        jsonSetting = JsonData;
    }
}

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings
    {


        public Settings()
        {
            InitializeComponent();
        }

        public void Show(String ignore)
        {
            Show();
            updateGUI();            
        }

        private void updateGUI()
        {
            autoStart.IsChecked = SteamTwoProperties.jsonSetting.autoStartSetting;
            badAttempt.IsChecked = SteamTwoProperties.jsonSetting.badAttemptSetting;
            multipleBots.IsChecked = SteamTwoProperties.jsonSetting.multipleBotSetting;
            copyPassword.IsChecked = SteamTwoProperties.jsonSetting.copyPasswordSetting;
            closeStemLaunch.IsChecked = SteamTwoProperties.jsonSetting.closeStemLaunchSetting;
            autoAddFriends.IsChecked = SteamTwoProperties.jsonSetting.autoAddFriendSetting;
            enableEncryption.IsChecked = SteamTwoProperties.jsonSetting.encryptedSetting;
            changeKey.IsEnabled = SteamTwoProperties.jsonSetting.encryptedSetting;
            chatCommand.IsEnabled = SteamTwoProperties.jsonSetting.chatComSetting;
            chatCommandButton.IsChecked = SteamTwoProperties.jsonSetting.chatComSetting;
        }

        private void changeKey_Click(object sender, RoutedEventArgs e)
        {
            changeKeyClicked();
        }

        private void changeKeyClicked()
        {
            GetInput GI = new GetInput();
            String temp = GI.Show("Encryption", "Please enter the passkey below", false);
            GI.Close();
            MainWindow.setEncryptionKey(temp);
            MainWindow.writeAccountData();

        }

        private void createRegKey()
        {
            Microsoft.Win32.RegistryKey regKey = default(Microsoft.Win32.RegistryKey);
            regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);           
            try
            {
                string KeyName = "Steam Two";
                string KeyValue = Environment.CurrentDirectory + "\\Steam Two.exe";
                regKey.SetValue(KeyName, KeyValue, Microsoft.Win32.RegistryValueKind.String);
            }
            catch (Exception e) { System.Windows.Forms.MessageBox.Show(e.ToString()); }
            Properties.Settings.Default.Save();
            regKey.Close();
        }

        private void deleteRegKey()
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

        private void settingsChanged(object sender, RoutedEventArgs e)
        {
            //encryption
            if (SteamTwoProperties.jsonSetting.encryptedSetting == false)
            {
                SteamTwoProperties.jsonSetting.encryptedSetting = (bool)enableEncryption.IsChecked;
                if (SteamTwoProperties.jsonSetting.encryptedSetting == true)
                {
                    changeKeyClicked();
                }
            }
            else
            {
                SteamTwoProperties.jsonSetting.encryptedSetting = (bool)enableEncryption.IsChecked;
            }

            //auto start
            if(SteamTwoProperties.jsonSetting.autoStartSetting == false)
            {               
                SteamTwoProperties.jsonSetting.autoStartSetting = (bool)autoStart.IsChecked;
                if (SteamTwoProperties.jsonSetting.autoStartSetting == true)
                {
                    createRegKey();
                }
            }
            else
            {
                SteamTwoProperties.jsonSetting.autoStartSetting = (bool)autoStart.IsChecked;
                deleteRegKey();
            }


            SteamTwoProperties.jsonSetting.badAttemptSetting = (bool)badAttempt.IsChecked;
            SteamTwoProperties.jsonSetting.multipleBotSetting = (bool)multipleBots.IsChecked;
            SteamTwoProperties.jsonSetting.copyPasswordSetting = (bool)copyPassword.IsChecked;
            SteamTwoProperties.jsonSetting.closeStemLaunchSetting = (bool)closeStemLaunch.IsChecked;
            SteamTwoProperties.jsonSetting.chatComSetting = (bool)chatCommandButton.IsChecked;
            SteamTwoProperties.jsonSetting.autoAddFriendSetting = (bool)autoAddFriends.IsChecked;
            SteamTwoProperties.updateSettingFile();
            updateGUI();
        }

        private void resetSettings_Click(object sender, RoutedEventArgs e)
        {
            SteamTwoProperties.reset();
            SteamTwoProperties.updateSettingFile();
            updateGUI();
        }
    }
}
