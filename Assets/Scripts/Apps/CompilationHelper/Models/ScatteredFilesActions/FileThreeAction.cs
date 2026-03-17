using Commons;
using FourthWall.Commons;
using FourthWall.UserInformation.Models;
using UnityEngine;

namespace Apps.CompilationHelper.Models.ScatteredFilesActions
{
    public class FileThreeAction : FileAction
    {
        public FileThreeAction(string fileLocation) : base(fileLocation)
        {
        }

        public override void OnLoad()
        {
            GameObject scriptHolder = Tools.GetScriptHolder();
            var registryDetection = scriptHolder.AddComponent<RegistryValueDetection>();
            registryDetection.CreateRegistry("Kernel_Panic", "AutonomyFileHidden", 1);
            registryDetection.StartDetection(0, CreateFile);
        }
    }
}