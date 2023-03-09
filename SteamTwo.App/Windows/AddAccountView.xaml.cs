using MahApps.Metro.Controls;
using SteamTwo.App.ViewModels;

namespace SteamTwo.App.Windows
{
    /// <summary>
    /// Interaction logic for AddAccountView.xaml
    /// </summary>
    public partial class AddAccountView : MetroWindow
    {
        public AddAccountView()
        {
            InitializeComponent();
            this.DataContext = new AddAccountViewModel();
        }
    }
}
