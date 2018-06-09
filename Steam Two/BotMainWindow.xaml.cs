using System.Threading;
using System.Windows;

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for BotMainWindow.xaml
    /// </summary>
    public partial class BotMainWindow
    {
        public BotMainWindow()
        {
            InitializeComponent();
        }

        public void Show(string username, string password)
        {
            label1.Content = "Login into " + username;
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
    }
}
