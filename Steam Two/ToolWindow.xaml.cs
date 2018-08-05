using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for ToolKit.xaml
    /// </summary>
    public partial class ToolKit
    {
        private const String STEAM_BOOST_DIRECTORY = "steamBoost\\";
        private const String STEAM_GAME_CONTROLLER = STEAM_BOOST_DIRECTORY + "steamGameControl.exe";  //https://github.com/vishwenga/Steam-Boost/tree/master/steamGameControl
        private const String GAME_LIST_FILE = "game-list.txt";
        private const String SAM_GAME = STEAM_BOOST_DIRECTORY + "SAM.Game.exe";

        private bool idleStoper = false; //mostly for spam is checked
        private ArrayList runningProc = new ArrayList();

        public ToolKit()
        {
            InitializeComponent();
        }

        private void initLogics()
        {
            if (File.Exists(STEAM_GAME_CONTROLLER))
            {
                if (!File.Exists(GAME_LIST_FILE))
                {
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

        //genereate game list file
        private void generateGames()
        {
            try
            {
                Process.Start(new ProcessStartInfo(STEAM_GAME_CONTROLLER, "gamelist"));
            }
            catch (Exception) { }
            Hide();
            do
            {
                Thread.Sleep(2000);
            } while (!File.Exists(GAME_LIST_FILE));
            Show();
        }

        //read games from file
        private void getGamesFromFile()
        {
            string[] gameList = System.IO.File.ReadAllLines(GAME_LIST_FILE);
            foreach (string game in gameList)
            {
                listView1.Items.Add(new ListViewItem() { Content = game.Split('`')[1], Tag = game.Split('`')[0] });
            }
        }

        //back button
        private void backButton(object sender, RoutedEventArgs e)
        {
            if (settingButton.Content.ToString().Equals("Back to Main Page"))
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

        public void Show(MainWindow backHandle)
        {
            Show();
            initLogics();
        }

        //when form is closing
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            killRunningProc();
            MainWindow.currentHandle.Show();
        }

        //store page is clicked
        private void storePage1_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                ListViewItem item = new ListViewItem();
                item = (ListViewItem)listView1.SelectedItem;
                Process.Start("https://store.steampowered.com/app/" + item.Tag.ToString());
            }
        }

        //achievement is clicked
        private void achievements1_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                ListViewItem item = new ListViewItem();
                item = (ListViewItem)listView1.SelectedItem;
                Process.Start(new ProcessStartInfo(SAM_GAME, item.Tag.ToString()));
            }
        }

        //launch game is lcicked
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

        //idle is clicked
        private async void idle1_Click(object sender, RoutedEventArgs e)
        {
            killRunningProc();
            idleStoper = false;
            if (listView1.SelectedItem != null)
            {
                if ((bool)Spam.IsChecked)
                {
                    do
                    {
                        startIdler(true);
                        await Task.Delay(30000);
                        killRunningProc();
                        await Task.Delay(5000);

                    } while (!idleStoper);
                }
                else
                {
                    startIdler(false);
                }
            }
        }

        //actaully start idle.exe 
        private async void startIdler(bool delayed)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (delayed)
                    await Task.Delay(4000);
                runningProc.Add(Process.Start(new ProcessStartInfo(STEAM_GAME_CONTROLLER, item.Tag.ToString()) { WindowStyle = ProcessWindowStyle.Hidden }));
            }
        }

        //stop idle clicked
        private void stopIdle1_Click(object sender, RoutedEventArgs e)
        {
            idleStoper = true;
            killRunningProc();
        }

        //kill idle processed 
        private void killRunningProc()
        {
            if (runningProc.Count > 0)
            {
                foreach (var item in runningProc)
                {
                    Process tempp = (Process)item;
                    try
                    {
                        tempp.Kill();
                    }
                    catch (Exception) { }
                }
                runningProc.Clear();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            idleStoper = true;
            killRunningProc();
            File.Delete(GAME_LIST_FILE);
            generateGames();
        }
    }
}
