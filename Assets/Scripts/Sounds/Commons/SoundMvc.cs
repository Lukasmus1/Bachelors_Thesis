using Sounds.Controllers;

namespace Sounds.Commons
{
    public class SoundMvc
    {
        //Singleton instance
        private static SoundMvc _instance;
        public static SoundMvc Instance
        {
            get
            {
                _instance ??= new SoundMvc();
                return _instance;
            }
        }

        public SoundController SoundController { get; set; }
        
        private SoundMvc()
        {
            SoundController = new SoundController();
        }
    }
}