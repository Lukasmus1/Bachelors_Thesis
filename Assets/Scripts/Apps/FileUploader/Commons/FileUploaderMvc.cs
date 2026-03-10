using Apps.FileUploader.Controllers;

namespace Apps.FileUploader.Commons
{
    public class FileUploaderMvc
    {
        private static FileUploaderMvc _instance;
        public static FileUploaderMvc Instance
        {
            get
            {
                _instance ??= new FileUploaderMvc();
                return _instance;
            }
        }

        public FileUploaderController FileUploaderController { get; set; }
        
        private FileUploaderMvc()
        {
            FileUploaderController = new FileUploaderController();
        }
    }
}