using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
            var viewModel = new AddAccountViewModel();
            viewModel.Close = () => this.Close();
            viewModel.SetPassword = () =>
            {
                viewModel.Password = this.ThePasswordBox.Password;
            };
            this.DataContext = viewModel;
        }
    }
}
