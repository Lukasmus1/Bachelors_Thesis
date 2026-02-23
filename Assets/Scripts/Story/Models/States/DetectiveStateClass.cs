using System;
using System.Collections;
using Apps.ChatTerminal.Commons;
using Apps.CipherSolver.Commons;
using Apps.FileManager.Commons;
using Apps.FileViewer.Commons;
using Commons;
using UnityEngine;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class DetectiveStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.Detective;
        public override int NextState { get; } = (int)StatesEnum.FirstChoice;

        [NonSerialized] private AsyncTimer t1;
        [NonSerialized] private AsyncTimer t2;
        [NonSerialized] private AsyncTimer t3;
        
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
            CipherMvc.Instance.CipherController.onDecryptionAttempt -= TransitionCheck;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened -= OnEmailOpened;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened -= OnEmailTwoOpened;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened -= OnMessagesOpened;
            
            t1?.Dispose();
            t2?.Dispose();
            t3?.Dispose();
        }

        public override void LoadFromState()
        {
            CipherMvc.Instance.CipherController.onDecryptionAttempt += TransitionCheck;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened += OnEmailOpened;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened += OnEmailTwoOpened;
            FileViewerMvc.Instance.FileLoaderController.onFileOpened += OnMessagesOpened;
            
            t1 = new AsyncTimer();
            t2 = new AsyncTimer();
            t3 = new AsyncTimer();
        }

        private void OnEmailOpened(string fileName)
        {
            _ = t1.StartTimer(5, () =>
            {
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "openedEmail");
            });
        }
        
        private void OnEmailTwoOpened(string fileName)
        {
            _ = t2.StartTimer(5, () =>
            {
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "openedEmailTwo");
            });
        }
        
        private void OnMessagesOpened(string fileName)
        {
            _ = t3.StartTimer(5, () =>
            {
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("headOfDpt", "openedMessages");
            });
        }
        
        private void TransitionCheck(string keyAttempt)
        {
            if (keyAttempt != UserMvc.Instance.UserController.ProceduralData(UserDataType.PictureCode))
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}