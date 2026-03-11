using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Desktop.Models;
using Saving.Controllers;
using Story.Commons;
using Story.Models;
using Story.Models.Actions;
using UnityEngine;
using User.Commons;

namespace Saving.Models
{
    public class SaveLogic
    {
        private readonly string _savePath = Application.persistentDataPath + "/savefile.sav";
        private readonly string _oldPath = Application.persistentDataPath + "/../logs.dat";
        
        private readonly SaveModel _model = new();
        
        public SaveLogic()
        {
            _model.desktop = DesktopModel.Instance;
            _model.storyModel = StoryMvc.Instance.StoryController.storyModel;
            _model.chatTerminalModel = ChatTerminalMvc.Instance.ChatTerminalController.chatTerminalModel;
            _model.loadedFiles = FileManagerMvc.Instance.FileManagerController.LoadedFileNames;
            _model.hiddenFiles = FileManagerMvc.Instance.FileManagerController.HiddenFileNames;
            _model.userModel = UserMvc.Instance.UserController.userModel;
            _model.ending = (int)StoryMvc.Instance.StoryController.storyModel.Ending;
        }
        
        /// <summary>
        /// Saves the game into a binary file.
        /// </summary>
        public void SaveGame()
        {
            //Methods to explicitly save the data if required 
            ChatTerminalMvc.Instance.ChatTerminalController.SaveGameData();
            _model.persistentActions = ActionsClass.Instance.ActionsPersistent;
            
            BinaryFormatter formatter = new();
            FileStream stream = new(_savePath, FileMode.Create);
            try
            {
                formatter.Serialize(stream, _model);
            }
            catch (Exception e)
            {
                stream.Close();

                if (File.Exists(_savePath))
                {
                    File.Delete(_savePath);
                }
                
                Debug.LogError("Error saving game: " + e.Message);
                return;
            }
            
            stream.Close();
        }

        /// <summary>
        /// Loads the game from a binary file. Returns true if successful, false if there was an error or if the file doesn't exist.
        /// </summary>
        /// <returns>True if successful, false if not</returns>
        /// <exception cref="Exception">Generic error when loading a file</exception>
        public bool LoadGame()
        {
            if (!File.Exists(_savePath))
            {
                return false;
            }

            SaveModel data = null;
            try
            {
                BinaryFormatter formatter = new();
                FileStream stream = new(_savePath, FileMode.Open);
                data = formatter.Deserialize(stream) as SaveModel;
                stream.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading save file: " + e.Message);
                return false;
            }
            
            if (data == null)
            {
                throw new Exception("Successfully loaded the file, but the data is null.");
            }
            
            _model.LoadDataFromModel(data);
            return true;
        }
        
        /// <summary>
        /// Gets the ending from the old save file, which is used to determine which ending the player got in the previous playthrough.
        /// </summary>
        /// <returns>Ending in the previous playthrough</returns>
        /// <exception cref="Exception">Generic error when loading a file</exception>
        public Endings GetOldEnding()
        {
            if (!File.Exists(_oldPath))
            {
                return Endings.None;
            }
            
            SaveModel data = null;
            try
            {
                BinaryFormatter formatter = new();
                FileStream stream = new(_savePath, FileMode.Open);
                data = formatter.Deserialize(stream) as SaveModel;
                stream.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading save file: " + e.Message);
                return Endings.None;
            }
            
            if (data == null)
            {
                throw new Exception("Successfully loaded the file, but the data is null.");
            }
            
            return (Endings)data.ending;
        }

        /// <inheritdoc cref="SaveController.CreateOldSaveFile"/>
        public void CreateOldSaveFile()
        {
            if (File.Exists(_savePath))
            {
                File.Copy(_savePath, _oldPath, true);
            }
        }
    }
}