using System;
using Apps.ChatTerminal.Commons;
using Apps.CompilationHelper.Commons;
using Desktop.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class EndHelpCuratorStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.EndingFightForCurator;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            //Fragmented AI compiling into a secret folder, if that process completes, the player will have to disconnect from the internet.
            //And the AI will turn on the game itself, corrupting parts of it overtime. If that completes -> jumpscare and the game shuts down and will be unable to load again due using a save file.
            //By exploring and adjusting the computer's settings they will slowly uncover the locations.
            //First file will be on the desktop
            //Second file will be revealed by muting the computer's audio
            //For last file, the player will have to edit a custom registry entry
            
            //debug
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("curator");
            
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator","curatorFightForCuratorSetup", true);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += LoadApp;
        }

        private void LoadApp(string messageID)
        {
            if (messageID != "curatorFightForCuratorSetupApp")
                return;
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= LoadApp;
            
            CompilationHelperMvc.Instance.CompilationHelperController.EnableForCuratorCompilationProcess(120);
        }
    }
}