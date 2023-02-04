using Newtonsoft.Json;
using SteamTwo.Shared.Helpers;

namespace SteamTwo.Shared.Models
{
    public class SteamAccount
    {
        [JsonProperty]
        public string Username { get; }
        [JsonProperty]
        public string PasswordEncrypted { get; }

        private SteamAccount(string username, string password)
        {
            Username = username;
            PasswordEncrypted = Encryptor.Encrypt(password);
        }
        
        public string GetPassword()
        {
            return Encryptor.Decrypt(PasswordEncrypted);
        }
    }
}
