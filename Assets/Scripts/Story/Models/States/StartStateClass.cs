using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Apps.FileViewer.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class StartStateClass : StateClass
    {
        public override int State => (int)StatesEnum.Start;
        public override int NextState => (int)StatesEnum.MouseQuest;
        
        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("itDept", 0);
            
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("Guide", true);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            FileViewerMvc.Instance.FileLoaderController.fileOpened -= CheckForStateChange;
        }

        public override void LoadFromState()
        {
            FileViewerMvc.Instance.FileLoaderController.fileOpened += CheckForStateChange;
        }

        private void CheckForStateChange(string appName)
        {
            if (appName == "Guide")
            {
                ChangeToNextState();
                
                FileLoaderMvc.Instance.FileLoaderController.ToggleFileVisibility("Guide", true);
            }
        }
    }
}