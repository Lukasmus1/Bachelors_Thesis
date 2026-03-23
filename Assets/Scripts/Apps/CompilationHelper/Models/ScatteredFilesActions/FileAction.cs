using System;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Apps.CompilationHelper.Models.ScatteredFilesActions
{
    public abstract class FileAction
    {
        protected FileAction(string fileLocation)
        {
            FileLocation = fileLocation;
        }
        protected string FileLocation { get; private set; }
        
        protected void StartFileDeletionDetection()
        {
            FourthWallMvc.Instance.FileGenerationController.SetupFileDeletion(FileLocation, OnFileDeletion);
        }
        
        public abstract void OnLoad();

        private void OnFileDeletion()
        {
            onDeleteFile?.Invoke();
        }

        protected void CreateFile()
        {
            string fileContent = FourthWallMvc.Instance.FileGenerationController.GenerateFileData(); 
            FourthWallMvc.Instance.FileGenerationController.CreateFile(FileLocation, fileContent, false);
            
            StartFileDeletionDetection();
        }
        
        public Action onDeleteFile;
    }
}