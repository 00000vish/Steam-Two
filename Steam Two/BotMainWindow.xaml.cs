using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for BotMainWindow.xaml
    /// </summary>
    public partial class BotMainWindow
    {
        private const String STEAM_BOOST_DIRECTORY = "steamBoost\\";
        private const String STEAM_GAME_CONTROLLER = STEAM_BOOST_DIRECTORY + "steamGameControl.exe";  //https://github.com/vishwenga/Steam-Boost/tree/master/steamGameControl
        private const String SAM_GAME = STEAM_BOOST_DIRECTORY + "SAM.Game.exe";

        private String gameListFile = "-game-list.txt";

        public BotMainWindow()
        {
            InitializeComponent();
        }

        string username;
        public void Show(string u, string p, MainWindow backHandle)
        {
            this.username = u;
            gameListFile = username + gameListFile;          
            Show();
            do
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            } while (!_shown && !SteamBotController.loggedIn);
            SteamBotController.steamLogin(username, p);
            initLogics();
            label1.Content = "Login into " + username;
            openChat1.IsEnabled = SteamTwoProperties.jsonSetting.chatSetting;
            label3.Content = "Chat Commands On : " + SteamTwoProperties.jsonSetting.chatComSetting;             
        }

        private void initLogics()
        {
            if (File.Exists(STEAM_GAME_CONTROLLER))
            {
                if (!File.Exists(gameListFile))
                {
                    do
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                    } while (!SteamBotController.loggedIn);
                    generateGames();
                }
                getGamesFromFile();              
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
            //Hide();
            try
            {               
                Process.Start(new ProcessStartInfo(STEAM_GAME_CONTROLLER, "botgamelist " + SteamBotController.getSteamUserID() + " " + username));
            }
            catch (Exception) { }           
            do
            {
                Thread.Sleep(2000);
            } while (!File.Exists(gameListFile));
            //Show();
        }

        private void getGamesFromFile()
        {
            string[] gameList = System.IO.File.ReadAllLines(gameListFile);
            foreach (string game in gameList)
            {
                listView1.Items.Add(new ListViewItem() { Content = game.Split('`')[1], Tag = game.Split('`')[0] });
            }
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
                MainWindow.currentHandle.Show();
                settingButton.Content = "Hide Main Page";
            }
            else
            {
                MainWindow.currentHandle.Hide();
                settingButton.Content = "Back to Main Page";
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SteamBotController.logBotOff();
            MainWindow.currentHandle.Show();
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

        bool _shown;
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_shown)
                return;
            _shown = true;                       
        }

        private void openChat1_Click(object sender, RoutedEventArgs e)
        {
            steamChatWindow SCW = new steamChatWindow();
            SCW.Show();
        }

        private void idle1_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                ListViewItem item = new ListViewItem();
                item = (ListViewItem)listView1.SelectedItem;
                int temp;
                int.TryParse(item.Tag.ToString(), out temp);
                SteamBotController.playGame(temp);
            }
        }
    }
}
