using ReactiveUI;
using SteamTwo.Shared.Helpers;
using System;
using System.Reactive;

namespace SteamTwo.App.ViewModels
{
    public class MasterKeyViewModel : BaseViewModel
    {
        public string MasterKey { get; set; }
        public Action? SetMasterKey { get; set; }
        public Action? OpenMainWindow { get; set; } 

        public ReactiveCommand<Unit, Unit> SetMasterKeyCommand { get; }

        public MasterKeyViewModel()
        {
            MasterKey = "";
            SetMasterKeyCommand = ReactiveCommand.Create(SetKeyMethod);
        }

        public void SetKeyMethod()
        {
            SetMasterKey?.Invoke();
            AccountManager.LoadAccounts(MasterKey);
            OpenMainWindow?.Invoke();
        }
    }
}
