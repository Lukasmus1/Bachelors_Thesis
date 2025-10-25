using Apps.FileManager.Models;
using Apps.FileViewer.Commons;
using TMPro;
using UnityEngine;

namespace Apps.FileManager.Views
{
    public class FileView : MonoBehaviour
    {
        [SerializeField] private TMP_Text fileName;
        private GameObject openFileButton;
        
        public void SetProps(GameObject fileObject, GameObject openFButton)
        {
            // Set the opened file in the FileViewer context
            FileViewerMvc.Instance.FileLoaderController.OpenedFile = fileObject;
            
            var fileModel = fileObject.GetComponent<FileModel>();
            
            fileName.text = fileModel.FileName;
            openFileButton = openFButton;
        }

        public void OnClick()
        {
            openFileButton.SetActive(true);
        }
    }
}