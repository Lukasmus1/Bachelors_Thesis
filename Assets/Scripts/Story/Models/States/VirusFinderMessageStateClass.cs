using System;
using Apps.ChatTerminal.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class VirusFinderMessageStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.VirusFinderMessage;
        public override int NextState { get; } = (int)StatesEnum.VirusFinder;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("itDept", 1);
            
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
            if (contactId == "itDept")
            {
                ChangeToNextState();
            }
        }
    }
}