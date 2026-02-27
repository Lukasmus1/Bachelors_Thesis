using Apps.FileManager.Controllers;

namespace Apps.FileManager.Commons
{
    public class FileManagerMvc
    {
        //Singleton instance
        private static FileManagerMvc _instance;
        public static FileManagerMvc Instance
        {
            get
            {
                _instance ??= new FileManagerMvc();
                return _instance;
            }
        }

        public FileLoaderController FileManagerController { get; set; }
        public ContextMenuController ContextMenuController { get; set; }
        
        private FileManagerMvc()
        {
            FileManagerController = new FileLoaderController();
            ContextMenuController = new ContextMenuController();
        }
    }
}