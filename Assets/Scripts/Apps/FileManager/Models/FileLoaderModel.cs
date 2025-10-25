using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Apps.FileManager.Models
{
    public class FileLoaderModel
    {
        private readonly List<GameObject> loadedFiles;
        
        public FileLoaderModel()
        {
            //There must always be at least the tutorial file in the loaded files folder
            loadedFiles = Resources.LoadAll<GameObject>("Prefabs/Apps/FileLoader/LoadedFiles").ToList();
            if (loadedFiles.Count == 0)
            {
                throw new Exception("Prefabs/Apps/FileLoader/LoadedFiles Possible rename?");
            }
        }
        public List<GameObject> GetLoadedFiles()
        {
            return loadedFiles;
        }
    }
}