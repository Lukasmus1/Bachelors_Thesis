using System;
using System.Text;
using System.Threading.Tasks;
using Apps.CipherSolver.Controllers;
using UnityEngine;
using User.Commons;
using Random = UnityEngine.Random;

namespace Apps.CipherSolver.Models
{
    public class CipherModel
    {
        private const string VIGENERE_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ<>/\"-=0123456789";
        private readonly string[] _names = {"Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Heidi", "Ivan", "Judy", "Karl", "Leo", "Mallory", "Nina", "Oscar", "Peggy", "Quentin", "Rupert"};

        /// <see cref="CipherController.GenerateVigenereKey"/>
        public string GenerateVigenereKey(int keyLen)
        {
            StringBuilder key = new();
            while (keyLen > 0)
            {
                key.Append(VIGENERE_CHARS[Random.Range(0, VIGENERE_CHARS.Length)]);
                keyLen--;
            }
            
            return key.ToString();
        }

        //Cache the random year string so that it doesn't change every time the function is called (which would make it impossible to solve the puzzle)
        //This information is saved in the UserModel, however to generate the XOR code, I would need to call the ProceduralData at time of initialization, which is impossible
        private string randomYearString;
        
        /// <summary>
        /// Generates a random year between 1995 and 2005 as a string to be used as a key for the XOR cipher in the picture puzzle.
        /// </summary>
        /// <returns>A string of year between 1995 and 2005</returns>
        public string GeneratePictureYearCode()
        {
            if (!string.IsNullOrEmpty(randomYearString))
            {
                return randomYearString;
            }
            
            int year = Random.Range(1995, 2005);
            randomYearString = year.ToString();
            return year.ToString();
        }
        
        //Cache the random name string so that it doesn't change every time the function is called (which would make it impossible to solve the puzzle)
        //This information is saved in the UserModel, however to generate the picture code, I would need to call the ProceduralData at time of initialization, which is impossible
        private string randomNameString;
        
        /// <summary>
        /// Generates a random name from the predefined list of names. This is used to generate the scammer's name for the picture code puzzle.
        /// </summary>
        /// <returns>A random string first name</returns>
        public string GenerateRandomName()
        {
            return !string.IsNullOrEmpty(randomNameString) ? randomNameString : _names[Random.Range(0, _names.Length)];
        }
        
        /// <see cref="CipherController.EncryptText"/>
        public string EncryptText(string plainText, string key)
        {
            StringBuilder encrypted = new();
            var currentKeyIndex = 0;
            
            foreach (char c in plainText)
            {
                //Ignore characters not in VIGENERE_CHARS (such as newline, spaces, etc.)
                if (!VIGENERE_CHARS.Contains(c.ToString()))
                {
                    encrypted.Append(c);
                    continue;
                }
                
                //Index of the c in VIGENERE_CHARS + Index of the key char in VIGENERE_CHARS (this is the shift in the alphabet) modulo the length of VIGENERE_CHARS
                int index = (VIGENERE_CHARS.IndexOf(c) + VIGENERE_CHARS.IndexOf(key[currentKeyIndex])) % VIGENERE_CHARS.Length;
                encrypted.Append(VIGENERE_CHARS[index]);
                
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
                //Ignore characters not in VIGENERE_CHARS (such as newline, spaces, etc.)
                if (!VIGENERE_CHARS.Contains(c.ToString()))
                {
                    decrypted.Append(c);
                    continue;
                }
                
                //Index of the c in VIGENERE_CHARS - Index of the key char in VIGENERE_CHARS (this is the shift in the alphabet) modulo the length of VIGENERE_CHARS
                int index = (VIGENERE_CHARS.IndexOf(c) - VIGENERE_CHARS.IndexOf(key[currentKeyIndex]) + VIGENERE_CHARS.Length) % VIGENERE_CHARS.Length;
                decrypted.Append(VIGENERE_CHARS[index]);
                
                //Increment key index and loop around if necessary
                currentKeyIndex = (currentKeyIndex + 1) % key.Length;
            }

            return decrypted.ToString();
        }

        /// <see cref="CipherController.EncryptDecryptImage"/>
        public async Task<Texture2D> DecryptImage(Texture2D cipherTexture, string key)
        {
            byte[] originalTexture = cipherTexture.GetRawTextureData();
            int width = cipherTexture.width;
            int height = cipherTexture.height;
            TextureFormat format = cipherTexture.format;
            
            UserMvc.Instance.UserController.ScreenshotSize = new Vector2(width, height);
            UserMvc.Instance.UserController.ScreenshotFormat = (int)format;
            
            byte[] decryptedData = await Task.Run(() =>
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(key);
                int seedNum;

                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(textBytes);
                    seedNum = BitConverter.ToInt32(hash, 0);
                }

                System.Random rnd = new(seedNum);
                var keyStream = new byte[originalTexture.Length];
                rnd.NextBytes(keyStream);
                
                // XOR cipher
                for (int i = 0; i < originalTexture.Length; i++)
                {
                    originalTexture[i] ^= keyStream[i];
                }

                return originalTexture;
            });
            
            var decryptedTexture = new Texture2D(width, height, format, false);
            decryptedTexture.LoadRawTextureData(decryptedData);
            decryptedTexture.Apply();
            
            return decryptedTexture;
        }
    }
}