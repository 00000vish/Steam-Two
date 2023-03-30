using DynamicData;
using Newtonsoft.Json;
using SteamTwo.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SteamTwo.Shared.Helpers
{
    public static class AccountManager
    {
        private static readonly SourceList<SteamAccount> _accounts = new SourceList<SteamAccount>();
        private static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".accounts");
        
        public static IObservable<IChangeSet<SteamAccount>> Connect() => _accounts.Connect();

        public static void Load()
        {
            if (File.Exists(_filePath))
            {
                var accs = JsonConvert.DeserializeObject<List<SteamAccount>>(_filePath) ?? new();
                var accsValid = accs.Where(x => x.Validate()).ToList();
                
                _accounts.Clear();
                _accounts.AddRange(accsValid);
            }
        }

        public static void Add(SteamAccount account)
        {
            _accounts.Add(account);
        }

        public static void Save()
        {
            var output = JsonConvert.SerializeObject(_accounts);
            File.WriteAllText(_filePath, output);
        }
    }
}
