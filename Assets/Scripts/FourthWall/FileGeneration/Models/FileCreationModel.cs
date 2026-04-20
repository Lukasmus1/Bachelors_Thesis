using System;
using System.Collections;
using System.IO;
using System.Text;
using Commons;
using FourthWall.FileGeneration.Controllers;
using UnityEngine;
using User.Commons;
using User.Models;
using Random = System.Random;
using Ionic.Zip;

namespace FourthWall.FileGeneration.Models
{
    public class FileCreationModel
    {
        private string CreateFile(string fileName, string content)
        {
            string dir =  Path.GetDirectoryName(fileName);
            string name = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            
            string fullPath = fileName;
            
            var i = 1;
            while (File.Exists(fullPath))
            {
                name = $"{name}({i})";
                
                fullPath = Path.Combine(dir!, name + ext);
                i++;
            }
            
            File.WriteAllText(fullPath, content);
            
            return fullPath;
        }

        /// <summary>
        /// Creates multiple files with the specified names and a delay between each creation. The content of each file is the same as its name.
        /// </summary>
        /// <param name="dir">Directory to save the files</param>
        /// <param name="text">Name and content of the file</param>
        /// <param name="delay">Delay between creations</param>
        public void CreateMultipleFilesWithDelay(string dir, string[] text, float delay)
        {
            MonoBehaviour mb = Tools.GetScriptReferenceLinker().GetMonoBehavior();
            
            mb.StartCoroutine(CreateMultipleFileWithDelayCoroutine(dir, text, delay));
        }
        
        private IEnumerator CreateMultipleFileWithDelayCoroutine(string dir, string[] fileName, float delay)
        {
            for (int i = 0; i < fileName.Length; i++)
            {
                CreateFile($"{dir}\\{10+i}_{fileName[i]}.txt", fileName[i]);
                yield return new WaitForSeconds(delay);
            }
        }
        
        /// <inheritdoc cref="FileGenerationController.CreateFile"/>
        public void CreateHiddenFile(string fileName, string content, bool hidden)
        {
            string path = CreateFile(fileName, content);
            
            if (hidden)
                File.SetAttributes(path, FileAttributes.Hidden);
        }
        
        /// <inheritdoc cref="FileGenerationController.DestroyFile"/>
        public void DestroyFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
        
        /// <inheritdoc cref="FileGenerationController.DestroyFolder"/>
        public void DestroyFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
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

        /// <inheritdoc cref="FileGenerationController.CreateImportantHiddenFileLocation"/>
        public string CreateNewImportantFileLocation()
        {
            string filePath = GenerateRandomImportantFileLocation();
            string content = GenerateFileData();
            
            CreateHiddenFile(filePath, content, true);
            
            return filePath;
        }

        /// <summary>
        /// Generates a random important file location.
        /// </summary>
        /// <returns>A random important file location.</returns>
        private string GenerateRandomImportantFileLocation()
        {
            string fullPath = GetLocalAppDataLowPath();
            string[] dirs = Directory.GetDirectories(fullPath);
            string randomPath = dirs[new Random().Next(dirs.Length)] + "_Backup";
            Directory.CreateDirectory(randomPath);
            
            //Create a hidden file in that directory with the generated content.
            string fileName = GenerateRandomFileName();
            return Path.Combine(randomPath, fileName);
        }

        /// <inheritdoc cref="FileGenerationController.CreateImportantHiddenFileLocationFromSave"/>
        public void CreateImportantHiddenFileLocationFromSave()
        {
            string filePath = UserMvc.Instance.UserController.ProceduralData(UserDataType.ImportantFileLocation);
            string content = GenerateFileData();
            
            CreateHiddenFile(filePath, content, true);
        }
        
        /// <inheritdoc cref="FileGenerationController.DestroyImportantFileLocation"/>
        public void DestroyImportantFileLocation()
        {
            string fullPath = UserMvc.Instance.UserController.ProceduralData(UserDataType.ImportantFileLocation);
            string directoryPath = Path.GetDirectoryName(fullPath);
            DestroyFolder(directoryPath);
        }

        /// <inheritdoc cref="FileGenerationController.CreateLastFileLocation"/>
        public string CreateLastFileLocation()
        {
            string localLowPath = GetLocalAppDataLowPath();
            const string dirName = "SelfChecker";
            string newDirPath = Path.Combine(localLowPath, dirName);

            string fileName = GenerateRandomFileName();
            string filePath = Path.Combine(newDirPath, fileName);

            return filePath;
        }

        /// <inheritdoc cref="FileGenerationController.CreateLastImportantFile"/>
        public void CreateLastImportantFile()
        {
            string path = UserMvc.Instance.UserController.ProceduralData(UserDataType.LastFileLocation);
            string content = GenerateFileData();
            
            CreateFile(path, content);
        }
        
        /// <summary>
        /// Returns the path to the LocalLow folder of the user's computer.
        /// </summary>
        /// <returns>The path to the LocalLow folder of the user's computer.</returns>
        private string GetLocalAppDataLowPath()
        {
            //Create a new directory that mimics a random directory in the LocalAppData\low folder.
            string localAppDataLow = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                "..", 
                "LocalLow"
            );
            
            return Path.GetFullPath(localAppDataLow); //Removes the .. and gives us the actual path to the LocalLow folder
        }

        /// <summary>
        /// Opens the Windows file explorer at the specified path.
        /// </summary>
        /// <param name="path"></param>
        public void OpenFileExplorer(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        /// <inheritdoc cref="FileGenerationController.GenerateRandomText"/>
        public string GenerateRandomText(int count)
        {
            Random rand = new();
            StringBuilder sb = new();

            for (var i = 0; i < count; i++)
            {
                var randChar = (char)rand.Next(0, 256);
                sb.Append(randChar);
            }
            
            return sb.ToString();
        }

        /// <inheritdoc cref="FileGenerationController.CreateZipFile"/>
        public void CreateZipFile(string zipPath, string directoryPath)
        {
            using var zip = new ZipFile();
            zip.AddDirectory(directoryPath);
            zip.Save(zipPath);
        }
        
        /// <inheritdoc cref="FileGenerationController.GeneratePatternHelper"/>
        public string GeneratePatternHelper()
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, @"ExternalApp\EncryptionPattern.exe");
            const string destPath = "EncryptionPattern.exe";
            
            File.Copy(sourcePath, destPath, true);

            return Path.GetFullPath(destPath);
        }
    }
}