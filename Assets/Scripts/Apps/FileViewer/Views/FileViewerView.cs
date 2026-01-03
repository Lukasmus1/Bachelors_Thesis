using System;
using Apps.FileManager.Models;
using Apps.FileViewer.Commons;
using Desktop.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Apps.FileViewer.Views
{
    public class FileViewerView : MonoBehaviour
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
            FileViewerMvc.Instance.FileLoaderController.fileOpened?.Invoke(fileToOpen.GetComponent<FileModel>().FileName);
            
            _instantiatedFileReference = Instantiate(fileToOpen, fileHolder);
            _fileHolderBackup = fileHolder;
            zoomLevelText.text = "100%";
            
            //Bring to front
            transform.SetAsLastSibling();
        }

        private void OnDisable()
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
