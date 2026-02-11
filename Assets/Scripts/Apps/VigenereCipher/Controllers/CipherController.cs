using Apps.VigenereCipher.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.VigenereCipher.Controllers
{
    public class CipherController
    {
        private readonly CipherModel cipherModel = new(); 
        
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
        /// Encrypts a plain text using the Vigenere cipher with the provided key
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="key">Cipher key</param>
        /// <returns>Encrypted text</returns>
        public string EncryptText(string plainText, string key)
        {
            return cipherModel.EncryptText(plainText, key);
        }
        
        /// <summary>
        /// Decrypts a cipher text using the Vigenere cipher with the provided key
        /// </summary>
        /// <param name="cipherText">Text to encrypt</param>
        /// <param name="key">Cipher key</param>
        /// <returns>Encrypted text</returns>
        public string DecryptText(string cipherText, string key)
        {
            return cipherModel.DecryptText(cipherText, key);
        }
        
        /// <summary>
        /// Encrypts or decrypts an image using a XOR cipher with the provided key. The same function can be used for both encryption and decryption since XOR is symmetric.
        /// </summary>
        /// <param name="cipherTexture">Texture of the image to XOR</param>
        /// <param name="key">Key to XOR the image</param>
        /// <returns>Image ran through the XOR cipher</returns>
        public Sprite EncryptDecryptImage(Texture2D cipherTexture, string key)
        {
            Texture2D tex = cipherModel.DecryptImage(cipherTexture, key);
            
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }
}