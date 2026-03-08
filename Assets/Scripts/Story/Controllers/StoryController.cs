using Story.Models;
using Story.Models.States;
using UnityEngine;

namespace Story.Controllers
{
    public class StoryController
    {
        public StoryModel storyModel = new();

        public StateClass CurrentStateClass
        {
            get => storyModel.CurrentStateClass;
            set => storyModel.CurrentStateClass = value;
        }

        public void InitNew()
        {
            storyModel.Init();
        }
        
        public void LoadFromState()
        {
            storyModel.LoadFromState();
        }

        /// <summary>
        /// Sets the ending of the story.
        /// </summary>
        /// <param name="ending"></param>
        public void SetEnding(Endings ending)
        {
            if (storyModel.Ending != Endings.None)
            {
                Debug.LogError("Ending has already been set. Cannot change ending.");
                return;
            }
            
            storyModel.Ending = ending;
        }
    }
}