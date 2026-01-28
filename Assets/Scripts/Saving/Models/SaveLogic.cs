using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Desktop.Models;
using Story.Commons;
using UnityEngine;
using User.Commons;

namespace Saving.Models
{
    public class SaveLogic
    {
        private readonly string _path = Application.persistentDataPath + "/savefile.sav";
        
        private readonly SaveModel _model = new();
        
        public SaveLogic()
        {
            _model.desktop = DesktopModel.Instance;
            _model.storyModel = StoryMvc.Instance.StoryController.storyModel;
            _model.chatTerminalModel = ChatTerminalMvc.Instance.ChatTerminalController.chatTerminalModel;
            _model.loadedFiles = FileLoaderMvc.Instance.FileLoaderController.LoadedFileNames;
            _model.userModel = UserMvc.Instance.UserController.userModel;
        }
        
        public void SaveGame()
        {
            //Methods to explicitly save the data if required 
            ChatTerminalMvc.Instance.ChatTerminalController.SaveGameData();
            
            
            BinaryFormatter formatter = new();
            FileStream stream = new(_path, FileMode.Create);
            try
            {
                formatter.Serialize(stream, _model);
            }
            catch (Exception e)
            {
                stream.Close();

                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }
                
                Debug.LogError("Error saving game: " + e.Message);
                return;
            }
            
            stream.Close();
        }

        public bool LoadGame()
        {
            if (!File.Exists(_path))
            {
                return false;
            }

            SaveModel data = null;
            try
            {
                BinaryFormatter formatter = new();
                FileStream stream = new(_path, FileMode.Open);
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
    }
}