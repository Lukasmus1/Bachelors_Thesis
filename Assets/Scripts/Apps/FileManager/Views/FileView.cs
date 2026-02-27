using System;
using Apps.Commons.FileScripts;
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
        [SerializeField] private GameObject contextOpenOnlyMenuPrefab;
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

        public void OnPointerClick(PointerEventData eventData)
        {
            //Disgusting hack
            if (_fileObject.TryGetComponent(out ScreenshotHandler handler))
            {
                handler.SetScreenshot();
            }
            
            FileViewerMvc.Instance.FileLoaderController.OpenedFile = _fileObject;
            
            if (_fileObject.name == "NumberPattern")
            {
                CreateContextMenu(eventData, contextOpenOnlyMenuPrefab);
                return;
            }
            
            CreateContextMenu(eventData, contextMenuPrefab);
        }

        private void CreateContextMenu(PointerEventData data, GameObject contextMenuType)
        {
            //Instantiate context menu
            GameObject contextMenu = Instantiate(contextMenuType, _contextMenuParent);
            var contextMenuView = contextMenu.GetComponent<ContextMenuView>();
            contextMenuView.SelectedFile = _fileObject;
            contextMenuView.SetShowHideFileButton();
            
            //Set the current context menu in the controller
            FileManagerMvc.Instance.ContextMenuController.OpenNewContextMenu(contextMenu);
            
            //Setting the position to the right corner of the context menu
            var contextMenuRect = contextMenu.GetComponent<RectTransform>();
            contextMenu.transform.position = new Vector2(data.position.x + contextMenuRect.rect.width * 0.5f, data.position.y - contextMenuRect.rect.height * 0.5f);
        }
    }
}