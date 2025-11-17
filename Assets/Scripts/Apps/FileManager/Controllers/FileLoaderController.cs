using System;
using System.Collections.Generic;
using Apps.FileManager.Models;
using Apps.FileManager.Views;
using UnityEngine;

namespace Apps.FileManager.Controllers
{
    public class FileLoaderController
    {
        private readonly FileLoaderModel _fileLoaderModel = new();
        
        public Action onFilesUpdated;
        
        private FileLoaderView _fileLoaderView;
        public void SetFileLoaderView(FileLoaderView fileLoaderView)
        {
            _fileLoaderView = fileLoaderView;
        }
        
        public List<GameObject> GetLoadedFile()
        {
            return _fileLoaderModel.GetLoadedFiles();
        }
        
        public List<string> InstantiatedFileNames
        {
            get => _fileLoaderModel.InstantiatedFileNames;
            set => _fileLoaderModel.InstantiatedFileNames = value;
        }
        
        public void SetLoadedFileFlag(string fileName, bool isLoaded)
        {
            _fileLoaderModel.SetLoadedFileFlag(fileName, isLoaded);
            onFilesUpdated?.Invoke();
        }
    }
}