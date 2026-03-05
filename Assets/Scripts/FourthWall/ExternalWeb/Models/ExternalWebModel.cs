using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Apps.CipherSolver.Commons;
using FourthWall.ExternalWeb.Controllers;
using UnityEngine;

namespace FourthWall.ExternalWeb.Models
{
    public class ExternalWebModel
    {
        private const string BASE_URL = "https://s2vybmvsx1bhbmlj.netlify.app/?m=";
            
        //Vigenère cipher params
        private const string VIGENERE_KEY = "plushie";
        private const string VIGENERE_ALPHABET = "7ĚŠČ8ŘŽ9ÝÁÍ0ÉQWERTZUIOPÚASDFGH1JKLŮ-MNBVCXYěšč2řžýáíéúpoiuz3trewqasd4fghjklůmn5bvcxy6";
        
        ///<inheritdoc cref="ExternalWebController.CreateEndingUrl"/>
        ///<param name="realName">The real name to encode in the URL</param>
        public string CreateEndingUrl(string realName)
        {
            var nameLength = realName.Length.ToString();
            
            //Padding the name length with zeros if necessary
            while (nameLength.Length < 4)
            {
                nameLength = nameLength.Insert(0, "0");
            }
            
            string hash = ComputeSha256Hash(realName)[..10];
            
            string rawText = nameLength + realName + hash;

            string encryptedText = CipherMvc.Instance.CipherController.EncryptText(rawText, VIGENERE_KEY, VIGENERE_ALPHABET);
            Debug.Log(CipherMvc.Instance.CipherController.DecryptText(encryptedText, VIGENERE_KEY, VIGENERE_ALPHABET));
            
            return BASE_URL + encryptedText;
        }

        /// <summary>
        /// Computes the SHA256 hash of a given string and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="rawData">Data to create a hash of</param>
        /// <returns>Hash of the inputted data</returns>
        private string ComputeSha256Hash(string rawData)
        {
            var sb = new StringBuilder();

            using (var hash = SHA256.Create()) {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(rawData));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}