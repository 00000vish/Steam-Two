using Newtonsoft.Json;
using SteamTwo.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace SteamTwo.Shared.Helpers
{
    public static class AccountManager
    {
        private static string _masterKey = "";
        private static List<SteamAccount> _accounts = new();
        private static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".accounts");
        
        public static void LoadAccounts(string masterKey)
        {
            _masterKey = masterKey;
        }

        public static void AddAccount(SteamAccount account)
        {
            _accounts.Add(account); 
        }

        private static void Save()
        {
            JsonConvert.SerializeObject(_accounts);
        }
    }
}
