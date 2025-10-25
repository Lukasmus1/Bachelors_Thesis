using System.Collections.Generic;
using Apps.FileManager.Models;
using UnityEngine;

namespace Apps.FileManager.Controllers
{
    public class FileLoaderController
    {
        private readonly FileLoaderModel _fileLoaderModel = new();

        public List<GameObject> GetLoadedFile()
        {
            return _fileLoaderModel.GetLoadedFiles();
        }
    }
}