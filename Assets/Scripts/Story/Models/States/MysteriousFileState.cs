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
        public override int NextState => (int)StatesEnum.AfterCrash;
        
        public override void OnEnter()
        {
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("MysteriousFile", true);
            
            //Create a screenshot of the desktop and save it for later in the story
            FileLoaderMvc.Instance.FileLoaderController.CreateUsersScreenshotFile();
            
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
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("MysteriousFile", false);
            
            ChangeToNextState();
            
            SavingMvc.Instance.SavingController.QuitAndSaveGame();
        }
    }
}