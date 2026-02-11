using System;
using System.Text;
using Apps.VigenereCipher.Controllers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Apps.VigenereCipher.Models
{
    public class CipherModel
    {
        private const string CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ<>/\"-=0123456789";

        /// <see cref="CipherController.GenerateVigenereKey"/>
        public string GenerateVigenereKey(int keyLen)
        {
            StringBuilder key = new();
            while (keyLen > 0)
            {
                key.Append(CHARS[Random.Range(0, CHARS.Length)]);
                keyLen--;
            }
            
            return key.ToString();
        }
        
        /// <see cref="CipherController.EncryptText"/>
        public string EncryptText(string plainText, string key)
        {
            StringBuilder encrypted = new();
            var currentKeyIndex = 0;
            
            foreach (char c in plainText)
            {
                //Ignore characters not in CHARS (such as newline, spaces, etc.)
                if (!CHARS.Contains(c.ToString()))
                {
                    encrypted.Append(c);
                    continue;
                }
                
                //Index of the c in CHARS + Index of the key char in CHARS (this is the shift in the alphabet) modulo the length of CHARS
                int index = (CHARS.IndexOf(c) + CHARS.IndexOf(key[currentKeyIndex])) % CHARS.Length;
                encrypted.Append(CHARS[index]);
                
                //Increment key index and loop around if necessary
                currentKeyIndex = (currentKeyIndex + 1) % key.Length;
            }
            
            return encrypted.ToString();
        }
        
        /// <see cref="CipherController.DecryptText"/>
        public string DecryptText(string plainText, string key)
        {
            StringBuilder decrypted = new();
            var currentKeyIndex = 0;

            foreach (char c in plainText)
            {
                //Ignore characters not in CHARS (such as newline, spaces, etc.)
                if (!CHARS.Contains(c.ToString()))
                {
                    decrypted.Append(c);
                    continue;
                }
                
                //Index of the c in CHARS - Index of the key char in CHARS (this is the shift in the alphabet) modulo the length of CHARS
                int index = (CHARS.IndexOf(c) - CHARS.IndexOf(key[currentKeyIndex]) + CHARS.Length) % CHARS.Length;
                decrypted.Append(CHARS[index]);
                
                //Increment key index and loop around if necessary
                currentKeyIndex = (currentKeyIndex + 1) % key.Length;
            }

            return decrypted.ToString();
        }

        /// <see cref="CipherController.EncryptDecryptImage"/>
        public Texture2D DecryptImage(Texture2D cipherTexture, string key)
        {
            //Getting the raw texture data as a byte array
            byte[] originalTexture = cipherTexture.GetRawTextureData();
            byte[] textBytes = Encoding.UTF8.GetBytes(key);
            int seedNum;
            
            //Creating a hash of the key to use as a seed for the random number generator (this ensures that the same key will always produce the same random sequence)
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(textBytes);
                seedNum = BitConverter.ToInt32(hash, 0);
            }
            
            //Creating a random number generator with the seed derived from the key that has the same length as the original picture
            System.Random rnd = new(seedNum);
            var keyStream = new byte[originalTexture.Length];
            rnd.NextBytes(keyStream);
            
            //XOR cipher
            for (int i = 0; i < originalTexture.Length; i++)
            {
                originalTexture[i] ^= keyStream[i];
            }
            
            //Create a new texture with the same dimensions and format as the original and load the decrypted raw texture data into it
            var decryptedTexture = new Texture2D(cipherTexture.width, cipherTexture.height, cipherTexture.format, false);
            decryptedTexture.LoadRawTextureData(originalTexture);
            decryptedTexture.Apply();

            return decryptedTexture;
        }
    }
}