using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.Autostereogram.Commons;
using Apps.CipherSolver.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;
using FourthWall.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User.Commons;
using User.Models;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Apps.FileManager.Models
{
    public class FileLoaderModel
    {
        private List<GameObject> _instantiatedFiles;
        private List<GameObject> InstantiatedFiles
        {
            get
            {
                if (_instantiatedFiles == null)
                {
                    LoadAllFiles();
                }
                
                return _instantiatedFiles;
            }
            
            set => _instantiatedFiles = value;
        }
        
        public List<string> LoadedFileNames { get; set; } = new();
        
        public List<string> InstantiatedFileNames { get; set; } = new();
        
        public List<string> HiddenFileNames { get; set; } = new();
        
        /// <summary>
        /// Loads all files from the Resources folder, creates clones of them, and sets their loaded state based on the context. Also procedurally generates content for specific files that require it.
        /// </summary>
        /// <exception cref="Exception">Gets thrown if the path to the files changes</exception>
        public void LoadAllFiles()
        {
            //Load all files and create clones of them
            InstantiatedFiles = Resources.LoadAll<GameObject>("Prefabs/Apps/FileManager/LoadedFiles").ToList().Select(Object.Instantiate).ToList();
            InstantiatedFiles.ForEach(x => x.name = x.name.Replace("(Clone)", ""));
            
            //Set all loaded files to not loaded at the start
            InstantiatedFiles.ForEach(x => x.GetComponent<FileModel>().IsLoaded = false);

            //Set loaded files based on context
            foreach (string fileName in LoadedFileNames)
            {
                InstantiatedFiles.Find(x => x.name == fileName).GetComponent<FileModel>().IsLoaded = true;
            }
            
            ProcedurallyGenerateFileContent();
         
            //Sets the hidden state of files based on loaded context
            foreach (string fileName in HiddenFileNames)
            {
                InstantiatedFiles.Find(x => x.name == fileName).GetComponent<FileModel>().IsHidden = true;
            }
            
            if (InstantiatedFiles.Count == 0)
            {
                throw new Exception("Prefabs/Apps/FileLoader/LoadedFiles Possible rename?");
            }
        }

        /// <summary>
        /// Loads files from the given context list of loaded file names.
        /// </summary>
        /// <param name="loadedFiles">List of file names to load</param>
        /// <param name="hiddenFiles">List of currently hidden files</param>
        public void LoadFromSave(List<string> loadedFiles, List<string> hiddenFiles)
        {
            LoadedFileNames = loadedFiles;
            HiddenFileNames = hiddenFiles;
        }
        
        /// <summary>
        /// Procedurally generates the content of files that require it.
        /// </summary>
        /// <exception cref="Exception">Gets thrown when specific file is not found</exception>
        private void ProcedurallyGenerateFileContent()
        {
            //Autostereogram file content
            GameObject autostereoFile = InstantiatedFiles.FirstOrDefault(x => x.name == "CypherCode");
            if (autostereoFile != null)
            {
                autostereoFile.GetComponentInChildren<Image>().sprite = 
                    AutostereogramMvc.Instance.AutostereogramController.GenerateAutostereogram(UserMvc.Instance.UserController.ProceduralData(UserDataType.VignereCode));
            }
            else
            {
                throw new Exception("Trying to access CypherCode file, but it does not exist. Possible rename?");
            }
            
            //Vigenere cipher file content
            GameObject vigenereFile = InstantiatedFiles.FirstOrDefault(x => x.name == "MysteriousFile");
            if (vigenereFile != null)
            {
                var text = vigenereFile.GetComponentInChildren<TMP_Text>();
                text.text = CipherMvc.Instance.CipherController.EncryptText(text.text, UserMvc.Instance.UserController.ProceduralData(UserDataType.VignereCode));
            }
            else
            {
                throw new Exception("Trying to access MysteriousFile file, but it does not exist. Possible rename?");
            }
            
            //Number sequence file content
            GameObject numberPattern = InstantiatedFiles.FirstOrDefault(x => x.name == "NumberPattern");
            if (numberPattern != null)
            {
                //This is based on the specific pattern used in the NumberPattern file.
                //const int numRows = 6;
                const int numCols = 38;
                const int maxNumCount = 228;
                
                //The pattern is an image with rigid holes. 
                //To make sure the numbers line up with the holes we have to generate the code in the same spot every time.
                //These numbers represent the indexes of the positions relative to the start of the pattern (0)
                int[] relativeNumSpots = { 0, 11, 54, 82, 133 };

                //I want the number code pattern to not always be at the same place, so I offset it by their maximum tested amounts.
                const int maxXOffset = 19;
                const int maxYOffset = 3;
                int randomXOffset = Random.Range(1, maxXOffset); //I don't want it to start at the top most left, so I start at 1 for the x offset
                int randomYOffset = Random.Range(0, maxYOffset);
                
                string numberPatternCode = UserMvc.Instance.UserController.ProceduralData(UserDataType.NumberPatternCode);
                
                StringBuilder numberNoise = new();
                numberNoise.Append(FourthWallMvc.Instance.NumberPatternController.CreateRandomNumberNoise(maxNumCount, false));
                
                for (int i = 0; i < relativeNumSpots.Length; i++)
                {
                    int numIndex = relativeNumSpots[i] + randomXOffset + (randomYOffset * numCols);
                    numberNoise[numIndex] = numberPatternCode[i];
                }
                
                var numberText = numberPattern.GetComponentInChildren<TMP_Text>();
                numberText.text = numberNoise.ToString();
            }
            else
            {
                throw new Exception("Trying to access NumberPattern file, but it does not exist. Possible rename?");
            }
            
            //Important Hidden file content
            GameObject importantHiddenFile = InstantiatedFiles.FirstOrDefault(x => x.name == "FileLocation");
            if (importantHiddenFile != null)
            {
                var text = importantHiddenFile.GetComponentInChildren<TMP_Text>();
                
                string fileLocation = UserMvc.Instance.UserController.ProceduralData(UserDataType.ImportantFileLocation);
                string encryptedLocation = CipherMvc.Instance.CipherController.IncompleteKeyEncrypt(fileLocation, UserMvc.Instance.UserController.ProceduralData(UserDataType.NumberPatternCode));
                
                text.text = encryptedLocation;
            }
            else
            {
                throw new Exception("Trying to access FileLocation file, but it does not exist. Possible rename?");
            }
        }
        
        /// <summary>
        /// Returns all loaded files.
        /// </summary>
        /// <returns>List of GameObjects of all loaded files</returns>
        public List<GameObject> GetLoadedFiles()
        {
            return InstantiatedFiles;
        }

        /// <summary>
        /// Sets the IsLoaded flag of a file with the given name.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="isLoaded">True for loaded, false for not</param>
        /// <exception cref="Exception">The file name could not be found</exception>
        public void SetLoadedFileFlag(string fileName, bool isLoaded)
        {
            GameObject loadedFile = InstantiatedFiles.FirstOrDefault(x => x.name == fileName);
            if (loadedFile == null)
            {
                throw new Exception($"File with name {fileName} not found in loaded files.");
            }
            
            //Save to context for saving and loading purposes
            if (isLoaded && !LoadedFileNames.Contains(fileName))
            {
                LoadedFileNames.Add(fileName);
            }
            else if (!isLoaded && LoadedFileNames.Contains(fileName))
            {
                LoadedFileNames.Remove(fileName);
            }
            
            var model = loadedFile.GetComponent<FileModel>();
            model.IsLoaded = isLoaded;

            if (isLoaded)
            {
                NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewFile, model.FileName);
            }
        }

        /// <summary>
        /// Sets the IsHidden flag of a file with the given name.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="shouldHide">Should it be hidden</param>
        /// <exception cref="Exception">Gets thrown if the file doesnt exist</exception>
        public void SetHiddenFileFlag(string fileName, bool shouldHide)
        {
            if (shouldHide)
            {
                if (!HiddenFileNames.Contains(fileName))
                {
                    HiddenFileNames.Add(fileName);
                }
            }
            else
            {
                if (HiddenFileNames.Contains(fileName))
                {
                    HiddenFileNames.Remove(fileName);
                }
            }
        }

        /// <summary>
        /// Sets the screenshot of the UserScreenshot sprite to the given file.
        /// </summary>
        /// <param name="screenshotSprite">Sprite of the image to set</param>
        /// <exception cref="Exception">Gets thrown if the screenshot file couldn't be found</exception>
        public void SetUserScreenshotFileImage(Sprite screenshotSprite)
        {
            GameObject screenshotFile = InstantiatedFiles.FirstOrDefault(x => x.name == "UserScreenshot");
            if (screenshotFile == null)
            {
                throw new Exception("Trying to access UserScreenshot file, but it does not exist. Possible rename?");
            }
            
            screenshotFile.GetComponentInChildren<Image>().sprite = screenshotSprite;
        }
    }
}