using Apps.VirusFinder.Controllers;

namespace Apps.VirusFinder.Commons
{
    public class VirusFinderMvc
    {
        private static VirusFinderMvc _instance;
        public static VirusFinderMvc Instance
        {
            get
            {
                _instance ??= new VirusFinderMvc();
                return _instance;
            }
        }

        public VirusFinderController VirusFinderController { get; set; }
        
        private VirusFinderMvc()
        {
            VirusFinderController = new VirusFinderController();
        }
    }
}