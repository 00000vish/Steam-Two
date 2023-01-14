using ReactiveUI;
using SteamTwo.Shared.Helpers;
using System.Diagnostics;
using System.Reactive;

namespace SteamTwo.App.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        public ReactiveCommand<Unit, Unit> SettingCommand { get; }
        public ReactiveCommand<Unit, Unit> GithubCommand { get; }

        public MainWindowViewModel()
        {
            GithubCommand = ReactiveCommand.Create(VisitGithub);
            SettingCommand = ReactiveCommand.Create(ShowSettings);
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
