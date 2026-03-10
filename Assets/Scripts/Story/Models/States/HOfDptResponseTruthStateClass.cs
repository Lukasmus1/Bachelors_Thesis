using System;
using Apps.ChatTerminal.Commons;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;

namespace Story.Models.States
{
    [Serializable]
    public class HOfDptResponseTruthStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.HOfDptResponseTruth;
        public override int NextState { get; set; } = (int)StatesEnum.CuratorDetector;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "dptTruthConsequence");
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += FirstConnectionAttempt;
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= FirstConnectionAttempt;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= SecondConnectionAttempt;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += FirstConnectionAttempt;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += SecondConnectionAttempt;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
        }
        
        private void FirstConnectionAttempt(string messageID)
        {
            if (messageID != "dptConTruthAttempt1")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(true);
            
            var t = new AsyncTimer();
            _ = t.StartTimer(2, () =>
            {
                FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "No",
                    "Unknown Connection Attempt");

                ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= FirstConnectionAttempt;
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt",
                    "dptTruthConsequence2");
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped += SecondConnectionAttempt;
                
                t.Dispose();
            });
        }
        
        private void SecondConnectionAttempt(string messageID)
        {
            if (messageID != "dptConTruthAttempt2")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(true);
            
            var t = new AsyncTimer();
            _ = t.StartTimer(2, () =>
            {
                FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "No, you won't",
                    "Unknown Connection Attempt");

                ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= SecondConnectionAttempt;
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt",
                    "dptTruthConsequence3");
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
                
                t.Dispose();
            });
        }
        
        private void TransitionCheck(string messageID)
        {
            if (messageID != "dptConTruthDone")
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}