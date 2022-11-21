using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SharedLibrary
{
    public sealed class AppEncryption
    {
        private const string Key = "A9z0B8y1C7x2D6w3";

        public static string RandomString()
        {
            //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            //byte[] buff = new byte[20];
            //rng.GetBytes(buff);
            //return  Convert.ToBase64String(buff);

            byte[] rng = RandomNumberGenerator.GetBytes(20);
            return Convert.ToBase64String(rng);
        }


        public static string CreateSalt()
        {
            return RandomString();
        }


        public static string CreatePasswordHash(string pwd, string salt)
        {
            string saltandpass = string.Concat(pwd, salt);
            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltandpass));
            return Convert.ToBase64String(hash);
        }


        public static bool ComparePassword(string hash, string password, string salt)
        {
            return hash == CreatePasswordHash(password, salt);
        }


        public static string Encrypt(string plainText, string key = Key)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new MemoryStream();
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }


        public static string Decrypt(string cipherText, string key = Key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new MemoryStream(buffer);
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }


        public static bool TryDecrypt(string cipherText, out string plainText, string key = Key)
        {
            try
            {
                plainText = Decrypt(cipherText, key);
                return true;
            }
            catch (Exception)
            {
                plainText = string.Empty;
                return false;
            }
        }
    }
}
