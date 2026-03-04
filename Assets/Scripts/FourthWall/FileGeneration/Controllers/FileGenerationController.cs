using System;
using System.Text;
using Commons;
using FourthWall.FileGeneration.Models;

namespace FourthWall.FileGeneration.Controllers
{
    public class FileGenerationController
    {
        private readonly FileCreationModel _model = new();
        private readonly WindowsErrorHandling _winErrorHandling = new();

        /// <summary>
        /// Creates a file with the specified name and content. Optionally it could be a hidden file.
        /// </summary>
        /// <param name="fileName">Path of the file</param>
        /// <param name="content">Content of the file</param>
        /// <param name="hidden">Should the file be hidden?</param>
        public void CreateFile(string fileName, string content, bool hidden)
        {
            _model.CreateHiddenFile(fileName, content, hidden);
        }
        
        /// <summary>
        /// Destroys the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        public void DestroyFile(string fileName)
        {
            _model.DestroyFile(fileName);
        }

        /// <summary>
        /// Generates a random file name from a predefined list.
        /// </summary>
        /// <returns>Random file name</returns>
        public string GenerateRandomFileName()
        {
            return _model.GenerateRandomFileName();
        }

        /// <summary>
        /// Generates file data content.
        /// </summary>
        /// <returns>File content in base64</returns>
        public string GenerateFileData()
        {
            return _model.GenerateFileData();
        }

        /// <summary>
        /// Created a new windows dialog based on the specified type, message and title.
        /// </summary>
        /// <param name="dialogType">Type of the dialog</param>
        /// <param name="message">Message of the dialog window</param>
        /// <param name="title">Title of the window</param>
        /// <exception cref="ArgumentOutOfRangeException">Gets thrown if unknown dialogTypeis used</exception>
        public void ThrowWindowsDialog(DialogType dialogType, string message, string title)
        {
            switch (dialogType)
            {
                case DialogType.Error:
                    _winErrorHandling.ThrowError(message, title);
                    break;
                case DialogType.Info:
                    _winErrorHandling.ThrowInfo(message, title);
                    break;
                case DialogType.Warning:
                    _winErrorHandling.ThrowWarning(message, title);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null);
            }
        }

        /// <summary>
        /// Creates a hidden file in a random location based on the user's environment.
        /// </summary>
        /// <returns>Path to the hidden file</returns>
        public string CreateImportantHiddenFileLocation()
        {
            return _model.CreateImportantFileLocation();
        }

        /// <summary>
        /// Force opens file explorer and creates several files in a sequence. Every file has a word given in the text.
        /// Each file is created with a delay, so they don't appear all at once. The files are created in the specified directory.
        /// </summary>
        /// <param name="text">Names of the files</param>
        /// <param name="directoryPath">Path to the directory in which to create the files in sequence</param>
        /// <param name="delay">Delay between each file creation</param>
        public void CreateCreepyFileSequence(string text, string directoryPath, float delay)
        {
            _model.OpenFileExplorer(directoryPath);
            _model.CreateMultipleFilesWithDelay(directoryPath, text.Split(" "), delay);
        }
    }
}