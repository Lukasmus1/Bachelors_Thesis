using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Apps.FileViewer.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class MouseQuestStateClass : StateClass
    {
        public override int State => (int)StatesEnum.MouseQuest;
        public override int NextState { get; set; } = (int)StatesEnum.ThomasBare;
        
        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("headOfDpt");
            
            LoadFromState();
        }

        public override void OnExit()
        {
            FileViewerMvc.Instance.FileLoaderController.metadataOpened -= CheckForMetadataOpened;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= LoadMouseQuest;
            
            FileManagerMvc.Instance.FileManagerController.ToggleFileVisibility("MouseQuest", true);
        }

        public override void LoadFromState()
        {
            FileViewerMvc.Instance.FileLoaderController.metadataOpened += CheckForMetadataOpened;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += LoadMouseQuest;
        }
        
        private void LoadMouseQuest(string messageID)
        {
            if (messageID == "dptMouseQuest")
            {
                FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("MouseQuest", true);
            }
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