using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Autostereogram.Commons;
using Apps.VigenereCipher.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User.Commons;
using User.Models;

namespace Apps.FileManager.Models
{
    public class FileLoaderModel
    {
        private readonly List<GameObject> _loadedFiles;
        
        public List<string> InstantiatedFileNames { get; set; } = new();
        
        public FileLoaderModel()
        {
            //There must always be at least the tutorial file in the loaded files folder
            _loadedFiles = Resources.LoadAll<GameObject>("Prefabs/Apps/FileLoader/LoadedFiles").ToList();
            
            //Set all loaded files to not loaded at the start
            _loadedFiles.ForEach(x => x.GetComponent<FileModel>().IsLoaded = false);

            ProceduralyGenerateFileContent();
            
            if (_loadedFiles.Count == 0)
            {
                throw new Exception("Prefabs/Apps/FileLoader/LoadedFiles Possible rename?");
            }
        }
        
        /// <summary>
        /// Proceduraly generates the content of files that require it.
        /// </summary>
        /// <exception cref="Exception">Gets thrown when specific file is not found</exception>
        private void ProceduralyGenerateFileContent()
        {
            //Autostereogram file content
            GameObject autostereoFile = _loadedFiles.FirstOrDefault(x => x.name == "CypherCode");
            if (autostereoFile != null)
            {
                autostereoFile.GetComponentInChildren<Image>().sprite = 
                    AutostereogramMvc.Instance.AutostereogramController.GenerateAutostereogram(UserMvc.Instance.UserController.ProceduralData(ProceduralDataType.VignereCode));
            }
            else
            {
                throw new Exception("Trying to access CypherCode file, but it does not exist. Possible rename?");
            }
            
            //Vigenere cipher file content
            GameObject vigenereFile = _loadedFiles.FirstOrDefault(x => x.name == "MysteriousFile");
            if (vigenereFile != null)
            {
                var text = vigenereFile.GetComponentInChildren<TMP_Text>();
                text.text = VigenereMvc.Instance.VigenereController.EncryptText(text.text, UserMvc.Instance.UserController.ProceduralData(ProceduralDataType.VignereCode));
            }
            else
            {
                throw new Exception("Trying to access MysteriousFile file, but it does not exist. Possible rename?");
            }
        }
        
        /// <summary>
        /// Returns all loaded files.
        /// </summary>
        /// <returns>List of GameObjects of all loaded files</returns>
        public List<GameObject> GetLoadedFiles()
        {
            return _loadedFiles;
        }

        /// <summary>
        /// Sets the IsLoaded flag of a file with the given name.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="isLoaded">True for loaded, false for not</param>
        /// <exception cref="Exception">The file name could not be found</exception>
        public void SetLoadedFileFlag(string fileName, bool isLoaded)
        {
            GameObject loadedFile = _loadedFiles.FirstOrDefault(x => x.name == fileName);
            if (loadedFile == null)
            {
                throw new Exception($"File with name {fileName} not found in loaded files.");
            }
            
            loadedFile.GetComponent<FileModel>().IsLoaded = isLoaded;
        }
    }
}