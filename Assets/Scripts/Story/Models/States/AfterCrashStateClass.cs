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
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("headOfDpt");
            
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
            if (messageID == "dptMsgIT")
            {
                ChangeToNextState();
            }
        }
    }
}