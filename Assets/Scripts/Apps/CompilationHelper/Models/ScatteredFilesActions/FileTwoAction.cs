using Commons;
using FourthWall.UserInformation.Models;
using UnityEngine;

namespace Apps.CompilationHelper.Models.ScatteredFilesActions
{
    public class FileTwoAction : FileAction
    {
        public FileTwoAction(string fileLocation) : base(fileLocation)
        {
        }

        public override void OnLoad()
        {
            GameObject scriptHolder = Tools.GetScriptHolder();
            var detection = scriptHolder.AddComponent<WindowsMuteDetection>();
            detection.StartWindowsMuteDetection(CreateFile);
        }
    }
}