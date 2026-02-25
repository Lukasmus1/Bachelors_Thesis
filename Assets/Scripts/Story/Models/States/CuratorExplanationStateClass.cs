using System;
using Apps.ChatTerminal.Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using User.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class CuratorExplanationStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.CuratorExplanation;
        public override int NextState { get; } = (int)StatesEnum.Default;
        
        public override void OnEnter()
        {
            string content = ChatTerminalMvc.Instance.ChatTerminalController.GetSecondaryMessageGroupConcat("curator", "curatorExplanation");
            string path = UserMvc.Instance.UserController.CuratorExplanationFilePath;
            
            FourthWallMvc.Instance.FileGenerationController.CreateFile(path, content, false);
            FourthWallMvc.Instance.FileGenerationController.ThrowWindowsDialog(DialogType.Warning, $"An unknown file has been found: {path}", "New File");
            
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