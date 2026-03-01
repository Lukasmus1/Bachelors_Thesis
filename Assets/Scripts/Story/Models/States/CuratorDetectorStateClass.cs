using System;
using System.IO;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using Story.Models.Actions;
using UnityEngine;
using User.Commons;
using User.Models;
using Object = UnityEngine.Object;

namespace Story.Models.States
{
    [Serializable]
    public class CuratorDetectorStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.CuratorDetector;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            // If the player didn't delete the file, K-P will get angry
            if (File.Exists(UserMvc.Instance.UserController.CuratorExplanationFilePath))
            {
                ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpDesktopFile");
                UserMvc.Instance.UserController.IncreaseCopsAlignment(Alignment.AIDesktopFile);
            }

            AsyncTimer t = new();
            _ = t.StartTimer(2, () =>
            {
                LoadFromState();
                
                FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("NumberPattern", true);
                FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("FileLocation", true);
                
                ActionsClass.Instance.PerformAction(ActionType.ImportantFile);
                
                t.Dispose();
            });
        }

        public override void OnExit()
        {
            ImportantFile.OnImportantFileDeleted -= ChangeToNextState;
        }

        public override void LoadFromState()
        {
            FourthWallMvc.Instance.FileGenerationController.ThrowWindowsDialog(DialogType.Warning, "It is no longer safe for us, read the text I have put in your clipboard.", "...");
            
            GUIUtility.systemCopyBuffer =
                ChatTerminalMvc.Instance.ChatTerminalController.GetSecondaryMessageGroupConcat("curator",
                    "curatorHiddenFile");
            
            ImportantFile.OnImportantFileDeleted += ChangeToNextState;
        }
    }
}