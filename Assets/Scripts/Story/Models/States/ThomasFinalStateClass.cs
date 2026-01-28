using System;
using Apps.ChatTerminal.Commons;
using Apps.VirusFinder.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class ThomasFinalCleanupStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.ThomasFinal;
        public override int NextState { get; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("thomasBare", 1);
            
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
            if (contactId == "thomasBare")
            {
                ChangeToNextState();
            }
        }
    }
}