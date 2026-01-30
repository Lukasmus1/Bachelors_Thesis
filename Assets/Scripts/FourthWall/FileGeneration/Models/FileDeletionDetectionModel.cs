using System;
using System.IO;
using UnityEngine;

namespace FourthWall.FileGeneration.Models
{
    /// <summary>
    /// Class that detects the deletion of a specified file.
    /// </summary>
    public class FileDeletionDetectionModel : MonoBehaviour
    {
        private Action fileDeleted;

        private string _monitoredFilePath;
        private bool _start;
        
        /// <summary>
        /// Starts the detection of file deletion for the specified file path.
        /// </summary>
        /// <param name="pathToFile">Path of the file to detect the deletion of</param>
        /// <param name="onComplete">Action that triggers when the file is deleted</param>
        /// <exception cref="FileNotFoundException">Gets thrown when the file does not exist prior to detecting its deletion</exception>
        public void StartDetection(string pathToFile, Action onComplete)
        {
            fileDeleted += onComplete;
            
            _monitoredFilePath = pathToFile;
            _start = true;

            if (!File.Exists(_monitoredFilePath))
            {
                throw new FileNotFoundException($"{_monitoredFilePath} not found. Cannot start deletion detection.");
            }
        }

        public bool IsFileDeleted()
        {
            return !File.Exists(_monitoredFilePath);
        }
        
        private void Update()
        {
            if (!_start || !IsFileDeleted()) return;
            
            fileDeleted?.Invoke();
            _start = false;
        }
    }
}