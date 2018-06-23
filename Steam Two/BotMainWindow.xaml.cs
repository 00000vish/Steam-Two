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
        private const String GAME_LIST_FILE = "bot-game-list.txt";
        private const String SAM_GAME = STEAM_BOOST_DIRECTORY + "SAM.Game.exe";

        public BotMainWindow()
        {
            InitializeComponent();
        }

        string username, password;
        public void Show(string username2, string password2, MainWindow backHandle)
        {
            this.username = username2;
            this.password = password2;
            Show();
            label1.Content = "Login into " + username;
            openChat1.IsEnabled = SteamTwoProperties.jsonSetting.chatSetting;
            label3.Content = "Chat Commands On : " + SteamTwoProperties.jsonSetting.chatComSetting;             
        }

        private void initLogics()
        {
            if (File.Exists(STEAM_GAME_CONTROLLER))
            {
                if (!File.Exists(GAME_LIST_FILE))
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
                Process.Start(new ProcessStartInfo(STEAM_GAME_CONTROLLER, "botgamelist " + SteamBotController.getSteamUserID()));
            }
            catch (Exception) { }           
            do
            {
                Thread.Sleep(2000);
            } while (!File.Exists(GAME_LIST_FILE));
            //Show();
        }

        private void getGamesFromFile()
        {
            string[] gameList = System.IO.File.ReadAllLines(GAME_LIST_FILE);
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

        bool _shown;
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_shown)
                return;

            _shown = true;            
            SteamBotController.steamLogin(username, password);         
            initLogics();
        }

        private void idle1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
