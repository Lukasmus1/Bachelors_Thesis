using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Apps.FileViewer.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class NewFilesStateClass : StateClass
    {
        public override int State => (int)StatesEnum.NewFiles;
        public override int NextState => (int)StatesEnum.Detective;
        
        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("headOfDpt", 7);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
        }

        private void TransitionCheck(string messageID)
        {
            if (messageID != "dptSendingNewFiles")
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}