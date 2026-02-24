using System;
using System.Collections.Generic;
using Apps.CipherSolver.Commons;
using Apps.FileManager.Models;
using Apps.FileManager.Views;
using FourthWall.Commons;
using UnityEngine;
using User.Commons;
using User.Models;

namespace Apps.FileManager.Controllers
{
    public class FileLoaderController
    {
        private readonly FileLoaderModel _fileLoaderModel = new();
        
        public Action onFilesUpdated;
        
        private FileLoaderView _fileLoaderView;
        public void SetFileLoaderView(FileLoaderView fileLoaderView)
        {
            _fileLoaderView = fileLoaderView;
        }

        /// <summary>
        /// Loads all files.
        /// </summary>
        public void LoadAllFiles()
        {
            _fileLoaderModel.LoadAllFiles();
        }
        
        /// <summary>
        /// Returns a list of all loaded files in the file loader.
        /// </summary>
        /// <returns>List of all loaded files</returns>
        public List<GameObject> GetLoadedFiles()
        {
            return _fileLoaderModel.GetLoadedFiles();
        }
        
        /// <summary>
        /// Pointer to the list of instantiated file names in the file loader model.
        /// </summary>
        public List<string> InstantiatedFileNames
        {
            get => _fileLoaderModel.InstantiatedFileNames;
            set => _fileLoaderModel.InstantiatedFileNames = value;
        }
        
        /// <summary>
        /// Pointer to the list of loaded file names in the file loader model.
        /// </summary>
        public List<string> LoadedFileNames
        {
            get => _fileLoaderModel.LoadedFileNames;
            set => _fileLoaderModel.LoadedFileNames = value;
        }
        
        public List<string> HiddenFileNames
        {
            get => _fileLoaderModel.HiddenFileNames;
            set => _fileLoaderModel.HiddenFileNames = value;
        }
        
        /// <summary>
        /// Loads files from the given context list of loaded file names and updates the file loader view.
        /// </summary>
        /// <param name="loadedFiles">Files to load</param>
        public void LoadFromSave(List<string> loadedFiles, List<string> hiddenFiles)
        {
            _fileLoaderModel.LoadFromSave(loadedFiles, hiddenFiles);
            onFilesUpdated?.Invoke();
        }
        
        /// <summary>
        /// Sets the loaded flag of the file with the given name and updates the file loader view.
        /// </summary>
        /// <param name="fileName">Name of the file to update</param>
        /// <param name="isLoaded">Should be loaded flag</param>
        public void SetLoadedFileFlag(string fileName, bool isLoaded)
        {
            _fileLoaderModel.SetLoadedFileFlag(fileName, isLoaded);
            onFilesUpdated?.Invoke();
        }

        /// <summary>
        /// Toggles the hidden flag of the file with the given name and updates the file loader view.
        /// </summary>
        /// <param name="fileName">Name of the file to toggle</param>
        /// <param name="shouldHide">Should the file be hidden?</param>
        public void ToggleFileVisibility(string fileName, bool shouldHide)
        {
            _fileLoaderView.ToggleFileVisibility(fileName, shouldHide);
            _fileLoaderModel.SetHiddenFileFlag(fileName, shouldHide); // Sets the flag in model -> used for saving
        }
        
        public void CreateUsersScreenshotFile()
        {
            //Get the mono behavior from the script reference linker
            MonoBehaviour monoBehavior = GameObject.FindGameObjectWithTag("ScriptHolder")
                .GetComponent<ScriptReferenceLinker>()
                .GetMonoBehavior();

            //Get the screenshot texture from the user information controller and check if it's null
            Texture2D screenshotTexture;
            FourthWallMvc.Instance.UserInformationController.Screenshot(monoBehavior, async (tex) =>
            {
                screenshotTexture = tex;
                
                if (!screenshotTexture)
                {
                    throw new Exception("The screenshot texture is null!");
                }
                
                //Get the code for encrypting the image from the user data controller and encrypt the image with the cipher controller
                string code = UserMvc.Instance.UserController.ProceduralData(UserDataType.PictureCode);
                Debug.Log(code);
                Sprite encryptedImageSprite = await CipherMvc.Instance.CipherController.EncryptDecryptImage(screenshotTexture, code);
            
                //Finally sets the encrypted image as the user's screenshot file image in the file loader and user models
                UserMvc.Instance.UserController.userModel.EncryptedGameScreenshot =
                    encryptedImageSprite.texture.GetRawTextureData();
            });
        }
    }
}