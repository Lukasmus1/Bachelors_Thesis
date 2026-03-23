using System;
using Apps.ChatTerminal.Commons;
using Apps.FileUploader.Commons;
using Desktop.Commons;
using FourthWall.Commons;
using UnityEditor;
using UnityEngine;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class UploadAIStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.UploadAI;
        public override int NextState { get; set; } = (int)StatesEnum.SuccessFightForAIEnding;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("kp");
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFightForAIUpload", true);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= MessageCheck;
            FileUploaderMvc.Instance.FileUploaderController.OnSuccessfulUpload -= OnUploadCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += MessageCheck;
        }
        
        private void MessageCheck(string messageID)
        {
            switch (messageID)
            {
                case "kpFightForAIUploadZip":
                    FourthWallMvc.Instance.CompilationSimulationController.CreateCompiledZipFile();
                    break;
                case "kpFightForAIUploadUrlCopy":
                    string textToCopy = UserMvc.Instance.UserController.ProceduralData(UserDataType.AiUploadUrl);
                    FourthWallMvc.Instance.CommonsController.CopyToClipboard(textToCopy);
                    break;
                case "kpFightForAIUploadFileUploader":
                    DesktopMvc.Instance.DesktopGeneratorController.ToggleIcon("File Uploader", true);
                    FileUploaderMvc.Instance.FileUploaderController.OnSuccessfulUpload += OnUploadCheck;
                    break;
            }
        }

        private void OnUploadCheck(string fileName)
        {
            if (fileName != "confirmationFile.con")
            {
                return;
            }
            
            ChangeToNextState();
        }
    }
}