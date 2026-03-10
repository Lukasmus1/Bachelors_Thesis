using System;
using Apps.ChatTerminal.Commons;
using FourthWall.Commons;
using UnityEditor;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class UploadAIStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.UploadAI;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFightForAIUpload", true);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= MessageCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += MessageCheck;
        }
        
        private void MessageCheck(string messageID)
        {
            if (messageID == "kpFightForAIUploadZip")
            {
                FourthWallMvc.Instance.CompilationSimulationController.CreateCompiledZipFile();
            }
            else if (messageID == "kpFightForAIUploadUrlCopy")
            {
                EditorGUIUtility.systemCopyBuffer = UserMvc.Instance.UserController.ProceduralData(UserDataType.AiUploadUrl);
            }
        }
    }
}