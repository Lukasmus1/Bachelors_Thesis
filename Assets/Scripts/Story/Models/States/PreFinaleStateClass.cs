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
    public class PreFinaleStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.PreFinale;
        public override int NextState { get; set; } = (int)StatesEnum.EndingChoice;

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
                
                string filePath = UserMvc.Instance.UserController.ProceduralData(UserDataType.LastFileLocation);
                FourthWallMvc.Instance.FileGenerationController.SetupFileDeletion(filePath, ChangeToNextState);
            });
        }
    }
}