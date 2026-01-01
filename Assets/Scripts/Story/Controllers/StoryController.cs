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
    }
}