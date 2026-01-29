using System;
using System.Text;
using FourthWall.FileGeneration.Models;

namespace FourthWall.FileGeneration.Controllers
{
    public class FileGenerationController
    {
        private readonly FileCreationModel _model = new();
        
        /// <summary>
        /// Creates a hidden file with the specified name and content.
        /// </summary>
        /// <param name="fileName">Path of the file</param>
        /// <param name="content">Content of the file</param>
        public void CreateHiddenFile(string fileName, string content)
        {
            _model.CreateHiddenFile(fileName, content);
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
    }
}