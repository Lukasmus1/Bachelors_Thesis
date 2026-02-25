using System;
using System.Linq;
using Apps.ChatTerminal.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class FirstChoiceStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FirstChoice;
        public override int NextState { get; } = (int)StatesEnum.FirstConsequences;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("kp");
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("kp");
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= OnMessageTyped;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += OnMessageTyped;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
        }

        private void OnMessageTyped(string message)
        {
            if (message != "kpFirstChoice")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("headOfDpt");
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= OnMessageTyped;
        }

        private void TransitionCheck(string messageID)
        {
            if (messageID != "dptScreenEnd")
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}