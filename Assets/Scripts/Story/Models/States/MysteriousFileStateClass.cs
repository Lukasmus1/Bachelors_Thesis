using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;

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
            
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("headOfDpt", 2);
        }

        public override void OnExit()
        {
            
        }
    }
}