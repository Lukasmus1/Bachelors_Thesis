using System;
using Apps.FileManager.Commons;
using Apps.FileManager.Models;
using Apps.FileViewer.Commons;
using Desktop.Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Apps.FileManager.Views
{
    public class FileView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject contextMenuPrefab;
        private Transform _contextMenuParent;
        
        public TMP_Text fileName;
        private GameObject _fileObject;

        private void Awake()
        {
            _contextMenuParent = GameObject.FindGameObjectWithTag("DesktopHolder").transform;
        }

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
            CreateContextMenu(eventData);
        }

        private void CreateContextMenu(PointerEventData data)
        {
            //Instantiate context menu
            GameObject contextMenu = Instantiate(contextMenuPrefab, _contextMenuParent);
            
            //Set the current context menu in the controller
            FileLoaderMvc.Instance.ContextMenuController.OpenNewContextMenu(contextMenu);
            
            //Setting the position to the right corner of the context menu
            var contextMenuRect = contextMenu.GetComponent<RectTransform>();
            contextMenu.transform.position = new Vector2(data.position.x + contextMenuRect.rect.width * 0.5f, data.position.y - contextMenuRect.rect.height * 0.5f);
        }
    }
}