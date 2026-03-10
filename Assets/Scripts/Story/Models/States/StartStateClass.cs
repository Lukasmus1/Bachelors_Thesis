using System;
using Apps.ChatTerminal.Commons;
using Apps.CompilationHelper.Commons;
using Apps.FileManager.Commons;
using Apps.FileViewer.Commons;
using FourthWall.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class StartStateClass : StateClass
    {
        public override int State => (int)StatesEnum.Start;
        public override int NextState { get; set; } = (int)StatesEnum.MouseQuest;
        
        public override void OnEnter()
        {
            LoadFromState();
        }

        public override void OnExit()
        {
            FileViewerMvc.Instance.FileLoaderController.onFileOpened -= CheckForStateChange;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= CheckForLoadGuide;
        }

        public override void LoadFromState()
        {
            FileViewerMvc.Instance.FileLoaderController.onFileOpened += CheckForStateChange;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += CheckForLoadGuide;
        }

        private void CheckForLoadGuide(string messageID)
        {
            if (messageID == "itTutorial")
            {
                FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("Guide", true);
            }
        }
        
        private void CheckForStateChange(string appName)
        {
            if (appName != "Guide")
            {
                return;
            }
            
            FileManagerMvc.Instance.FileManagerController.ToggleFileVisibility("Guide", true);
            ChangeToNextState();
        }
    }
}