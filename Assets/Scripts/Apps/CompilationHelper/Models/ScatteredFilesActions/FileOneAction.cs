using FourthWall.Commons;

namespace Apps.CompilationHelper.Models.ScatteredFilesActions
{
    /// <summary>
    /// First file generated on the desktop.
    /// </summary>
    public class FileOneAction : FileAction
    {
        public FileOneAction(string fileLocation) : base(fileLocation)
        {
        }

        public override void OnLoad()
        {
            string fileContent = FourthWallMvc.Instance.FileGenerationController.GenerateFileData(); 
            FourthWallMvc.Instance.FileGenerationController.CreateFile(FileLocation, fileContent, false);
            
            StartFileDeletionDetection();
        }
    }
}