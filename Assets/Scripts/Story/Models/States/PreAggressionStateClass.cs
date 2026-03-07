using System;
using Apps.ChatTerminal.Commons;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using FourthWall.UserInformation.Models;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class PreAggressionStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.PreAggression;
        public override int NextState { get; set; } = (int)StatesEnum.Aggression;

        public override void OnEnter()
        {
            LoadFromState();
        }

        //Not needed this time
        public override void OnExit()
        {}

        public override void LoadFromState()
        {
            var checker = Tools.GetScriptHolder().AddComponent<UserTimeChecker>();
            
            checker.StartTimeChecking("0:00", "0:30", () =>
            {
                FourthWallMvc.Instance.FileGenerationController.CreateLastImportantFile();
                
                ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("curator");
                
                var fileChecker = Tools.GetScriptHolder().AddComponent<FileDeletionDetectionModel>();
                
                string filePath = UserMvc.Instance.UserController.ProceduralData(UserDataType.LastFileLocation);
                fileChecker.StartDetection(filePath, () =>
                {
                    UnityEngine.Object.Destroy(fileChecker);
                    ChangeToNextState();
                });
            });
        }
    }
}