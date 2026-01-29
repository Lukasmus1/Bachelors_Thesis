using System;
using Apps.ChatTerminal.Commons;
using Apps.VirusFinder.Commons;
using FourthWall.Commons;
using Story.Models.Actions;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class AfterCrashStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.AfterCrash;
        public override int NextState { get; } = (int)StatesEnum.VirusFinderMessage;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("headOfDpt", 3);
            
            //Perform Hidden Virus Action
            ActionsClass.Instance.PerformAction(ActionType.HiddenVirus);
            
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