using System;
using Apps.ChatTerminal.Commons;
using Apps.CompilationHelper.Commons;
using FourthWall.Commons;
using UnityEngine;

namespace Story.Models.States
{
    [Serializable]
    public class EndHelpAIStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.EndingFightForAI;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        private const int SIMULATION_SECONDS = 120;
        private const float THIRD_OF_SIMULATION = SIMULATION_SECONDS / 3f;
        private const float TWO_THIRDS_OF_SIMULATION = SIMULATION_SECONDS * (2f / 3f);

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

            // Start compilation simulation and subscribe to progress updates
            CompilationHelperMvc.Instance.CompilationHelperController.EnableCompilationProcess(SIMULATION_SECONDS);
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds +=
                FirstMovePrompt;
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds +=
                StartCuratorPings;
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds +=
                StartWindowMinimizing;
            CompilationHelperMvc.Instance.CompilationHelperController.onCompilationFinished += OnSuccessCompilation; // Win condition
            
            //Fail states
            CompilationHelperMvc.Instance.CompilationHelperController.onCompilationFailed += OnFailedCompilation;
        }

        public void FirstMovePrompt(int seconds)
        {
            if (seconds < SIMULATION_SECONDS * 0.1) // first prompt at 1/10th of the total time
            {
                return;
            }

            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds -=
                FirstMovePrompt;

            FourthWallMvc.Instance.CompilationSimulationController.FirstMovePrompt();
        }

        public void StartCuratorPings(int seconds)
        {
            if (seconds < THIRD_OF_SIMULATION)
            {
                return;
            }

            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds -=
                StartCuratorPings;

            FourthWallMvc.Instance.CompilationSimulationController.StartCuratorPings();
        }

        public void StartWindowMinimizing(int seconds)
        {
            if (seconds < TWO_THIRDS_OF_SIMULATION)
            {
                return;
            }

            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds -=
                StartWindowMinimizing;
            
            FourthWallMvc.Instance.CompilationSimulationController.StartLastCompilationComplication();
        }

        public void OnSuccessCompilation()
        {
            //todo
            Debug.Log("we won");
        }

        public void OnFailedCompilation()
        {
            //todo
            Debug.Log("we lost");
        }
    }
}