using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace SteamTwo.Shared.Helpers
{
    public class BaseViewModel : ReactiveObject, IDisposable
    {
        protected CompositeDisposable Disposables;
        public BaseViewModel()
        {
            Disposables = new();
        }
        
        public virtual void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
