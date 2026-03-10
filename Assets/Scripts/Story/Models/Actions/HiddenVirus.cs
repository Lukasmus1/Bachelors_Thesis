using System;
using System.IO;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using Story.Models.SideActions;
using UnityEngine;
using User.Commons;
using User.Models;
using Object = UnityEngine.Object;

namespace Story.Models.Actions
{
    public static class HiddenVirus
    {
        //Hidden Virus Action
        private static AsyncTimer _t1;
        private static FileDeletionDetectionModel _fileDeleteDetection;
        
        /// <summary>
        /// Definition for starting the timer for hidden virus cancellation. Uses recursion.
        /// </summary>
        /// <param name="fileName">Name of the hidden file</param>
        private static void StartTimerForHiddenVirusCancellation(string fileName)
        {
            _t1 = new AsyncTimer();
            _ = _t1.StartTimer(120, () =>
            {
                if (_fileDeleteDetection.IsFileDeleted())
                {
                    OnFileDeletion();
                    return;
                }
                if (_fileDeleteDetection == null)
                {
                    return;
                }
                
                var message = $"Windows detected a hidden corrupted file in your system: {Path.GetFullPath(fileName)}";
                FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Error, message, "File Corruption Error");
                
                StartTimerForHiddenVirusCancellation(fileName);
            });
        }
        
        /// <summary>
        /// Performs the hidden virus action.
        /// </summary>
        /// <exception cref="Exception">Gets thrown only when I change the tag on ScriptHolder object</exception>
        public static void PerformHiddenVirusAction()
        {
            //Create hidden virus file
            string fileName = UserMvc.Instance.UserController.ProceduralData(UserDataType.VirusName);
            string fileContent = UserMvc.Instance.UserController.ProceduralData(UserDataType.VirusContent);
            FourthWallMvc.Instance.FileGenerationController.CreateFile(fileName, fileContent, true);

            //Attach file deletion detection
            GameObject scriptHolder = Tools.GetScriptHolder();
            _fileDeleteDetection = scriptHolder.AddComponent<FileDeletionDetectionModel>();
            _fileDeleteDetection.StartDetection(fileName, OnFileDeletion);
            
            //Start timer
            StartTimerForHiddenVirusCancellation(fileName);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// What happens when the file gets deleted. Cleans up the action and messages the player about the successful deletion.
        /// </summary>
        private static void OnFileDeletion()
        {
            CleanupHiddenVirusAction();
            
            //Cleanup action flag
            ActionsClass.Instance.ActionsPersistent.SetAction(ActionType.HiddenVirus, false);
            
            KpDoNotDeleteFiles.MessagePlayer();
        }
        
        /// <summary>
        /// Cleanup after the hidden virus action.
        /// </summary>
        public static void CleanupHiddenVirusAction()
        {
            _t1.Dispose();
            
            if (_fileDeleteDetection)
            {
                Object.Destroy(_fileDeleteDetection);
                _fileDeleteDetection = null;
            }
            
            //Destroy hidden virus file
            string fileName = UserMvc.Instance.UserController.ProceduralData(UserDataType.VirusName);
            FourthWallMvc.Instance.FileGenerationController.DestroyFile(fileName);
        }
    }
}