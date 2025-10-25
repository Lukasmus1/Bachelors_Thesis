using System.Collections.Generic;
using Apps.FileLoader.Models;
using UnityEngine;

namespace Apps.FileLoader.Controllers
{
    public class FileLoaderController
    {
        private FileLoaderModel _fileLoaderModel = new();

        public List<GameObject> GetLoadedFile()
        {
            return _fileLoaderModel.GetLoadedFiles();
        }
    }
}