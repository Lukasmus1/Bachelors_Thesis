using Story.Models;
using Story.Models.States;

namespace Story.Controllers
{
    public class StoryController
    {
        public StoryModel storyModel = new();

        public IState CurrentState
        {
            get => storyModel.CurrentState;
            set => storyModel.CurrentState = value;
        }

        public void InitNew()
        {
            storyModel.Init();
        }
    }
}