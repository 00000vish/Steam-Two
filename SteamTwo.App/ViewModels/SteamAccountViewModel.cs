using ReactiveUI;
using SteamTwo.Shared.Helpers;
using SteamTwo.Shared.Models;
using System.Reactive;

namespace SteamTwo.App.ViewModels
{
    public class SteamAccountViewModel : BaseViewModel
    {
        public string Username { get; }
        public ReactiveCommand<Unit, Unit> CopyUsernameCommand { get; }
        public ReactiveCommand<Unit, Unit> CopyPasswordCommand { get; }

        private SteamAccount _account;

        public SteamAccountViewModel(SteamAccount account)
        {
            _account = account;

            Username = _account.Username;

            CopyUsernameCommand = ReactiveCommand.Create(CopyUsername);
            CopyPasswordCommand = ReactiveCommand.Create(CopyPassword);
        }

        private void CopyUsername()
        {
            System.Windows.Clipboard.SetText(_account.Username);
        }

        private void CopyPassword()
        {
            System.Windows.Clipboard.SetText(_account.GetPassword());
        }
    }
}
