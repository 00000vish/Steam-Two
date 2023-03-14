using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SteamTwo.Shared.Helpers;
using SteamTwo.Shared.Models;
using System;
using System.Reactive;

namespace SteamTwo.App.ViewModels
{
    public class AddAccountViewModel : BaseViewModel
    {
        [Reactive]
        public string Username { get; set; }
        public string Password { get; set; }
        public Action? SetPassword { get; set; } 
        public Action? Close { get; set; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }
        
        public AddAccountViewModel()
        {
            Username = "";
            Password = "";

            SaveCommand = ReactiveCommand.Create(Save);
        }

        public void Save()
        {
            SetPassword?.Invoke();
            AccountManager.AddAccount(new SteamAccount(Username, Password));
            Close?.Invoke();
        }

    }
}
