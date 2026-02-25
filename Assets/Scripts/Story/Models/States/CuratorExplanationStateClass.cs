using System;
using Apps.ChatTerminal.Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class CuratorExplanationStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.CuratorExplanation;
        public override int NextState { get; set; } = (int)StatesEnum.Default;
        
        public override void OnEnter()
        {
            string content = ChatTerminalMvc.Instance.ChatTerminalController.GetSecondaryMessageGroupConcat("curator", "curatorExplanation");
            string path = UserMvc.Instance.UserController.CuratorExplanationFilePath;
            
            FourthWallMvc.Instance.FileGenerationController.CreateFile(path, content, false);
            FourthWallMvc.Instance.FileGenerationController.ThrowWindowsDialog(DialogType.Warning, $"An unknown file has been found: {path}", "New File");
            
            // Next state is dependent on the player's choice in the first choice
            if (UserMvc.Instance.UserController.GetPersistentData(UserDataType.FirstChoiceSideWithCops))
            {
                NextState = (int)StatesEnum.HOfDptResponseTruth;
            }
            else
            {
                NextState = (int)StatesEnum.HOfDptResponseLie;
            }
            
            ChangeToNextState();
        }

        public override void OnExit()
        {
            // Nothing needed so far
        }

        public override void LoadFromState()
        {
            // The player should really not be able to load into this state, but if they do, we can just re-run the OnEnter logic to re-create the file and show the dialog again
            OnEnter();
        }
    }
}