using System;
using Apps.ChatTerminal.Commons;
using FourthWall.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class FailedFightForAIStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FailedFightForAI;
        public override int NextState { get; set; } = (int)StatesEnum.FailedFightForAIEnding;

        public override void OnEnter()
        {
            FourthWallMvc.Instance.CompilationSimulationController.DeleteKpCompilationFolder();
         
            ChatTerminalMvc.Instance.ChatTerminalController.UnloadProfile("kp");
            
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "curatorFightForAIFail", true);
            
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
            if (messageID != "curatorFightForAIFailEnd")
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}