using System;
using Apps.CipherSolver.Commons;
using Apps.FileManager.Commons;
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
        }

        public override void LoadFromState()
        {
            CipherMvc.Instance.CipherController.OnDecryptionAttempt += TransitionCheck;
        }

        private void TransitionCheck(string keyAttempt)
        {
            if (keyAttempt != UserMvc.Instance.UserController.ProceduralData(UserDataType.PictureCode))
            {
                return;
            }
            
            //What should happen after he decrypts the image
        }
    }
}