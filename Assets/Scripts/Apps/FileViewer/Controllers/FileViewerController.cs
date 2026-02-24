using System;
using Apps.FileViewer.Models;
using UnityEngine;

namespace Apps.FileViewer.Controllers
{
    public class FileViewerController
    {
        private readonly FileViewerModel _fileViewerModel = new(); 
        
        public Action<string> metadataOpened;
        
        /// <summary>
        /// Returns the name of the file as depicted in game -> NOT GameObject NAME
        /// </summary>
        public Action<string> onFileOpened;
        
        public GameObject OpenedFile
        {
            get => _fileViewerModel.OpenedFile;
            set => _fileViewerModel.OpenedFile = value;
        }
    }
}