using System.IO;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestScripts
{
    public class ImageCipher : MonoBehaviour
    {
        [SerializeField] private Image imageToEncrypt;
        [SerializeField] private TMP_InputField inputField;


        public void Decrypt()
        {
            byte[] originalTexture = imageToEncrypt.sprite.texture.GetRawTextureData();
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(inputField.text);
            int seedNum;
            
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(textBytes);
                seedNum = System.BitConverter.ToInt32(hash, 0);
            }
            
            var rnd = new System.Random(seedNum);
            
            var keyStream = new byte[originalTexture.Length];
            rnd.NextBytes(keyStream);
            
            for (int i = 0; i < originalTexture.Length; i++)
            {
                originalTexture[i] ^= keyStream[i];
            }
            
            var decryptedTexture = new Texture2D(imageToEncrypt.sprite.texture.width, imageToEncrypt.sprite.texture.height, imageToEncrypt.sprite.texture.format, false);
            decryptedTexture.LoadRawTextureData(originalTexture);
            decryptedTexture.Apply();
            
            imageToEncrypt.sprite = Sprite.Create(decryptedTexture, new Rect(0, 0, decryptedTexture.width, decryptedTexture.height), new Vector2(0.5f, 0.5f));
        }
    }
}