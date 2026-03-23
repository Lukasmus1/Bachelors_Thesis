using System;
using Apps.ChatTerminal.Commons;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using UnityEngine;
using User.Commons;
using User.Models;
using Object = UnityEngine.Object;

namespace Story.Models.States
{
    [Serializable]
    public class SuccessFightForAIStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.SuccessFightForAI;
        public override int NextState { get; set; } = (int)StatesEnum.UploadAI;
            
        public override void OnEnter()
        {
            //debug
            // ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("kp");
            // ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("curator");
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFightForAISuccess", true);
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "curatorFightForAISuccess", true);
            
            string path = UserMvc.Instance.UserController.ProceduralData(UserDataType.CuratorLocation);
            string content = FourthWallMvc.Instance.FileGenerationController.GenerateRandomText(200);
            
            FourthWallMvc.Instance.FileGenerationController.CreateFile(path, content, false);

            FourthWallMvc.Instance.FileGenerationController.SetupFileDeletion(path, OnFileDeletion);
        }

        public override void OnExit()
        {
            //todo
        }

        public override void LoadFromState()
        {
            //todo
        }

        private void OnFileDeletion()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.UnloadProfile("curator");
            
            ChangeToNextState();
        }
    }
}