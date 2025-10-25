using System;
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
        private RectTransform fileHolderBackup;
        private GameObject instantiatedFileReference;
        
        //Zoom Controls
        [SerializeField] private TMP_Text zoomLevelText;
        [SerializeField] private InputActionReference zoomAction;
        private Vector2 _zoomInput;

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
            
            instantiatedFileReference = Instantiate(FileViewerMvc.Instance.FileLoaderController.OpenedFile, fileHolder);
            fileHolderBackup = fileHolder;
            zoomLevelText.text = "100%";
        }

        private void OnDisable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
            
            Destroy(instantiatedFileReference);
            fileHolder = fileHolderBackup;
            zoomLevelText.text = "100%"; //Maybe redundant, but better safe than sorry
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
    }
}
