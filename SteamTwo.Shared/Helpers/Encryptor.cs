﻿using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;


namespace SteamTwo.Shared.Helpers
{
    public static class Encryptor
    {
        private static string _masterKey = "aPHsydtFsbC8jrxf";
        private static string _iv = "Hh8B3RCBNYKyYr42";

        public static void SetMasterKey(string masterKey)
        {
            _masterKey = masterKey;

            char[] charArray = _masterKey.ToCharArray();
            Array.Reverse(charArray);
            _iv =  new string(charArray);
        }

        public static string Encrypt(string data)
        {
            var myAesKey = Encoding.UTF8.GetBytes(_masterKey);
            var myAesIV = Encoding.UTF8.GetBytes(_iv);

            byte[] encrypted = EncryptStringToBytes_Aes(data, myAesKey, myAesIV);

            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string data)
        {
            var myAesKey = Encoding.UTF8.GetBytes(_masterKey);
            var myAesIV = Encoding.UTF8.GetBytes(_iv);

            var encrypted = Convert.FromBase64String(data);

            return DecryptStringFromBytes_Aes(encrypted, myAesKey, myAesIV);
        }

        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

    }
}
