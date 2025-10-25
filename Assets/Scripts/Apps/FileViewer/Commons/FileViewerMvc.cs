using Apps.FileManager.Controllers;
using Apps.FileViewer.Controllers;
using UnityEngine;

namespace Apps.FileViewer.Commons
{
    public class FileViewerMvc
    {
        private static FileViewerMvc _instance;
        public static FileViewerMvc Instance
        {
            get
            {
                _instance ??= new FileViewerMvc();
                return _instance;
            }
        }

        public FileViewerController FileLoaderController { get; set; }
        
        private FileViewerMvc()
        {
            FileLoaderController = new FileViewerController();
        }
    }
}