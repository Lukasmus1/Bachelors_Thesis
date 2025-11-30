using System;
using UnityEngine;

namespace Apps.FileManager.Views
{
    public class ContextMenuView : MonoBehaviour
    {
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
    }
}