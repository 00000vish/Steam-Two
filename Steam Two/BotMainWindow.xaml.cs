using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for BotMainWindow.xaml
    /// </summary>
    public partial class BotMainWindow
    {
        private const String STEAM_BOOST_DIRECTORY = "steamBoost\\";
        private const String STEAM_GAME_CONTROLLER = STEAM_BOOST_DIRECTORY + "steamGameControl.exe";  //https://github.com/vishwenga/Steam-Boost/tree/master/steamGameControl
        private const String GAME_LIST_FILE = "bot-game-list.txt";
        private const String SAM_GAME = STEAM_BOOST_DIRECTORY + "SAM.Game.exe";

        private MainWindow backHandle = null;

        public BotMainWindow()
        {
            InitializeComponent();
        }

        public void Show(string username, string password, MainWindow backHandle)
        {
            this.backHandle = backHandle;
            label1.Content = "Login into " + username;
            openChat1.IsEnabled = SteamTwoProperties.jsonSetting.chatSetting;
            label3.Content = "Chat Commands On : " + SteamTwoProperties.jsonSetting.chatComSetting;
            SteamBotController.steamLogin(username, password);
            Show();
            initLogics();
        }

        private void initLogics()
        {
            if (File.Exists(STEAM_GAME_CONTROLLER))
            {
                if (File.Exists(GAME_LIST_FILE))
                {
                    getGamesFromFile();
                }
                else
                {
                    generateGames();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("FILES MISSING  >>  download at \n https://github.com/vishwenga/Steam-Boost/."
                    + " \n\n\n MISSING FOLDER >> \n\n"
                    + Environment.CurrentDirectory.ToString() + STEAM_BOOST_DIRECTORY
                    + " \n\n\n MISSING FILES IN FOLDER >> \n\n"
                    + Environment.CurrentDirectory.ToString() + STEAM_GAME_CONTROLLER + "\n\n"
                    + Environment.CurrentDirectory.ToString() + SAM_GAME + "\n\n"
                    + Environment.CurrentDirectory.ToString() + "\\steamBoost\\CSteamworks.dll \n\n"
                    + Environment.CurrentDirectory.ToString() + "\\steamBoost\\Newtonsoft.Json.dll \n\n"
                    + Environment.CurrentDirectory.ToString() + "\\steamBoost\\Newtonsoft.Json.xml \n\n"
                    + Environment.CurrentDirectory.ToString() + "\\steamBoost\\steam_api.dll \n\n"
                    + Environment.CurrentDirectory.ToString() + "\\steamBoost\\Steamworks.NET.dll \n\n");

                Close();
            }

        }

        private void generateGames()
        {
            throw new NotImplementedException();
        }

        private void getGamesFromFile()
        {
            throw new NotImplementedException();
        }

        private void logOut1_Click(object sender, RoutedEventArgs e)
        {
            SteamBotController.logBotOff();
        }

        private void logIn1_Click(object sender, RoutedEventArgs e)
        {
            SteamBotController.logBotIn();
        }

        private void endBot1_Click(object sender, RoutedEventArgs e)
        {
            SteamBotController.logBotOff();
            Close();
        }

        private void backButton(object sender, RoutedEventArgs e)
        {
            if (settingButton.Content.Equals("Back to Main Page"))
            {
                backHandle.Show();
                settingButton.Content = "Hide Main Page";
            }
            else
            {
                backHandle.Hide();
                settingButton.Content = "Back to Main Page";
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            backHandle.Show();
        }

        private void storePage1_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                ListViewItem item = new ListViewItem();
                item = (ListViewItem)listView1.SelectedItem;
                Process.Start("https://store.steampowered.com/app/" + item.Tag.ToString());
            }
        }

        private void achievements1_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                ListViewItem item = new ListViewItem();
                item = (ListViewItem)listView1.SelectedItem;
                Process.Start(new ProcessStartInfo(SAM_GAME, item.Tag.ToString()));
            }
        }

        private void launch1_Click(object sender, RoutedEventArgs e)
        {

            if (listView1.SelectedItem != null)
            {
                ListViewItem item = new ListViewItem();
                item = (ListViewItem)listView1.SelectedItem;
                Process.Start("steam://rungameid/" + item.Tag.ToString());
                WindowState = WindowState.Minimized;
            }
        }

        private void idle1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
