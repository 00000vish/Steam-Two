using DynamicData;
using Newtonsoft.Json;
using SteamTwo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SteamTwo.Shared.Helpers
{
    public static class AccountManager
    {
        public static readonly ReadOnlyObservableCollection<SteamAccount> Accounts => _accounts;
        private static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".accounts");
        private static SourceList<SteamAccount> _accounts = new();

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
