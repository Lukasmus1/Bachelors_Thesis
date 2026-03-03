using System;
using Apps.ChatTerminal.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class HelpChoiceStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.HelpChoice;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("headOfDpt");
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += ContinuationCheck;
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= ContinuationCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= QuestionCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += ContinuationCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += QuestionCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
        }

        private void ContinuationCheck(string messageID)
        {
            if (messageID != "dptLastTask")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("curator");
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= ContinuationCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += QuestionCheck;
        }
        
        private void QuestionCheck(string messageID)
        {
            if (messageID != "curatorLastDetective")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("headOfDpt");
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= QuestionCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
        }

        private void TransitionCheck(string messageID)
        {
            if (messageID != "dptChoiceEnd")
            {
                return;
            }

            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;

            ChangeToNextState();
        }
    }
}