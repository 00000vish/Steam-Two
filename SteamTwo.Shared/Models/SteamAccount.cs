using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTwo.Shared.Models
{
    public class SteamAccount
    {
        public string Username { get; }
        public string Password { get; }
        public SteamAccount(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
