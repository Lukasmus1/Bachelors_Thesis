using System;
using System.Threading.Tasks;
using Apps.CipherSolver.Models;
using UnityEngine;

namespace Apps.CipherSolver.Controllers
{
    public class CipherController
    {
        private readonly CipherModel cipherModel = new(); 
        
        public Action<string> onDecryptionAttempt;
        
        /// <summary>
        /// Generates a random key for the Vigenere cipher of the specified length
        /// </summary>
        /// <param name="keyLen">Lenght of the desired key</param>
        /// <returns>Random key string for vigenere cipher</returns>
        public string GenerateVigenereKey(int keyLen)
        {
            return cipherModel.GenerateVigenereKey(keyLen);
        }

        /// <summary>
        /// Generates a random year between 1980 and 2005 as a string to be used as a key for the XOR cipher in the picture puzzle.
        /// </summary>
        /// <returns>A random string of year between 1980 and 2005</returns>
        public string GeneratePictureYearCode()
        {
            return cipherModel.GeneratePictureYearCode();
        }

        /// <summary>
        /// Generates a random name from the predefined list of names. This is used to generate the scammer's name for the picture code puzzle.
        /// </summary>
        /// <returns>String with a random all lowercase name </returns>
        public string GenerateRandomName()
        {
            return cipherModel.GenerateRandomName().ToLower();
        }
        
        /// <summary>
        /// Encrypts a plain text using the Vigenere cipher with the provided key
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="key">Cipher key</param>
        /// <returns>Encrypted text</returns>
        public string EncryptText(string plainText, string key)
        {
            return cipherModel.EncryptDecryptText(plainText, key, true);
        }
        
        /// <summary>
        /// Decrypts a cipher text using the Vigenere cipher with the provided key
        /// </summary>
        /// <param name="cipherText">Text to encrypt</param>
        /// <param name="key">Cipher key</param>
        /// <returns>Encrypted text</returns>
        public string DecryptText(string cipherText, string key)
        {
            string text = cipherModel.EncryptDecryptText(cipherText, key, false); 
            onDecryptionAttempt?.Invoke(cipherText);
            return text;
        }
        
        /// <summary>
        /// Decrypts a cipher text using the Vigenere cipher with an incomplete key.
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="key">Key used for decryption</param>
        /// <returns>Decrypted text</returns>
        public string IncompleteKeyDecrypt(string cipherText, string key)
        {
            string text = cipherModel.EncryptDecryptTextWithIncompleteKey(cipherText, key, false); 
            onDecryptionAttempt?.Invoke(cipherText);
            return text;
        }

        /// <summary>
        /// Encrypts a plain text using the Vigenere cipher with an incomplete key.
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="key">Key used for encryption</param>
        /// <returns>Encrypted text</returns>
        public string IncompleteKeyEncrypt(string plainText, string key)
        {
            return cipherModel.EncryptDecryptTextWithIncompleteKey(plainText, key, true);
        }
        
        /// <summary>
        /// Encrypts or decrypts an image using a XOR cipher with the provided key. The same function can be used for both encryption and decryption since XOR is symmetric.
        /// </summary>
        /// <param name="cipherTexture">Texture of the image to XOR</param>
        /// <param name="key">Key to XOR the image</param>
        /// <returns>Image ran through the XOR cipher</returns>
        public async Task<Sprite> EncryptDecryptImage(Texture2D cipherTexture, string key)
        {
            Texture2D tex = await cipherModel.DecryptImage(cipherTexture, key);
            
            var res = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            
            //Shouldn't really call it when encrypting, but it should not cause any issues :pray:
            onDecryptionAttempt?.Invoke(key);
            return res;
        }
    }
}