using System.Text;
using UnityEngine;

namespace User.Models
{
    public abstract class ProceduralDataGeneration
    {
        public static string GenerateVigenereCode(int keyLen)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ<>/\"=";

            StringBuilder key = new();
            while (keyLen > 0)
            {
                key.Append(chars[Random.Range(0, chars.Length)]);
                keyLen--;
            }
            
            return key.ToString();
        }
    }
}