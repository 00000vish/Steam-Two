using SteamTwo.App.ViewModels;
using SteamTwo.App.Views;
using System.Windows;

namespace SteamTwo.App.Windows
{
    /// <summary>
    /// Interaction logic for MasterKeyView.xaml
    /// </summary>
    public partial class MasterKeyView : Window
    {
        public MasterKeyView()
        {
            InitializeComponent();
            this.DataContext = new MasterKeyViewModel();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(e.NewValue != null && e.NewValue is MasterKeyViewModel vm)
            {
                vm.SetMasterKey = () =>
                {
                    vm.MasterKey = MasterKeyInput.Password;
                };
                vm.OpenMainWindow = () =>
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                };
            }
        }
    }
}
