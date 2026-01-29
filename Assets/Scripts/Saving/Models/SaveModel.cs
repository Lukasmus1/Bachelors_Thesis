using System;
using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using Apps.FileManager.Commons;
using Desktop.Controllers;
using Desktop.Models;
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
        public UserModel userModel;
        public ListOfActionsPersistent persistentActions;
        
        
        public void LoadDataFromModel(SaveModel saveModel)
        {
            desktop = saveModel.desktop;
            DesktopModel.Instance = desktop;
            DesktopGeneratorController.ClearFlags(); //Clear any flags that may have stuck
            
            storyModel = saveModel.storyModel;
            StoryMvc.Instance.StoryController.storyModel = storyModel;
            StoryMvc.Instance.StoryController.LoadFromState();
            
            chatTerminalModel = saveModel.chatTerminalModel;
            ChatTerminalMvc.Instance.ChatTerminalController.chatTerminalModel = chatTerminalModel;
            
            loadedFiles = saveModel.loadedFiles;
            FileLoaderMvc.Instance.FileLoaderController.LoadFilesFromContext(loadedFiles);
            
            userModel = saveModel.userModel;
            UserMvc.Instance.UserController.userModel = userModel;

            ActionsClass.Instance.ActionsPersistent = saveModel.persistentActions;
        }
    }
}
