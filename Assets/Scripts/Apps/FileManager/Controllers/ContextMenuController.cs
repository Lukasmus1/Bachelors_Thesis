using Apps.FileManager.Models;
using UnityEngine;

namespace Apps.FileManager.Controllers
{
    public class ContextMenuController
    {
        private readonly ContextMenuModel _contextMenuModel = new();

        public void OpenNewContextMenu(GameObject contextMenu)
        {
            _contextMenuModel.OpenNewContextMenu(contextMenu);
        }
    }
}