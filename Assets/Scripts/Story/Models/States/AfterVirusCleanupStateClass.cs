using System;
using Apps.ChatTerminal.Commons;
using Apps.VirusFinder.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class AfterVirusCleanupStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.AfterVirus;
        public override int NextState { get; } = (int)StatesEnum.ThomasFinal;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("headOfDpt");
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.openedContact -= TransitionCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.openedContact += TransitionCheck;
        }

        private void TransitionCheck(string contactId)
        {
            if (contactId == "headOfDpt")
            {
                ChangeToNextState();
            }
        }
    }
}