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
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= CheckForContactOpened;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= LoadThomas;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += CheckForContactOpened;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += LoadThomas;
        }

        public void LoadThomas(string messageID)
        {
            if (messageID == "dptThomasBare")
            {
                ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("thomasBare");
            }
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