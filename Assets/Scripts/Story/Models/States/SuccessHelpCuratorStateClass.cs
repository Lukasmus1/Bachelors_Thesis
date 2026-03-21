using System;
using Apps.ChatTerminal.Commons;
using Apps.CompilationHelper.Commons;
using Commons;
using Desktop.Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
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
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFightForCuratorSuccess", true);
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "curatorFightForCuratorSuccess", true);
            
            string path = UserMvc.Instance.UserController.ProceduralData(UserDataType.KpLocation);
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

        private void OnFileDeletion(FileDeletionDetectionModel detectionModel)
        {
            Object.Destroy(detectionModel);
            
            ChatTerminalMvc.Instance.ChatTerminalController.UnloadProfile("kp");
            
            ChangeToNextState();
        }
    }
}