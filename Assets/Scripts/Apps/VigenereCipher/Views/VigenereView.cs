using Apps.FileViewer.Commons;
using Apps.VigenereCipher.Commons;
using Desktop.Commons;
using TMPro;
using UnityEngine;

namespace Apps.VigenereCipher.Views
{
    public class VigenereView : MonoBehaviour
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
            _fileTextCopy = _fileText.text;
        }

        private void OnDisable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
            
            keyInputField.GetComponent<TMP_InputField>().text = "";
            Destroy(_instantiatedFileReference);
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