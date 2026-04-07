using System;
using Apps.ChatTerminal.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class QuestionStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.Question;
        public override int NextState { get; set; } = (int)StatesEnum.HelpChoice;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.ChangeUsername("curator", "Curator");
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("curator");
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("curator");
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= ContinuationCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += ContinuationCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
        }

        private void ContinuationCheck(string messageID)
        {
            if (messageID != "curatorQuestionEnd")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("curator");
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= ContinuationCheck;
        }

        private void TransitionCheck(string messageID)
        {
            if (messageID != "curatorNextStep")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
            
            ChangeToNextState();
        }
    }
}