using System;
using Apps.ChatTerminal.Commons;
using Apps.CompilationHelper.Commons;
using Commons;
using Desktop.Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using Saving.Commons;
using UnityEngine;
using User.Commons;
using User.Models;
using Object = UnityEngine.Object;

namespace Story.Models.States
{
    [Serializable]
    public class SuccessHelpCuratorStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.SuccessFightForCurator;
        public override int NextState { get; set; } = (int)StatesEnum.FightForCuratorEnding;
        
        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFightForCuratorSuccess", true);
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "curatorFightForCuratorSuccess", true);
            
            string path = UserMvc.Instance.UserController.ProceduralData(UserDataType.KpLocation);
            string content = FourthWallMvc.Instance.FileGenerationController.GenerateRandomText(200);
            
            FourthWallMvc.Instance.FileGenerationController.CreateFile(path, content, false);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
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
            
            ChangeToNextState();
            
            SavingMvc.Instance.SavingController.QuitAndSaveGame();
        }
    }
}