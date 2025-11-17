using Apps.FileManager.Models;
using Apps.FileViewer.Commons;
using TMPro;
using UnityEngine;

namespace Apps.FileManager.Views
{
    public class FileView : MonoBehaviour
    {
        public TMP_Text fileName;
        private GameObject _openFileButton;
        private GameObject _fileObject;
        
        public void SetProps(GameObject fileObject, GameObject openFButton)
        {
            // Set the opened file in the FileViewer context
            _fileObject = fileObject;
            
            var fileModel = fileObject.GetComponent<FileModel>();
            
            fileName.text = fileModel.FileName;
            _openFileButton = openFButton;
        }

        public void OnClick()
        {
            FileViewerMvc.Instance.FileLoaderController.OpenedFile = _fileObject;
            _openFileButton.SetActive(true);
        }
    }
}