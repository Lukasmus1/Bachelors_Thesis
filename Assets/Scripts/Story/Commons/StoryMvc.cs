using Story.Controllers;

namespace Story.Commons
{
    public class StoryMvc
    {
        //Singleton instance
        private static StoryMvc _instance;
        public static StoryMvc Instance
        {
            get
            {
                _instance ??= new StoryMvc();
                return _instance;
            }
        }

        public StoryController StoryController { get; set; }
        
        private StoryMvc()
        {
            StoryController = new StoryController();
        }
    }
}