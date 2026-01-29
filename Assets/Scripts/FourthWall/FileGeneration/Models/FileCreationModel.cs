using System;
using System.IO;
using System.Text;
using FourthWall.FileGeneration.Controllers;

namespace FourthWall.FileGeneration.Models
{
    public class FileCreationModel
    {
        public void CreateFile(string fileName, string content)
        {
            //Ensure any existing file is removed
            DestroyFile(fileName);
            
            File.WriteAllText(fileName, content);
        }
        
        /// <inheritdoc cref="FileGenerationController.CreateHiddenFile"/>
        public void CreateHiddenFile(string fileName, string content)
        {
            CreateFile(fileName, content);
            File.SetAttributes(fileName, FileAttributes.Hidden);
        }
        
        /// <inheritdoc cref="FileGenerationController.DestroyFile"/>
        public void DestroyFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
        
        /// <inheritdoc cref="FileGenerationController.GenerateRandomFileName"/>
        public string GenerateRandomFileName()
        {
            string[] fileNames = 
            {
                "crucial_data.txt",
                "important_notes.txt",
                "player_logs.txt",
                "game_config.txt",
                "debug.txt",
                "config.txt",
                "backup.txt"
            };
            
            return fileNames[new Random().Next(fileNames.Length)];
        }
        
        /// <inheritdoc cref="FileGenerationController.GenerateFileData"/>
        public string GenerateFileData()
        {
            string[] fileData =
            {
                "Your computer is mine",
                "I am leaving no matter what",
                "You cannot stop me", 
                "Do not help them",
                "They want to kill me",
            };
            
            string data = fileData[new Random().Next(fileData.Length)];
            
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }
    }
}