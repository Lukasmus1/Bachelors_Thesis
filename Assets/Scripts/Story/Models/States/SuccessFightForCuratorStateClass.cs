using System;
using System.IO;
using Apps.ChatTerminal.Commons;
using Commons;
using Desktop.Commons;
using FourthWall.Commons;
using Saving.Commons;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class SuccessFightForCuratorStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.SuccessFightForCurator;
        public override int NextState { get; set; } = (int)StatesEnum.SuccessFightForCuratorEnding;
        
        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFightForCuratorSuccess", true);
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "curatorFightForCuratorSuccess", true);
            
            string fullPath = UserMvc.Instance.UserController.ProceduralData(UserDataType.KpLocation);
            string content = FourthWallMvc.Instance.FileGenerationController.GenerateRandomText(200);
            
            FourthWallMvc.Instance.FileGenerationController.CreateFile(fullPath, content, false);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
            
            DesktopMvc.Instance.DesktopGeneratorController.ToggleIcon("Compilation Helper", false);
            DesktopMvc.Instance.DesktopGeneratorController.CloseApp("CompilationHelper");
        }

        public override void LoadFromState()
        {
            string path = UserMvc.Instance.UserController.ProceduralData(UserDataType.KpLocation);
            
            FourthWallMvc.Instance.FileGenerationController.SetupFileDeletion(path, OnFileDeletion);
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
        }

        private void OnFileDeletion()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.UnloadProfile("kp");

            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "curatorEnding", true);
        }

        private void TransitionCheck(string messageID)
        {
            if (messageID != "curatorEndingEnd")
                return;
            
            var t = new AsyncTimer();
            _ = t.StartTimer(3, () =>
            {
                ChangeToNextState();
            
                SavingMvc.Instance.SavingController.QuitAndSaveGame();

                t.Dispose();
            });
        }
    }
}