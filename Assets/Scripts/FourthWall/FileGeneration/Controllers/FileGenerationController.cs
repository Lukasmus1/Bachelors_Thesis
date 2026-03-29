using System;
using Commons;
using FourthWall.FileGeneration.Models;
using UnityEngine;

namespace FourthWall.FileGeneration.Controllers
{
    public class FileGenerationController
    {
        private readonly FileCreationModel _model = new();

        /// <summary>
        /// Creates a file with the specified name and content. Optionally it could be a hidden file.
        /// </summary>
        /// <param name="fileName">Path of the file</param>
        /// <param name="content">Content of the file</param>
        /// <param name="hidden">Should the file be hidden?</param>
        public void CreateFile(string fileName, string content, bool hidden)
        {
            _model.CreateHiddenFile(fileName, content, hidden);
        }
        
        /// <summary>
        /// Destroys the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        public void DestroyFile(string fileName)
        {
            _model.DestroyFile(fileName);
        }
        
        /// <summary>
        /// Destroys the specified folder and all of its contents.
        /// </summary>
        /// <param name="folderPath">Path of the folder</param>
        public void DestroyFolder(string folderPath)
        {
            _model.DestroyFolder(folderPath);
        }

        /// <summary>
        /// Generates a random file name from a predefined list.
        /// </summary>
        /// <returns>Random file name</returns>
        public string GenerateRandomFileName()
        {
            return _model.GenerateRandomFileName();
        }

        /// <summary>
        /// Generates file data content.
        /// </summary>
        /// <returns>File content in base64</returns>
        public string GenerateFileData()
        {
            return _model.GenerateFileData();
        }

        /// <summary>
        /// Creates a hidden file in a random location based on the user's environment.
        /// </summary>
        /// <returns>Path to the hidden file</returns>
        public string CreateImportantHiddenFileLocation()
        {
            return _model.CreateNewImportantFileLocation();
        }

        /// <summary>
        /// Destroys the hidden file created in the random location.
        /// </summary>
        public void DestroyImportantFileLocation()
        {
            _model.DestroyImportantFileLocation();
        }

        public void CreateImportantHiddenFileLocationFromSave()
        {
            _model.CreateImportantHiddenFileLocationFromSave();
        }
        
        /// <summary>
        /// Creates a location of the last important file for K-P. 
        /// </summary>
        /// <returns>Location of the last important file</returns>
        public string CreateLastFileLocation()
        {
            return _model.CreateLastFileLocation();
        }
        
        /// <summary>
        /// Creates the last important K-P's file.
        /// </summary>
        public void CreateLastImportantFile()
        {
            _model.CreateLastImportantFile();
        }
        
        /// <summary>
        /// Force opens file explorer and creates several files in a sequence. Every file has a word given in the text.
        /// Each file is created with a delay, so they don't appear all at once. The files are created in the specified directory.
        /// </summary>
        /// <param name="text">Names of the files</param>
        /// <param name="directoryPath">Path to the directory in which to create the files in sequence</param>
        /// <param name="delay">Delay between each file creation</param>
        public void CreateCreepyFileSequence(string text, string directoryPath, float delay)
        {
            _model.OpenFileExplorer(directoryPath);
            _model.CreateMultipleFilesWithDelay(directoryPath, text.Split(" "), delay);
        }

        /// <summary>
        /// Opens a file explorer with the specified path.
        /// </summary>
        /// <param name="path">Path to open the file explorer in.</param>
        public void OpenFileExplorer(string path)
        {
            _model.OpenFileExplorer(path);
        }
        
        /// <summary>
        /// Generates a random text of the specified length.
        /// </summary>
        /// <param name="length">Length of the text</param>
        /// <returns>Random char noise</returns>
        public string GenerateRandomText(int length)
        {
            return _model.GenerateRandomText(length);
        }

        /// <summary>
        /// Creates a zip file at the specified path containing the contents of the specified directory.
        /// </summary>
        /// <param name="zipFilePath">Path of the zip file</param>
        /// <param name="directoryPath">Path of the directory to be zipped</param>
        public void CreateZipFile(string zipFilePath, string directoryPath)
        {
            _model.CreateZipFile(zipFilePath, directoryPath);
        }

        /// <summary>
        /// Sets up a file deletion detection and executes an action when the file is deleted. 
        /// </summary>
        /// <param name="pathToFile">Path to the file</param>
        /// <param name="callback">What should happen after the file is deleted</param>
        public void SetupFileDeletion(string pathToFile, Action callback)
        {
            GameObject scriptHolder = Tools.GetScriptHolder();
            var detectionModel = scriptHolder.AddComponent<FileDeletionDetectionModel>();
            detectionModel.StartDetection(pathToFile, callback);
        }
    }
}