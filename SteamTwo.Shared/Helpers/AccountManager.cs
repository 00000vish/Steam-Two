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
        private static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".accounts");
        private static List<SteamAccount> _accounts = new();

        public static void LoadAccounts()
        {
            if (File.Exists(_filePath))
            {
                var accs = JsonConvert.DeserializeObject<List<SteamAccount>>(_filePath) ?? new();
                var accsValid = accs.Where(x => x.Validate()).ToList();
                _accounts = accsValid;
            }
        }

        public static void AddAccount(SteamAccount account)
        {
            _accounts.Add(account);
        }

        public static void SaveAccounts()
        {
            var output = JsonConvert.SerializeObject(_accounts);
            File.WriteAllText(output, _filePath);
        }
    }
}
