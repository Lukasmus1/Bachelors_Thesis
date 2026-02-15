using System;
using Apps.ChatTerminal.Commons;
using Apps.VirusFinder.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class AfterThomasFinalCleanupStateClass : StateClass
    {
        public override int State => (int)StatesEnum.AfterThomasFinal;
        public override int NextState => (int)StatesEnum.CuratorFirst;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("headOfDpt");
            
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
            if (messageID != "dptSendingThomasLogs")
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}