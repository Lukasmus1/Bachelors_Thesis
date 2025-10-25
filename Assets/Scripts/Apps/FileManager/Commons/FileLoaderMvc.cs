using Apps.FileManager.Controllers;

namespace Apps.FileManager.Commons
{
    public class FileLoaderMvc
    {
        //Singleton instance
        private static FileLoaderMvc _instance;
        public static FileLoaderMvc Instance
        {
            get
            {
                _instance ??= new FileLoaderMvc();
                return _instance;
            }
        }

        public FileLoaderController FileLoaderController { get; set; }
        
        private FileLoaderMvc()
        {
            FileLoaderController = new FileLoaderController();
        }
    }
}