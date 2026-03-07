using System;
using Apps.ChatTerminal.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class ItHelpStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.ItHelp;
        public override int NextState { get; set; } = (int)StatesEnum.Preparation;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("itDept");
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
        
        public void TransitionCheck(string messageID)
        {
            if (messageID != "itHelpEnd")
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}