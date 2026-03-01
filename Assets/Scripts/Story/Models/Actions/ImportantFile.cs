using System;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using UnityEngine;
using User.Commons;
using User.Models;
using Object = UnityEngine.Object;

namespace Story.Models.Actions
{
    public static class ImportantFile
    {
        private static FileDeletionDetectionModel _fileDeleteDetection;
        
        public static Action OnImportantFileDeleted;
        
        public static void SetupFileDeletionDetection()
        {
            GameObject scriptHolder = GameObject.FindWithTag("ScriptHolder");
            if (!scriptHolder)
            {
                throw new Exception("ScriptHolder not found in scene. Cannot attach FileDeletionDetectionModel.");
            }
            _fileDeleteDetection = scriptHolder.AddComponent<FileDeletionDetectionModel>();
            string filePath = UserMvc.Instance.UserController.ProceduralData(UserDataType.ImportantFileLocation);
            _fileDeleteDetection.StartDetection(filePath, OnDeletion);
        }

        private static void OnDeletion()
        {
            if (_fileDeleteDetection)
            {
                Object.Destroy(_fileDeleteDetection);
                _fileDeleteDetection = null;
            }
            
            ActionsClass.Instance.ActionsPersistent.SetAction(ActionType.ImportantFile, false);
            
            OnImportantFileDeleted?.Invoke();
        }
        
    }
}