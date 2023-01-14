using Microsoft.Win32;
using System;

namespace SteamTwo.Shared.Registery
{
    public static class RegistryController
    {
        public static string ReadKeyValue(RegistryEntry entry)
        {
            return Registry.LocalMachine.OpenSubKey(entry.Key)?.GetValue(entry.Value)?.ToString() ?? "Not Found";
        }

        public static bool UpdateKeyValue(RegistryEntry entry, object data)
        {
            var myKey = Registry.LocalMachine.OpenSubKey(entry.Key, true);
            if (myKey == null) return false;

            myKey.SetValue(entry.Value, data, entry.DataType);
            myKey.Close();
            return true;
        }

        public static bool DeleteKeyValue(RegistryEntry entry)
        {
            return true;
        }
    }
}
