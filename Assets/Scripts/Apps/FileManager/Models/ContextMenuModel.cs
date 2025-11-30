using UnityEngine;

namespace Apps.FileManager.Models
{
    public class ContextMenuModel
    {
        private GameObject CurrentContextMenu { get; set; }
        
        /// <summary>
        /// Sets the current context menu, destroying any existing one.
        /// </summary>
        /// <param name="contextMenu">Context menu to be opened</param>
        public void OpenNewContextMenu(GameObject contextMenu)
        {
            if (CurrentContextMenu != null)
            {
                Object.Destroy(CurrentContextMenu);
            }
            
            CurrentContextMenu = contextMenu;
        }
    }
}