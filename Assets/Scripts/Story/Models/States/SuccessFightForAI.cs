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
    public class SuccessFightForAI : StateClass
    {
        public override int State { get; } = (int)StatesEnum.SuccessFightForAI;
        public override int NextState { get; set; } = (int)StatesEnum.UploadAI;
            
        public override void OnEnter()
        {
            // ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("kp");
            // ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("curator");
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFightForAISuccess", true);
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "curatorFightForAISuccess", true);
            
            string path = UserMvc.Instance.UserController.ProceduralData(UserDataType.CuratorLocation);
            string content = FourthWallMvc.Instance.FileGenerationController.GenerateRandomText(200);
            
            FourthWallMvc.Instance.FileGenerationController.CreateFile(path, content, false);

            //Attach file deletion detection
            GameObject scriptHolder = Tools.GetScriptHolder();
            var detectionModel = scriptHolder.AddComponent<FileDeletionDetectionModel>();
            detectionModel.StartDetection(path, () =>
            {
                OnFileDeletion(detectionModel);
            });
        }

        public override void OnExit()
        {
            //todo
        }

        public override void LoadFromState()
        {
            //todo
        }

        private void OnFileDeletion(FileDeletionDetectionModel model)
        {
            Object.Destroy(model);
            
            ChatTerminalMvc.Instance.ChatTerminalController.UnloadProfile("curator");
            
            ChangeToNextState();
        }
    }
}