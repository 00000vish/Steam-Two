using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

class SettingJson
{
    public bool encryptedSetting { get; set; }
    public bool autoAddFriendSetting { get; set; }
    public bool chatComSetting { get; set; }
    public String encryptedKeySetting { get; set; }
    public bool closeStemLaunchSetting { get; set; }
    public bool copyPasswordSetting { get; set; }
    public bool multipleBotSetting { get; set; }
    public bool badAttemptSetting { get; set; }
    public bool chatSetting { get; set; }
    public bool autoStartSetting { get; set; }
    public bool autoLoginSetting { get; set; }
    public bool notifyOnMessage { get; set; }
}

static class SteamTwoProperties
{
    private const String SETTING_FILE = "SteamTwoSetting.config";
    public static SettingJson jsonSetting = null;

    //init settings
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

    //reset resstings
    public static void reset()
    {
        jsonSetting = new SettingJson()
        {
            encryptedSetting = false,
            autoAddFriendSetting = false,
            chatComSetting = false,
            encryptedKeySetting = "FteUuLPNgH2K7YjGhHbPGw==",
            closeStemLaunchSetting = false,
            copyPasswordSetting = false,
            multipleBotSetting = false,
            badAttemptSetting = true,
            chatSetting = false,
            autoStartSetting = false,
            autoLoginSetting = false,
            notifyOnMessage = false
        };
    }

    //update seetting file
    public static void updateSettingFile()
    {
        string json = JsonConvert.SerializeObject(jsonSetting);
        System.IO.File.WriteAllText(SETTING_FILE, json);
    }

    //read settings from setting files
    public static void readSettingFile()
    {
        try
        {
            var JsonData = JsonConvert.DeserializeObject<SettingJson>(File.ReadAllText(SETTING_FILE));
            jsonSetting = JsonData;
        }
        catch (Exception)
        {
            SteamTwoProperties.reset();
            SteamTwoProperties.updateSettingFile();
        }
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

        //updates gui according to currentsettings
        private void updateGUI()
        {
            enableChat.IsChecked = SteamTwoProperties.jsonSetting.chatSetting;
            autoStart.IsChecked = SteamTwoProperties.jsonSetting.autoStartSetting;
            autoLogin.IsChecked = SteamTwoProperties.jsonSetting.autoLoginSetting;
            badAttempt.IsChecked = SteamTwoProperties.jsonSetting.badAttemptSetting;
            multipleBots.IsChecked = SteamTwoProperties.jsonSetting.multipleBotSetting;
            copyPassword.IsChecked = SteamTwoProperties.jsonSetting.copyPasswordSetting;
            closeStemLaunch.IsChecked = SteamTwoProperties.jsonSetting.closeStemLaunchSetting;
            autoAddFriends.IsChecked = SteamTwoProperties.jsonSetting.autoAddFriendSetting;
            enableEncryption.IsChecked = SteamTwoProperties.jsonSetting.encryptedSetting;
            changeKey.IsEnabled = SteamTwoProperties.jsonSetting.encryptedSetting;
            chatCommand.IsEnabled = SteamTwoProperties.jsonSetting.chatComSetting;
            chatCommandButton.IsChecked = SteamTwoProperties.jsonSetting.chatComSetting;
            notifyOnMessage.IsChecked = SteamTwoProperties.jsonSetting.notifyOnMessage;
        }

        //change passkey
        private void changeKey_Click(object sender, RoutedEventArgs e)
        {
            changeKeyClicked();
        }

        //change pass key button pressed
        private void changeKeyClicked()
        {
            GetInput GI = new GetInput();
            String temp = GI.Show("Encryption", "Please enter the passkey below", false);
            GI.Close();
            MainWindow.setEncryptionKey(temp);
            MainWindow.writeAccountData();

        }     

        //when settings changed
        private void settingsChanged(object sender, RoutedEventArgs e)
        {

            CheckBox item = (CheckBox)sender;
            //encryption

            SteamTwoProperties.jsonSetting.encryptedSetting = (bool)enableEncryption.IsChecked;
            if (SteamTwoProperties.jsonSetting.encryptedSetting == true && item.Name.ToString().Equals("enableEncryption"))
            {
                changeKeyClicked();
            }           

            //auto start
            SteamTwoProperties.jsonSetting.autoStartSetting = (bool)autoStart.IsChecked;
            if (SteamTwoProperties.jsonSetting.autoStartSetting == true && item.Name.ToString().Equals("autoStart"))
            {
                createRegKey();
            }
            else if (SteamTwoProperties.jsonSetting.autoStartSetting != true && item.Name.ToString().Equals("autoStart"))
            {
                deleteRegKey();
            }

            SteamTwoProperties.jsonSetting.autoLoginSetting = (bool)autoLogin.IsChecked;
            SteamTwoProperties.jsonSetting.badAttemptSetting = (bool)badAttempt.IsChecked;
            SteamTwoProperties.jsonSetting.multipleBotSetting = (bool)multipleBots.IsChecked;
            SteamTwoProperties.jsonSetting.copyPasswordSetting = (bool)copyPassword.IsChecked;
            SteamTwoProperties.jsonSetting.closeStemLaunchSetting = (bool)closeStemLaunch.IsChecked;
            SteamTwoProperties.jsonSetting.chatComSetting = (bool)chatCommandButton.IsChecked;
            SteamTwoProperties.jsonSetting.chatSetting = (bool)enableChat.IsChecked;
            SteamTwoProperties.jsonSetting.autoAddFriendSetting = (bool)autoAddFriends.IsChecked;
            SteamTwoProperties.jsonSetting.notifyOnMessage = (bool)notifyOnMessage.IsChecked;
            SteamTwoProperties.updateSettingFile();
            updateGUI();
        }

        //open launcher and enabled auto start
        private void createRegKey()
        {
            using (Process p = new Process())
            {
                p.StartInfo = new ProcessStartInfo { FileName = "SteamTwo Launcher.exe", Arguments = "on" };
                p.Start();
            }
        }

        //closes launcher and disable auto start
        private void deleteRegKey()
        {
            using (Process p = new Process())
            {
                p.StartInfo = new ProcessStartInfo { FileName = "SteamTwo Launcher.exe", Arguments = "off" };
                p.Start();
            }
        }

        //reset settings
        private void resetSettings_Click(object sender, RoutedEventArgs e)
        {
            SteamTwoProperties.reset();
            SteamTwoProperties.updateSettingFile();
            updateGUI();
        }
    }
}
