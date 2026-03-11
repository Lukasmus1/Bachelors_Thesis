using System;
using System.Collections.Generic;
using System.IO;
using FourthWall.Commons;
using UnityEngine;

namespace Apps.FileUploader.Models
{
    public class FileUploaderModel
    {
        public Action<string> onSuccessfulUpload;
        
        //Key, (Content, Result)
        private readonly Dictionary<string, (string, string)> _uploadedFiles = new()
        {
            {"confirmationFile.con",("VmFsaWRVcGxvYWRDb25maXJtYXRpb24=", FourthWallMvc.Instance.ExternalWebController.CreateEndingUrl())},
        };
        
        /// <summary>
        /// Handles the file upload.
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>Result if the file name and content match the predetermined values.</returns>
        /// <exception cref="FileNotFoundException">Gets thrown if the file does not exist</exception>
        public string HandleFileUpload(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }
            
            string fileName = Path.GetFileName(filePath);
            string content;
            try
            {
                content = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                return null;
            }

            if (_uploadedFiles.ContainsKey(fileName) && _uploadedFiles[fileName].Item1 == content)
            {
                onSuccessfulUpload?.Invoke(fileName);
                return _uploadedFiles[fileName].Item2;
            }

            return null;
        }
    }
}