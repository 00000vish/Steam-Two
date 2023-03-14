using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;


namespace SteamTwo.Shared.Helpers
{
    public static class Encryptor
    {
        private static string _masterKey = "6uh5GTFjUPvf7P";

        private static readonly int _iterations = 2;
        private static readonly int _keySize = 256;

        private static readonly string _hash = "SHA1";
        private static readonly string _salt = "qyotvsvbitb1fh70";
        private static readonly string _vector = "wgcsg7si5sbspunf";

        public static void SetMasterKey(string masterKey)
        {
            _masterKey = masterKey; 
        }

        public static string Encrypt(string value)
        {
            return Encrypt(Aes.Create(), value, _masterKey);
        }

        public static string Decrypt(string value)
        {
            return Decrypt(Aes.Create(), value, _masterKey);
        }

        private static string Encrypt<T>(T encryptAlg, string value, string password) where T : SymmetricAlgorithm
        {
            byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
            byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);

            byte[] encrypted;
            using (T cipher = encryptAlg)
            {
                PasswordDeriveBytes _passwordBytes =
                    new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (MemoryStream to = new MemoryStream())
                    {
                        using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }
                cipher.Clear();
            }
            return Convert.ToBase64String(encrypted);
        }

        private static string Decrypt<T>(T encryptAlg, string value, string password) where T : SymmetricAlgorithm
        {
            byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
            byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] valueBytes = Convert.FromBase64String(value);

            byte[] decrypted;
            int decryptedByteCount = 0;

            using (T cipher = encryptAlg)
            {
                PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                try
                {
                    using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                    {
                        using (MemoryStream from = new MemoryStream(valueBytes))
                        {
                            using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                            {
                                decrypted = new byte[valueBytes.Length];
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }

                cipher.Clear();
            }
            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }
   
    }
}
