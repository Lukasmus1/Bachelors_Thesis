using System;
using Apps.ChatTerminal.Commons;
using Apps.VirusFinder.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class ThomasFinalCleanupStateClass : StateClass
    {
        public override int State => (int)StatesEnum.ThomasFinal;
        public override int NextState { get; set; } = (int)StatesEnum.AfterThomasFinal;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("thomasBare");
            
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
            if (messageID == "bareFinal")
            {
                ChangeToNextState();
            }
        }
    }
}