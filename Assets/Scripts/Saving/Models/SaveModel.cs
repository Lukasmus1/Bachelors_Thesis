using System;
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
        
        public void LoadDataFromModel(SaveModel saveModel)
        {
            desktop = saveModel.desktop;
            DesktopModel.Instance = desktop;
            
            storyModel = saveModel.storyModel;
            StoryMvc.Instance.StoryController.storyModel = storyModel;
        }
    }
}
