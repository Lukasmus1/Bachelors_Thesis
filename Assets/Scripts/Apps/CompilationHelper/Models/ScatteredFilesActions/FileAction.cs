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

        private FileDeletionDetectionModel _fileDeleteDetection;
        protected void StartFileDeletionDetection()
        {
            GameObject scriptHolder = Tools.GetScriptHolder();
            _fileDeleteDetection = scriptHolder.AddComponent<FileDeletionDetectionModel>();
            _fileDeleteDetection.StartDetection(FileLocation, OnFileDeletion);
        }
        
        public abstract void OnLoad();

        private void OnFileDeletion()
        {
            Object.Destroy(_fileDeleteDetection);
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