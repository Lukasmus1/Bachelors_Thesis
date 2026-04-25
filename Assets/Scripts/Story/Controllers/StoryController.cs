using Saving.Commons;
using Story.Models;
using Story.Models.States;

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
        /// Sets the ending of the story. And creates a hidden old save file.
        /// </summary>
        /// <param name="ending">Ending to set</param>
        public void SetEnding(Endings ending)
        {
            storyModel.SetEnding(ending);

            SavingMvc.Instance.SavingController.CreateOldSaveFile();
        }

        /// <summary>
        /// Gets the maximum or minimum value of the sum of all alignments, depending on the boolean parameter.
        /// </summary>
        /// <param name="maximumAlignment">Do I want the maximum possible alignment?</param>
        /// <returns>Max or min possible alignment</returns>
        public int GetExtremeAlignment(bool maximumAlignment) => storyModel.GetExtremeAlignment(maximumAlignment);
    }
}