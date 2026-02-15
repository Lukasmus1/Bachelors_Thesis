using System;
using Apps.ChatTerminal.Commons;
using Apps.CipherSolver.Commons;
using Apps.FileManager.Commons;
using Apps.FileViewer.Commons;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class DetectiveStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.Detective;
        public override int NextState { get; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            //Puzzle files
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("DetectiveEmail", true);
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("DetectiveEmailTwo", true);
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("DetectiveMessages", true);
            
            //Encrypted image
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("UserScreenshot", true);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            CipherMvc.Instance.CipherController.OnDecryptionAttempt -= TransitionCheck;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened -= OnEmailOpened;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened -= OnEmailTwoOpened;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened -= OnMessagesOpened;
        }

        public override void LoadFromState()
        {
            CipherMvc.Instance.CipherController.OnDecryptionAttempt += TransitionCheck;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened += OnEmailOpened;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened += OnEmailTwoOpened;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened += OnMessagesOpened;
        }

        private void OnEmailOpened(string fileName)
        {
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "openedEmail");
        }
        
        private void OnEmailTwoOpened(string fileName)
        {
            
        }
        
        private void OnMessagesOpened(string fileName)
        {
            
        }
        
        private void TransitionCheck(string keyAttempt)
        {
            if (keyAttempt != UserMvc.Instance.UserController.ProceduralData(UserDataType.PictureCode))
            {
                return;
            }
            
            //What should happen after they decrypt the image
        }
    }
}