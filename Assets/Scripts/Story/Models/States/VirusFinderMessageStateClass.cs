using System;
using Apps.ChatTerminal.Commons;
using Story.Models.Actions;

namespace Story.Models.States
{
    [Serializable]
    public class VirusFinderMessageStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.VirusFinderMessage;
        public override int NextState { get; } = (int)StatesEnum.VirusFinder;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("itDept");
            
            //Perform Hidden Virus Action
            ActionsClass.Instance.PerformAction(ActionType.HiddenVirus);
            
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
            if (messageID == "itVirusFinderInstalled")
            {
                ChangeToNextState();
            }
        }
    }
}