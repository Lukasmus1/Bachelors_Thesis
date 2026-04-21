using System;
using Apps.ChatTerminal.Commons;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using UnityEngine;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class SuccessFightForAIStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.SuccessFightForAI;
        public override int NextState { get; set; } = (int)StatesEnum.UploadAI;
        
        private string _path;
            
        public override void OnEnter()
        {
            //debug
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("kp");
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("curator");
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFightForAISuccess", true);
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "curatorFightForAISuccess", true);
            
            _path = UserMvc.Instance.UserController.ProceduralData(UserDataType.CuratorLocation);
            string content = FourthWallMvc.Instance.FileGenerationController.GenerateRandomText(200);
            
            FourthWallMvc.Instance.FileGenerationController.CreateFile(_path, content, false);

            LoadFromState();
        }

        public override void OnExit()
        {
        }

        public override void LoadFromState()
        {
            FourthWallMvc.Instance.FileGenerationController.SetupFileDeletion(_path, OnFileDeletion);
        }

        private void OnFileDeletion()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.UnloadProfile("curator");
            
            ChangeToNextState();
        }
    }
}