using System;
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

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for ToolKit.xaml
    /// </summary>
    public partial class ToolKit
    {       
        private const String STEAM_GAME_CONTROLLER = "steamBoost\\steamGameControl.exe";  //https://github.com/vishwenga/Steam-Boost/tree/master/steamGameControl
        private const String GAME_LIST_FILE = "steamBoost\\game-list.txt";
        private MainWindow backHandle = null;

        public ToolKit()
        {           
            InitializeComponent();
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
                    + Environment.CurrentDirectory.ToString() + "\\steamBoost\\"
                    + " \n\n\n MISSING FILES IN FOLDER >> \n\n" 
                    + Environment.CurrentDirectory.ToString() + "\\steamBoost\\steamGameControl.exe \n\n"
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

        private void getGamesFromFile()
        {
            string[] gameList = System.IO.File.ReadAllLines(GAME_LIST_FILE);
            foreach (string game in gameList)
            {
                listView1.Items.Add(new ListViewItem() { Content = game.Split('`')[1], Tag = game.Split('`')[0] });
            }
        }

        private void backButton(object sender, RoutedEventArgs e)
        {
            if (settingButton.Content.ToString().Equals("Back to Main Page"))
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

        public void Show(MainWindow backHandle)
        {
            this.backHandle = backHandle;
            Show();
            initLogics();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            backHandle.Show();
        }
    }
}
