using Apps.FileManager.Models;
using Apps.FileViewer.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Apps.FileManager.Views
{
    public class FileView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject contextMenuPrefab;
        
        public TMP_Text fileName;
        private GameObject _fileObject;
        
        public void SetProps(GameObject fileObject)
        {
            // Set the opened file in the FileViewer context
            _fileObject = fileObject;
            
            var fileModel = fileObject.GetComponent<FileModel>();
            
            fileName.text = fileModel.FileName;
        }
        
        public void OnClick()
        {
            FileViewerMvc.Instance.FileLoaderController.OpenedFile = _fileObject;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                
            }
        }

        private void CreateContextMenu()
        {
            
        }
    }
}