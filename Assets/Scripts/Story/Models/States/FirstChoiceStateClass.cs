using System;
using Apps.ChatTerminal.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class FirstChoiceStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FirstChoice;
        public override int NextState { get; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("kp");
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("kp");
        }

        public override void OnExit()
        {
            
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += OnMessageTyped;
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
    }
}