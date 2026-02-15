using System;
using System.IO;
using Apps.Commons;
using Apps.FileManager.Models;
using Apps.FileViewer.Commons;
using Desktop.Commons;
using FourthWall.FileGeneration.Models;
using Story.Models.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using User.Commons;
using User.Models;

namespace Apps.FileViewer.Views
{
    public class FileViewerView : AppsCommon
    {
        //File Display
        [SerializeField] private RectTransform fileHolder;
        private RectTransform _fileHolderBackup;
        private GameObject _instantiatedFileReference;
        
        //Zoom Controls
        [SerializeField] private TMP_Text zoomLevelText;
        [SerializeField] private InputActionReference zoomAction;
        private Vector2 _zoomInput;
        
        //Metadata
        [SerializeField] private GameObject metadataPopup;
        
        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
            
            GameObject fileToOpen = FileViewerMvc.Instance.FileLoaderController.OpenedFile;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened?.Invoke(fileToOpen.GetComponent<FileModel>().FileName);
            
            _instantiatedFileReference = Instantiate(fileToOpen, fileHolder);
            
            //Instantiate and center the file in the file holder
            var instantiatedRect = _instantiatedFileReference.GetComponent<RectTransform>();
            // Store original size if it's stretched
            Vector2 originalSize = instantiatedRect.rect.size;
            // Set anchors to center
            instantiatedRect.anchorMin = new Vector2(0.5f, 0.5f);
            instantiatedRect.anchorMax = new Vector2(0.5f, 0.5f);
            instantiatedRect.pivot = new Vector2(0.5f, 0.5f);
            // Set size and position
            instantiatedRect.sizeDelta = originalSize;
            instantiatedRect.anchoredPosition = Vector2.zero;
            
            _fileHolderBackup = fileHolder;
            zoomLevelText.text = "100%";
            
            //Bring to front
            transform.SetAsLastSibling();
        }

        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
            
            Destroy(_instantiatedFileReference);
            fileHolder = _fileHolderBackup;
            zoomLevelText.text = "100%"; //Maybe redundant, but better safe than sorry
            metadataPopup.SetActive(false);
        }

        private void Update()
        {
            _zoomInput = zoomAction.action.ReadValue<Vector2>();
            switch (_zoomInput.y)
            {
                case > 0:
                    ChangeZoomLevel(0.1f);
                    break;
                case < 0:
                    ChangeZoomLevel(-0.1f);
                    break;
            }
        }
            
        /// <summary>
        /// Changes the zoom level of the file. 
        /// </summary>
        /// <param name="zoom">Strength of the zoom</param>
        public void ChangeZoomLevel(float zoom)
        {
            var oldScale = (decimal)fileHolder.localScale.x;
            decimal newScale = oldScale + (decimal)zoom;
            
            if (newScale < 0.1m)
            {
                return;
            }
            
            fileHolder.localScale = new Vector3((float)newScale, (float)newScale, (float)newScale);
            zoomLevelText.text = Math.Truncate(newScale * 100m) + "%";
        }

        /// <summary>
        /// Toggles the metadata popup visibility.
        /// </summary>
        public void ToggleMetadata()
        {
            metadataPopup.SetActive(!metadataPopup.activeSelf);
        }
    }
}
