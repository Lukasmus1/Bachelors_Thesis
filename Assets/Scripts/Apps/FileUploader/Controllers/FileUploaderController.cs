using System;
using Apps.FileUploader.Models;
using Apps.FileUploader.Views;

namespace Apps.FileUploader.Controllers
{
    public class FileUploaderController
    {
        private readonly FileUploaderModel _fileUploaderModel = new();
        private FileUploaderView _fileUploaderView;

        public Action<string> OnSuccessfulUpload
        {
            get => _fileUploaderModel.onSuccessfulUpload;
            set => _fileUploaderModel.onSuccessfulUpload = value;
        }
        
        public void SetView(FileUploaderView view)
        {
            _fileUploaderView = view;
        }
        
        /// <summary>
        /// Handles the file upload process by delegating to the model. It checks if the uploaded file's content matches the expected content and returns a corresponding result.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        public string HandleFileUpload(string filePath)
        {
            return _fileUploaderModel.HandleFileUpload(filePath);
        }
    }
}