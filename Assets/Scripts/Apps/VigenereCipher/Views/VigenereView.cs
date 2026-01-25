using Apps.Commons;
using Apps.FileViewer.Commons;
using Apps.VigenereCipher.Commons;
using Desktop.Commons;
using TMPro;
using UnityEngine;

namespace Apps.VigenereCipher.Views
{
    public class VigenereView : AppsCommon
    {
        //Cipher solving
        [SerializeField] private TMP_InputField keyInputField;
        private TMP_Text _fileText;
        private string _fileTextCopy;
        
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
                Debug.LogWarning("No TMP_Text component found in the instantiated file reference. Need to create functionality to prevent this from happening");
                return;
            }
            _fileTextCopy = _fileText.text;
            
            //Bring to front
            transform.SetAsLastSibling();
        }

        private void OnDisable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
            
            keyInputField.GetComponent<TMP_InputField>().text = "";
            Destroy(_instantiatedFileReference);
            
            DeleteBottomBarIcon();
        }

        public void SolveCipher()
        {
            string key = keyInputField.text;
            if (key.Length == 0)
            {
                Debug.LogWarning("Key is empty. Please enter a valid key to solve the cipher.");
                return;
            }
            
            string decryptedText = VigenereMvc.Instance.VigenereController.DecryptText(_fileTextCopy, key);
            
            _fileText.text = decryptedText;
        }
    }
}