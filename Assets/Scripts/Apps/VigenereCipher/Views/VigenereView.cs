using Apps.Commons;
using Apps.FileViewer.Commons;
using Apps.VigenereCipher.Commons;
using Desktop.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.VigenereCipher.Views
{
    public class VigenereView : AppsCommon
    {
        //Cipher solving
        [SerializeField] private TMP_InputField keyInputField;
        private TMP_Text _fileText;
        private string _fileTextCopy;
        
        //Image cypher solving
        private Image _imageComponent;
        private Texture2D _imageTextureCopy;
        
        //Cipher type choosing
        [SerializeField] private TMP_Text cipherTypeLabel;
        private bool _isTextCypher;
        
        //File manipulation
        [SerializeField] private RectTransform fileHolder;
        private GameObject _instantiatedFileReference;

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);

            _instantiatedFileReference = Instantiate(FileViewerMvc.Instance.FileLoaderController.OpenedFile, fileHolder);
            _fileText = _instantiatedFileReference.GetComponentInChildren<TMP_Text>();
            if (_fileText == null)
            {
                _imageComponent = fileHolder.GetComponentInChildren<Image>();

                if (_imageComponent == null)
                {
                    return;   
                }
                
                SetAsImage();
            }
            else
            {
                SetAsText();
            }
            
            //Bring to front
            transform.SetAsLastSibling();
        }

        /// <summary>
        /// Sets the context of the cipher to be an image, and updates the UI accordingly.
        /// </summary>
        private void SetAsImage()
        {
            _imageTextureCopy = _imageComponent.sprite.texture;
            cipherTypeLabel.text = "Image Cypher";
            _isTextCypher = false;
        }

        /// <summary>
        /// Sets the context of the cipher to be a text, and updates the UI accordingly.
        /// </summary>
        private void SetAsText()
        {
            _fileTextCopy = _fileText.text;
            cipherTypeLabel.text = "Text Cypher";
            _isTextCypher = true;
        }
        
        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
            
            keyInputField.GetComponent<TMP_InputField>().text = "";
            Destroy(_instantiatedFileReference);
        }

        /// <summary>
        /// Attempt to solve the cipher using the provided key.
        /// </summary>
        public void SolveCipher()
        {
            string key = keyInputField.text;
            if (key.Length == 0)
            {
                Debug.LogWarning("Key is empty. Please enter a valid key to solve the cipher.");
                return;
            }
            
            if (_isTextCypher)
            {
                string decryptedText = CipherMvc.Instance.CipherController.DecryptText(_fileTextCopy, key);

                _fileText.text = decryptedText;
            }
            else
            {
                Sprite decryptedImage = CipherMvc.Instance.CipherController.EncryptDecryptImage(_imageTextureCopy, key);
                
                _imageComponent.sprite = decryptedImage;
            }
        }
    }
}