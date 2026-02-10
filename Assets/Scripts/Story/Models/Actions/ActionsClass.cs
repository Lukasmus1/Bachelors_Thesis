using System;
using System.IO;
using System.Text;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
using Story.Models.SideActions;
using UnityEngine;
using User.Commons;
using User.Models;
using Object = UnityEngine.Object;

namespace Story.Models.Actions
{
    /// <summary>
    /// This class handles the execution of various actions within the story.
    /// For reasons described in <see cref="ListOfActionsPersistent"/>
    /// </summary>
    public class ActionsClass
    {
        //Singleton instance
        private static ActionsClass _instance;
        public static ActionsClass Instance
        {
            get
            {
                _instance ??= new ActionsClass();
                return _instance;
            }
        }

        public Action actionsToPerformOnLoad;
        
        private ListOfActionsPersistent _actionsPersistent = new();
        public ListOfActionsPersistent ActionsPersistent 
        {
            get => _actionsPersistent;
            set
            {
                _actionsPersistent = value;
                LoadFromState();
            }
            
        }

        /// <summary>
        /// Method to perform actions that need to be executed on load.
        /// This exists because the LoadFromState is called when we are not on the correct scene yet.
        /// </summary>
        public void PerformActionsOnLoad()
        {
            actionsToPerformOnLoad?.Invoke();
            
            actionsToPerformOnLoad = null;
        }
        
        /// <summary>
        /// Method to load actions from persistent state.
        /// </summary>
        private void LoadFromState()
        {
            foreach (Pair action in ActionsPersistent.actions)
            {
                if (!action.value)
                {
                    continue;
                }

                actionsToPerformOnLoad += () => PerformAction(action.key);
            }
        }
        
        
        /// <summary>
        /// Performs the action specified by the ActionType enum.
        /// </summary>
        /// <param name="type">ActionType</param>
        /// <exception cref="ArgumentOutOfRangeException">Gets thrown if we use unspecified ActionType</exception>
        public void PerformAction(ActionType type)
        {
            switch (type)
            {
                case ActionType.HiddenVirus:
                    PerformHiddenVirusAction();
                    ActionsPersistent.SetAction(ActionType.HiddenVirus, true);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        /// <summary>
        /// Performs cleanup for the action specified by the ActionType enum.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Gets thrown if we use unspecified ActionType</exception>
        public void CleanupAction()
        {
            foreach (Pair action in ActionsPersistent.actions)
            {
                if (!action.value)
                    continue;
                
                switch (action.key)
                {
                    case ActionType.HiddenVirus:
                        PerformHiddenVirusAction();
                        ActionsPersistent.SetAction(ActionType.HiddenVirus, true);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        //Hidden Virus Action
        private AsyncTimer t1;
        private FileDeletionDetectionModel fileDeleteDetection;
        
        /// <summary>
        /// Performs the hidden virus action.
        /// </summary>
        /// <exception cref="Exception">Gets thrown only when I change the tag on ScriptHolder object</exception>
        private void PerformHiddenVirusAction()
        {
            //Create hidden virus file
            string fileName = UserMvc.Instance.UserController.ProceduralData(ProceduralDataType.VirusName);
            string fileContent = UserMvc.Instance.UserController.ProceduralData(ProceduralDataType.VirusContent);
            FourthWallMvc.Instance.FileGenerationController.CreateHiddenFile(fileName, fileContent);

            //Attach file deletion detection
            GameObject scriptHolder = GameObject.FindWithTag("ScriptHolder");
            if (scriptHolder == null)
            {
                throw new Exception("ScriptHolder not found in scene. Cannot attach FileDeletionDetectionModel.");
            }
            fileDeleteDetection = scriptHolder.AddComponent<FileDeletionDetectionModel>();
            fileDeleteDetection.StartDetection(fileName, CleanupHiddenVirusAction);
            
            //Start timer
            StartTimerForHiddenVirusCancellation(fileName);
        }

        /// <summary>
        /// Definition for starting the timer for hidden virus cancellation. Uses recursion.
        /// </summary>
        /// <param name="fileName">Name of the hidden file</param>
        private void StartTimerForHiddenVirusCancellation(string fileName)
        {
            t1 = new AsyncTimer();
            _ = t1.StartTimer(120, () =>
            {
                if (fileDeleteDetection.IsFileDeleted())
                {
                    CleanupHiddenVirusAction();
                    return;
                }
                if (fileDeleteDetection == null)
                {
                    return;
                }
                
                var message = $"Windows detected a hidden corrupted file in your system: {Path.GetFullPath(fileName)}";
                WindowsErrorHandling.ThrowError(message, "File Corruption Error");
                
                StartTimerForHiddenVirusCancellation(fileName);
            });
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Cleanup after the hidden virus action.
        /// </summary>
        private void CleanupHiddenVirusAction()
        {
            //Stop timer
            t1.Dispose();

            //Remove file deletion detection
            if (fileDeleteDetection)
            {
                Object.Destroy(fileDeleteDetection);
                fileDeleteDetection = null;
            }
            
            //Destroy hidden virus file
            string fileName = UserMvc.Instance.UserController.ProceduralData(ProceduralDataType.VirusName);
            FourthWallMvc.Instance.FileGenerationController.DestroyFile(fileName);
            
            //Cleanup action flag
            ActionsPersistent.SetAction(ActionType.HiddenVirus, false);
            
            //Follow up with the AI messaging the player
            KpDoNotDeleteFiles.MessagePlayer();
        }
    }
}