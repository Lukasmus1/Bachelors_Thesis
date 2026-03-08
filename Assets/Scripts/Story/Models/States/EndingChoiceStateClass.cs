using System;
using Apps.ChatTerminal.Commons;
using Story.Commons;
using User.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class EndingChoiceStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.EndingChoice;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            int maxValue = StoryMvc.Instance.StoryController.GetExtremeAlignment(true);
            int minValue = StoryMvc.Instance.StoryController.GetExtremeAlignment(false);
            
            if (UserMvc.Instance.UserController.GetCuratorAlignment() == maxValue)
            {
                //todo //The player hates the AI, doesn't get a choice
                
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "playerHateAI", true);
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped += PlayerLoveAIBonus;
            }
            else if (UserMvc.Instance.UserController.GetCuratorAlignment() == minValue)
            {
                //todo //The player loves the AI, doesn't get a choice
                
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "playerLoveAI", true);
            }
            else
            {
                //todo //The player is neutral, gets a choice
                
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpPleading", true);
            }
            
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

        public void TransitionCheck(string messageID)
        {
            if (messageID is "kpPleadingHelpEnd" or "kpThankingEnd")
            {
                
            }
            else if (messageID is "kpPleadingDeleteEnd" or "playerHateAIEnd")
            {
                
            }
        }

        public void PlayerLoveAIBonus(string messageID)
        {
            if (messageID is not "playerLoveAIEnd")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpThanking", true);
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= PlayerLoveAIBonus;
        }
    }
}