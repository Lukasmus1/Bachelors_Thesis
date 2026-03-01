using System;
using Apps.ChatTerminal.Commons;
using Apps.CipherSolver.Commons;
using Apps.FileManager.Commons;
using Apps.FileViewer.Commons;
using Commons;
using Saving.Commons;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class DetectiveStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.Detective;
        public override int NextState { get; set; } = (int)StatesEnum.FirstChoice;

        [NonSerialized] private AsyncTimer t1;
        private bool emailOpened = false;
        [NonSerialized] private AsyncTimer t2;
        private bool emailTwoOpened = false;
        [NonSerialized] private AsyncTimer t3;
        private bool messagesOpened = false;
        
        public override void OnEnter()
        {
            //Puzzle files
            FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("DetectiveEmail", true);
            FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("DetectiveEmailTwo", true);
            FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("DetectiveMessages", true);
            
            //Encrypted image
            FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("UserScreenshot", true);
                
            LoadFromState();
        }

        public override void OnExit()
        {
            CipherMvc.Instance.CipherController.onDecryptionAttempt -= TransitionCheck;
            
            t1?.Dispose();
            t2?.Dispose();
            t3?.Dispose();

            UserMvc.Instance.UserController.ClearScreenshot();
        }

        public override void LoadFromState()
        {
            CipherMvc.Instance.CipherController.onDecryptionAttempt += TransitionCheck;
            
            if (!emailOpened)
                FileViewerMvc.Instance.FileLoaderController.onFileOpened += OnEmailOpened;
            if (!emailTwoOpened)
                FileViewerMvc.Instance.FileLoaderController.onFileOpened += OnEmailTwoOpened;
            if (!messagesOpened)
                FileViewerMvc.Instance.FileLoaderController.onFileOpened += OnMessagesOpened;
            
            t1 = new AsyncTimer();
            t2 = new AsyncTimer();
            t3 = new AsyncTimer();
        }

        private void OnEmailOpened(string fileName)
        {
            if (fileName != "Scammer's Email")
            {
                return;
            }
            
            _ = t1.StartTimer(1, () =>
            {
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "openedEmail");
                FileViewerMvc.Instance.FileLoaderController.onFileOpened -= OnEmailOpened;
                emailOpened = true;
            });
        }
        
        private void OnEmailTwoOpened(string fileName)
        {
            if (fileName != "Scammer's Email Two")
            {
                return;
            }
            
            _ = t2.StartTimer(1, () =>
            {
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "openedEmailTwo");
                FileViewerMvc.Instance.FileLoaderController.onFileOpened -= OnEmailTwoOpened;
                emailTwoOpened = true;
            });
        }
        
        private void OnMessagesOpened(string fileName)
        {
            if (fileName != "Scammer's Messages")
            {
                return;
            }
            
            _ = t3.StartTimer(1, () =>
            {
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "openedMessages");
                FileViewerMvc.Instance.FileLoaderController.onFileOpened -= OnMessagesOpened;
                messagesOpened = true;
            });
        }
        
        private void TransitionCheck(string keyAttempt)
        {
            if (keyAttempt != UserMvc.Instance.UserController.ProceduralData(UserDataType.PictureCode))
            {
                return;
            }
            
            var t = new AsyncTimer();
            _ = t.StartTimer(5, () =>
            {
                ChangeToNextState();
                SavingMvc.Instance.SavingController.QuitAndSaveGame();
            });
            
        }
    }
}