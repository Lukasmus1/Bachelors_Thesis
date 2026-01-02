using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class MysteriousFileState : StateClass
    {
        public override int State => (int)StatesEnum.MysteriousFile;
        public override int NextState => (int)StatesEnum.Default;
        
        public override void OnEnter()
        {
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("MysteriousFile", true);
        }

        public override void OnExit()
        {
            
        }
    }
}