using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Apps.FileViewer.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;

namespace Story.Models.States
{
    [Serializable]
    public class MouseQuestStateClass : StateClass
    {
        public override int State => (int)StatesEnum.MouseQuest;
        public override int NextState => (int)StatesEnum.ThomasBare;
        
        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("headOfDpt");
            
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("MouseQuest", true);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            FileViewerMvc.Instance.FileLoaderController.metadataOpened -= CheckForMetadataOpened;
        }

        public override void LoadFromState()
        {
            FileViewerMvc.Instance.FileLoaderController.metadataOpened += CheckForMetadataOpened;
        }

        private void CheckForMetadataOpened(string fileName)
        {
            if (fileName == "Mouse Quest Beta")
            {
                ChangeToNextState();
            }
        }
    }
}