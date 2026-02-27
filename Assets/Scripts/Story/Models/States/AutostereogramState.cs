using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class AutostereogramState : StateClass
    {
        public override int State => (int)StatesEnum.AutostereogramState;
        public override int NextState { get; set; } = (int)StatesEnum.MysteriousFile;
        
        public override void OnEnter()
        {
            FileManagerMvc.Instance.FileManagerController.SetLoadedFileFlag("CypherCode", true);
            
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("headOfDpt");

            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.openedContact -= TransitionCheck;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.openedContact += TransitionCheck;
        }

        public void TransitionCheck(string profileId)
        {
            if (profileId == "headOfDpt")
            {
                ChangeToNextState();
            }
        }
    }
}