using System;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using Desktop.Commons;
using Desktop.Controllers;
using Desktop.Models;
using Story.Commons;
using Story.Models;
using UnityEngine;

namespace Saving.Models
{
    [Serializable]
    public class SaveModel
    {
        public DesktopModel desktop;
        public StoryModel storyModel;
        public ChatTerminalModel chatTerminalModel;
        
        public void LoadDataFromModel(SaveModel saveModel)
        {
            desktop = saveModel.desktop;
            DesktopModel.Instance = desktop;
            DesktopGeneratorController.ClearFlags(); //Clear any flags that may have stuck
            
            storyModel = saveModel.storyModel;
            StoryMvc.Instance.StoryController.storyModel = storyModel;
            
            chatTerminalModel = saveModel.chatTerminalModel;
            ChatTerminalMvc.Instance.ChatTerminalController.chatTerminalModel = chatTerminalModel;
        }
    }
}
