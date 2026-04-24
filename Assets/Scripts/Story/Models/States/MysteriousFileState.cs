using System;
using Apps.Commons.FileScripts;
using Apps.FileManager.Commons;
using Saving.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class MysteriousFileState : StateClass
    {
        public override int State => (int)StatesEnum.MysteriousFile;
        public override int NextState { get; set; } = (int)StatesEnum.AfterCrash;
        
        public override void OnEnter()
        {
            FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("MysteriousFile", true);
            
            //Create a screenshot of the desktop and save it for later in the story
            FileManagerMvc.Instance.FileManagerController.CreateUsersScreenshotFile();
            
            LoadFromState();
        }

        public override void OnExit()
        {
            MysteriousFileInteractionHandler.ClickedOnLink -= OnMysteriousFileLinkClicked;
        }

        public override void LoadFromState()
        {
            MysteriousFileInteractionHandler.ClickedOnLink += OnMysteriousFileLinkClicked;
        }

        private void OnMysteriousFileLinkClicked()
        {
            //What should happen when the link in the mysterious file is clicked
            FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("MysteriousFile", false);
            FileManagerMvc.Instance.FileManagerController.ToggleFileVisibility("CypherCode", true);
            
            ChangeToNextState();
            
            SavingMvc.Instance.SavingController.QuitAndSaveGame();
        }
    }
}