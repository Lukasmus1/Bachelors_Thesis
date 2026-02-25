using System;
using Apps.ChatTerminal.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class FirstConsequencesStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FirstConsequences;
        public override int NextState { get; set; } = (int)StatesEnum.CuratorExplanation;

        public override void OnEnter()
        {
            // From the first choice, there should be a message in the queue from KP
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("kp");
            
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
            if (messageID != "kpConsequence")
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}