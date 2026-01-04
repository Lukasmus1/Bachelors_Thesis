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
        public override int NextState => (int)StatesEnum.Default;
        
        public override void OnEnter()
        {
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("MysteriousFile", true);
            
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
            
            
            ChangeState();
            
            SavingMvc.Instance.SavingController.QuitAndSaveGame();
        }
    }
}