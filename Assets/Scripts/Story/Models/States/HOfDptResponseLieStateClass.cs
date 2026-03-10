using System;
using Apps.ChatTerminal.Commons;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;

namespace Story.Models.States
{
    [Serializable]
    public class HOfDptResponseLieStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.HOfDptResponseLie;
        public override int NextState { get; set; } = (int)StatesEnum.CuratorDetector;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "dptLieConsequence");
            
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
            if (messageID != "dptConLieAttempt1")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(true);
            
            var t = new AsyncTimer();
            _ = t.StartTimer(2, () =>
            {
                FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "An unknown connection attempt was detected and prevented", "Unknown Connection Attempt");
            
                ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= FirstConnectionAttempt;
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "dptLieConsequence2");
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped += SecondConnectionAttempt;
                
                t.Dispose();
            });
        }

        private void SecondConnectionAttempt(string messageID)
        {
            if (messageID != "dptConLieAttempt2")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(true);
            
            var t = new AsyncTimer();
            _ = t.StartTimer(2, () =>
            {
                FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info,
                    "An unknown connection attempt was detected and prevented", "Unknown Connection Attempt");

                ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= SecondConnectionAttempt;
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt",
                    "dptLieConsequence3");
                ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
                
                t.Dispose();
            });
        }
        
        private void TransitionCheck(string messageID)
        {
            if (messageID != "dptConLieDone")
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}