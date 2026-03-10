using Apps.FileUploader.Models;
using Apps.FileUploader.Views;

namespace Apps.FileUploader.Controllers
{
    public class FileUploaderController
    {
        private readonly FileUploaderModel _fileUploaderModel = new();
        private FileUploaderView _fileUploaderView = new();

        public void SetView(FileUploaderView view)
        {
            _fileUploaderView = view;
        }
    }
}