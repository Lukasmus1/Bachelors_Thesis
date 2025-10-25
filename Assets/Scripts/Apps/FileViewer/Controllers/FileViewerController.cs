using Apps.FileViewer.Models;
using UnityEngine;

namespace Apps.FileViewer.Controllers
{
    public class FileViewerController
    {
        private readonly FileViewerModel _fileViewerModel = new(); 
        
        public GameObject OpenedFile
        {
            get => _fileViewerModel.OpenedFile;
            set => _fileViewerModel.OpenedFile = value;
        }
    }
}