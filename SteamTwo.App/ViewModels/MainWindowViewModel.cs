using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using SteamTwo.App.Windows;
using SteamTwo.Shared.Helpers;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;

namespace SteamTwo.App.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollectionExtended<SteamAccountViewModel> SteamAccounts { get; }
        public ReactiveCommand<Unit, Unit> AddAccountCommand { get; }
        public ReactiveCommand<Unit, Unit> SettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> GithubCommand { get; }

        public MainWindowViewModel()
        {
            AddAccountCommand = ReactiveCommand.Create(AddAccount);
            GithubCommand = ReactiveCommand.Create(VisitGithub);
            SettingsCommand = ReactiveCommand.Create(ShowSettings);


            SteamAccounts = new();

            AccountManager.Connect()
                          .Transform(x => new SteamAccountViewModel(x))
                          .ObserveOnDispatcher()
                          .Bind(SteamAccounts)
                          .DisposeMany()
                          .Subscribe();
        }

        public void AddAccount()
        {
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
