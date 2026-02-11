using System;
using Apps.FileManager.Commons;
using Apps.FileManager.Models;
using TMPro;
using UnityEngine;

namespace Apps.FileManager.Views
{
    public class ContextMenuView : MonoBehaviour
    {
        //Toggling visibility of the file
        [SerializeField] private GameObject hideShowFileButton;
        private const string SHOW_FILE_STRING = "Show File";
        private const string HIDE_FILE_STRING = "Hide File";
        
        public GameObject SelectedFile { get; set; }
        private GameObject _parentObject;
        private DesktopHolderHelper _desktopHolderHelper; 
        
        /// <summary>
        /// Subscribes to the OnClick event of the parent object to close the context menu when clicking outside of it.
        /// </summary>
        /// <exception cref="NullReferenceException">Gets thrown if the desktopHolderHelper is null for whatever reason</exception>
        private void Awake()
        {
            _parentObject = transform.parent.gameObject;
            
            //Subscribe to the OnClick event of the parent object that is the entire screen
            _desktopHolderHelper = _parentObject.GetComponent<DesktopHolderHelper>();
            if (_desktopHolderHelper == null)
            {
                Debug.LogError("DesktopHolderHelper is null in ContextMenuView");
                throw new NullReferenceException("DesktopHolderHelper");
            }

            _desktopHolderHelper.OnClick += CloseContextMenu;
        }

        private void OnDestroy()
        {
            _desktopHolderHelper.OnClick -= CloseContextMenu;
        }

        /// <summary>
        /// Closes the context menu by disabling it.
        /// This is a method, because I need to unsubscribe from the event in OnDestroy.
        /// </summary>
        private void CloseContextMenu()
        {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Sets the text of the hide/show file button based on the current visibility of the file. If the file is hidden, the button will show "Show File", otherwise it will show "Hide File".
        /// </summary>
        public void SetShowHideFileButton()
        {
            var fileModel = SelectedFile.GetComponent<FileModel>();
            hideShowFileButton.GetComponentInChildren<TMP_Text>().text = fileModel.IsHidden ? SHOW_FILE_STRING : HIDE_FILE_STRING;
        }
        
        /// <summary>
        /// Toggles the visibility of the selected file. If the file is currently hidden, it will be shown, and if it is currently shown, it will be hidden.
        /// </summary>
        public void ToggleVisibilityOfFile()
        {
            var fileModel = SelectedFile.GetComponent<FileModel>();

            bool shouldHideFile = hideShowFileButton.GetComponentInChildren<TMP_Text>().text != SHOW_FILE_STRING;

            FileLoaderMvc.Instance.FileLoaderController.ToggleFileVisibility(fileModel.FileName, shouldHideFile);
            
            //Update the text of the button after toggling the visibility
            SetShowHideFileButton();
            
            //Closes the context menu after toggling the visibility of the file
            CloseContextMenu();
        }
    }
}