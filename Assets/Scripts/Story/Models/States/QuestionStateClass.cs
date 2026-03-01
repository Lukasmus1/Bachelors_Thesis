using System;
using Apps.ChatTerminal.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class QuestionStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.Question;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("curator");
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= ContinuationCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += ContinuationCheck;
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
    }
}