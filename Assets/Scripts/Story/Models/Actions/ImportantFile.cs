using System;
using FourthWall.Commons;
using User.Commons;
using User.Models;

namespace Story.Models.Actions
{
    public static class ImportantFile
    {
        public static Action OnImportantFileDeleted;
        
        public static void SetupFileDeletionDetection()
        {
            string filePath = UserMvc.Instance.UserController.ProceduralData(UserDataType.ImportantFileLocation);
            FourthWallMvc.Instance.FileGenerationController.SetupFileDeletion(filePath, OnDeletion);
        }

        private static void OnDeletion()
        {
            ActionsClass.Instance.ActionsPersistent.SetAction(ActionType.ImportantFile, false);
            UserMvc.Instance.UserController.SetPersistentData(UserDataType.ImportantFileDeleted, true);
            
            OnImportantFileDeleted?.Invoke();
        }
        
    }
}