using System;
using System.Windows;

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
            updateGUI();
            Show();
        }

        private void updateGUI()
        {
            badAttempt.IsChecked = Properties.Settings.Default.badAttemptSetting;
            multipleBots.IsChecked = Properties.Settings.Default.multipleBotSetting;
            copyPassword.IsChecked = Properties.Settings.Default.copyPasswordSetting;
            closeStemLaunch.IsChecked = Properties.Settings.Default.closeStemLaunchSetting;
            autoAddFriends.IsChecked = Properties.Settings.Default.autoAddFriend;
            enableEncryption.IsChecked = Properties.Settings.Default.encrypted;
            changeKey.IsEnabled = Properties.Settings.Default.encrypted;
            chatCommand.IsEnabled = Properties.Settings.Default.chatComSettings;
            chatCommandButton.IsChecked = Properties.Settings.Default.chatComSettings;
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

        private void settingsChanged(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.encrypted == false)
            {
                Properties.Settings.Default.encrypted = (bool)enableEncryption.IsChecked;
                if (Properties.Settings.Default.encrypted == true)
                {
                    changeKeyClicked();

                }
            }
            else
            {
                Properties.Settings.Default.encrypted = (bool)enableEncryption.IsChecked;
            }
            Properties.Settings.Default.badAttemptSetting = (bool)badAttempt.IsChecked;
            Properties.Settings.Default.multipleBotSetting = (bool)multipleBots.IsChecked;
            Properties.Settings.Default.copyPasswordSetting = (bool)copyPassword.IsChecked;
            Properties.Settings.Default.closeStemLaunchSetting = (bool)closeStemLaunch.IsChecked;
            Properties.Settings.Default.chatComSettings = (bool)chatCommandButton.IsChecked;
            Properties.Settings.Default.autoAddFriend = (bool)autoAddFriends.IsChecked;
            Properties.Settings.Default.Save();
            updateGUI();           
        }
    }
}
