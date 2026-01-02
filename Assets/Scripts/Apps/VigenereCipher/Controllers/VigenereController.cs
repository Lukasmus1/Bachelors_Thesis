using Apps.VigenereCipher.Models;

namespace Apps.VigenereCipher.Controllers
{
    public class VigenereController
    {
        private readonly VigenereModel _vigenereModel = new(); 
        
        public string GenerateVigenereKey(int keyLen)
        {
            return _vigenereModel.GenerateVigenereKey(keyLen);
        }
        
        public string EncryptText(string plainText, string key)
        {
            return _vigenereModel.EncryptText(plainText, key);
        }
        
        public string DecryptText(string cipherText, string key)
        {
            return _vigenereModel.DecryptText(cipherText, key);
        }
    }
}