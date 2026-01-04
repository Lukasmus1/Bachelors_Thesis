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

        public void LoadAllFiles()
        {
            _fileLoaderModel.LoadAllFiles();
        }
        
        public List<GameObject> GetLoadedFiles()
        {
            return _fileLoaderModel.GetLoadedFiles();
        }
        
        public List<string> InstantiatedFileNames
        {
            get => _fileLoaderModel.InstantiatedFileNames;
            set => _fileLoaderModel.InstantiatedFileNames = value;
        }
        
        public List<string> LoadedFileNames
        {
            get => _fileLoaderModel.LoadedFileNames;
            set => _fileLoaderModel.LoadedFileNames = value;
        }
        
        public void LoadFilesFromContext(List<string> loadedFiles)
        {
            _fileLoaderModel.LoadFilesFromContext(loadedFiles);
            onFilesUpdated?.Invoke();
        }
        
        public void SetLoadedFileFlag(string fileName, bool isLoaded)
        {
            _fileLoaderModel.SetLoadedFileFlag(fileName, isLoaded);
            onFilesUpdated?.Invoke();
        }
    }
}