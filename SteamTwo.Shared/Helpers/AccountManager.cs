using System;
using System.IO;

namespace SteamTwo.Shared.Helpers
{
    public static class AccountManager
    {
        private static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".accounts");
    }
}
