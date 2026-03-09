using System;
using Apps.ChatTerminal.Commons;
using Apps.CompilationHelper.Commons;
using Desktop.Commons;
using FourthWall.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class EndHelpAIStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.EndingFightForAI;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        private int simulationSeconds = 120;
        
        public override void OnEnter()
        {
            //The AI will need time to compile into a shippable zipfile that the player will then upload into the net.
            //Every 30 seconds for 2 minutes, the player will have to move the AI to different folders to hide it from the curator.
            //First third of seconds will be just the first moving.
            //Second third of seconds will be the curator pinging the folder by creating files that the player has to delete.  
            //Last third of seconds will be all of the above plus minimizing all windows.
            //After all of that, the player will have to delete the curator process in the task manager and upload KP to the net, which will result in the of the game.
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpHelpExplanation", true);   
            
            LoadFromState();
        }

        public override void OnExit()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += BeginCompilation;
        }
        
        public void BeginCompilation(string messageID)
        {
            if (messageID != "kpHelpExplanationEnd")
            {
                return;
            }
            
            CompilationHelperMvc.Instance.CompilationHelperController.EnableCompilationProcess(simulationSeconds);
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds += FirstMovePrompt;
        }

        public void FirstMovePrompt(int seconds)
        {
            if (seconds < simulationSeconds * 0.1) // first prompt at 1/10th of the total time
            {
                return;
            }
            
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds -= FirstMovePrompt;
            
            FourthWallMvc.Instance.CompilationSimulationController.FirstMovePrompt();
        }
    }
}