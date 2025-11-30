using System;
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
            
            var appIconAction = GetComponent<AppIconAction>();
            appIconAction.appToOpen = appToOpen;
            appIconAction.PerformAction();
            
            //Close the context menu after opening the app
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
}