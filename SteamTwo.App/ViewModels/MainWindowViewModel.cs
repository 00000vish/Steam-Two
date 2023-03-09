using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using SteamTwo.App.Helpers;
using SteamTwo.App.Windows;
using SteamTwo.Shared.Helpers;
using System.Diagnostics;
using System.Reactive;
using System.Security.AccessControl;

namespace SteamTwo.App.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        public ReactiveCommand<Unit, Unit> AddAccountCommand { get; }
        public ReactiveCommand<Unit, Unit> SettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> GithubCommand { get; }

        public MainWindowViewModel()
        {
            AddAccountCommand = ReactiveCommand.Create(AddAccount);
            GithubCommand = ReactiveCommand.Create(VisitGithub);
            SettingsCommand = ReactiveCommand.Create(ShowSettings);
        }

        public void AddAccount()
        {
            //forgive me mvvm
            AddAccountView addAcc = new();
            addAcc.Show();
        }

        public void VisitGithub()
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "https://github.com/00000vish/Steam-Two",
                UseShellExecute = true
            });
        }

        public void ShowSettings()
        {

        }
    }
}
