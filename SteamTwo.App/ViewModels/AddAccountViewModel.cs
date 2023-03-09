using ReactiveUI;
using SteamTwo.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SteamTwo.App.ViewModels
{
    public class AddAccountViewModel : BaseViewModel
    {
        public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }
        
        public AddAccountViewModel()
        {
            SaveCommand = ReactiveCommand.Create(Save);
        }

        public void Save()
        {

        }

    }
}
