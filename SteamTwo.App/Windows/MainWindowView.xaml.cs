using MahApps.Metro.Controls;
using SteamTwo.App.ViewModels;

namespace SteamTwo.App.Windows
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : MetroWindow
    {
        public MainWindowView()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
