﻿using System;
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
            autoAddFriends.IsChecked = Properties.Settings.Default.autoAddFriendSetting;
            enableEncryption.IsChecked = Properties.Settings.Default.encryptedSetting;
            changeKey.IsEnabled = Properties.Settings.Default.encryptedSetting;
            chatCommand.IsEnabled = Properties.Settings.Default.chatComSetting;
            chatCommandButton.IsChecked = Properties.Settings.Default.chatComSetting;
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
            if (Properties.Settings.Default.encryptedSetting == false)
            {
                Properties.Settings.Default.encryptedSetting = (bool)enableEncryption.IsChecked;
                if (Properties.Settings.Default.encryptedSetting == true)
                {
                    changeKeyClicked();

                }
            }
            else
            {
                Properties.Settings.Default.encryptedSetting = (bool)enableEncryption.IsChecked;
            }
            Properties.Settings.Default.badAttemptSetting = (bool)badAttempt.IsChecked;
            Properties.Settings.Default.multipleBotSetting = (bool)multipleBots.IsChecked;
            Properties.Settings.Default.copyPasswordSetting = (bool)copyPassword.IsChecked;
            Properties.Settings.Default.closeStemLaunchSetting = (bool)closeStemLaunch.IsChecked;
            Properties.Settings.Default.chatComSetting = (bool)chatCommandButton.IsChecked;
            Properties.Settings.Default.autoAddFriendSetting = (bool)autoAddFriends.IsChecked;
            Properties.Settings.Default.Save();
            updateGUI();           
        }
    }
}
