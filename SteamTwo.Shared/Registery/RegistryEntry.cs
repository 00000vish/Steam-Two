using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTwo.Shared.Registery
{
    public class RegistryEntry
    {
        public string Key { get; }

        public string Value { get; }

        public RegistryValueKind DataType { get; }

        public RegistryEntry(string key, string value, RegistryValueKind dataType)
        {
            Key = key;
            Value = value;
            DataType = dataType;
        }
    }
}
