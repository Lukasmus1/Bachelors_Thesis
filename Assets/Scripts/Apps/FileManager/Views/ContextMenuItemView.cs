using System;
using Apps.FileViewer.Commons;
using Desktop.Views;
using UnityEngine;

namespace Apps.FileManager.Views
{
    public class ContextMenuItemView : MonoBehaviour
    {
        private ScriptReferenceLinker referenceLinker;
        
        private void Awake()
        {
            referenceLinker = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<ScriptReferenceLinker>();
        }

        /// <summary>
        /// Opens the app associated with the given appTag. Uses the AppIconAction to perform the action.
        /// </summary>
        /// <param name="appTag">Tag of the app to open</param>
        public void OpenApp(string appTag)
        {
            GameObject appToOpen = referenceLinker.GetApp(appTag);
            if (appToOpen == null)
            {
                throw new NullReferenceException("The app tag has been found but is not set!");
            }
            
            var appIconAction = GetComponent<AppIconAction>();
            appIconAction.appToOpen = appToOpen;
            appIconAction.PerformAction();
            
            //Immediately focus the app 
            appIconAction.transform.SetAsLastSibling();
            
            //Close the context menu after opening the app
            transform.parent.parent.gameObject.SetActive(false);
        }

        /// <summary>
        /// Opens the fullscreen in the desktop holder.
        /// </summary>
        public void OpenOnly()
        {
            GameObject desktopHolder = GameObject.FindGameObjectWithTag("ScriptHolder")
                .GetComponent<ScriptReferenceLinker>().GetMainCanvas();
            
            Instantiate(FileViewerMvc.Instance.FileLoaderController.OpenedFile, desktopHolder.transform);
        }   
    }
}