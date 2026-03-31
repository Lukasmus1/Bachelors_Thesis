using System;
using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using Apps.FileManager.Commons;
using Desktop.Controllers;
using Desktop.Models;
using FourthWall.Commons;
using Sounds.Commons;
using Sounds.Models;
using Story.Commons;
using Story.Models;
using Story.Models.Actions;
using User.Commons;
using User.Models;

namespace Saving.Models
{
    [Serializable]
    public class SaveModel
    {
        public DesktopModel desktop;
        public StoryModel storyModel;
        public ChatTerminalModel chatTerminalModel;
        public List<string> loadedFiles;
        public List<string> hiddenFiles;
        public UserModel userModel;
        public ListOfActionsPersistent persistentActions;
        public SoundModel soundModel;
        public int ending;
        
        public void LoadDataFromModel(SaveModel saveModel)
        {
            desktop = saveModel.desktop;
            DesktopModel.Instance = desktop;
            DesktopGeneratorController.ClearFlags(); //Clear any flags that may have stuck
            
            chatTerminalModel = saveModel.chatTerminalModel;
            ChatTerminalMvc.Instance.ChatTerminalController.chatTerminalModel = chatTerminalModel;
            
            storyModel = saveModel.storyModel;
            StoryMvc.Instance.StoryController.storyModel = storyModel;
            StoryMvc.Instance.StoryController.LoadFromState();
            
            userModel = saveModel.userModel;
            UserMvc.Instance.UserController.userModel = userModel;
            if (!UserMvc.Instance.UserController.GetPersistentData(UserDataType.ImportantFileDeleted))
                FourthWallMvc.Instance.FileGenerationController.CreateImportantHiddenFileLocationFromSave();

            loadedFiles = saveModel.loadedFiles;
            hiddenFiles = saveModel.hiddenFiles;
            FileManagerMvc.Instance.FileManagerController.LoadFromSave(loadedFiles, hiddenFiles);
            
            persistentActions = saveModel.persistentActions;
            ActionsClass.Instance.ActionsPersistent = saveModel.persistentActions;
            
            soundModel = saveModel.soundModel;
            SoundMvc.Instance.SoundController.soundModel = saveModel.soundModel;
        }
    }
}
