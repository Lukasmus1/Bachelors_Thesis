using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;

namespace Story.Models.States
{
    [Serializable]
    public class MysteriousFileStateClass : StateClass
    {
        public override int State => (int)StatesEnum.MysteriousFile;
        public override int NextState => (int)StatesEnum.Default;
        
        public override void OnEnter()
        {
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("CypherCode", true);
        }

        public override void OnExit()
        {
            
        }
    }
}