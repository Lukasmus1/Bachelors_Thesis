using System.Text;
using UnityEngine;

namespace Apps.VigenereCipher.Models
{
    public class VigenereModel
    {
        private const string CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ<>/\"-=0123456789";

        /// <summary>
        /// Creates a random key for the Vigenere cipher
        /// </summary>
        /// <param name="keyLen">Desired length of the key</param>
        /// <returns>Random vigenere cipher key</returns>
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
        
        /// <summary>
        /// Encrypts a plain text using the Vigenere cipher with the provided key
        /// </summary>
        /// <param name="plainText">The text to encrypt</param>
        /// <param name="key">Key used for encrytpion</param>
        /// <returns>Encrypted text</returns>
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
        
        /// <summary>
        /// Decrypts a text using the Vigenere cipher with the provided key
        /// </summary>
        /// <param name="plainText">Text to decrypt</param>
        /// <param name="key">Key for decrytpion</param>
        /// <returns>Decrypted text</returns>
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
    }
}