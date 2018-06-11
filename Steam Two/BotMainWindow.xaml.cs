using System.Threading;
using System.Windows;

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for BotMainWindow.xaml
    /// </summary>
    public partial class BotMainWindow
    {

        private MainWindow backHandle = null;

        public BotMainWindow()
        {
            InitializeComponent();
        }

        public void Show(string username, string password, MainWindow backHandle)
        {
            this.backHandle = backHandle;
            label1.Content = "Login into " + username;
            openChat1.IsEnabled = Properties.Settings.Default.chatSetting;
            label3.Content = "Chat Commands On : " + Properties.Settings.Default.chatComSettings;
            SteamBotController.steamLogin(username, password);
            Show();
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
            backHandle.Show();
        }
    }
}
