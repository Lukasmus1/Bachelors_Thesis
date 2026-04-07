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
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "playerHateAI", true);
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped += PlayerLoveAIBonus;
            }
            else if (UserMvc.Instance.UserController.GetCuratorAlignment() == minValue)
            { 
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "playerLoveAI", true);
            }
            else
            { 
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
            switch (messageID)
            {
                case "kpPleadingHelpEnd" or "kpThankingEnd":
                    NextState = (int)StatesEnum.EndingFightForAI;
                    ChangeToNextState();
                    break;
                case "kpPleadingDeleteEnd" or "playerHateAIEnd":
                    NextState = (int)StatesEnum.EndingFightForCurator;
                    ChangeToNextState();
                    break;
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