using System;
using Apps.ChatTerminal.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class ThomasBareStateClass : StateClass
    {
        public override int State => (int)StatesEnum.ThomasBare;
        public override int NextState => (int)StatesEnum.AutostereogramState;
        
        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("headOfDpt");
            
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("thomasBare");
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= CheckForContactOpened;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += CheckForContactOpened;
        }

        private void CheckForContactOpened(string messageID)
        {
            if (messageID == "bareFileSent")
            {
                ChangeToNextState();
            }
        }
    }
}