using Apps.FileLoader.Controllers;

namespace Apps.FileLoader.Commons
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