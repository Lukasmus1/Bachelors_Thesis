using System;
using System.Text;
using Commons;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
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

        private readonly ListOfActionsPersistent _actionsPersistent = new();
        
        /// <summary>
        /// Performs the action specified by the ActionType enum.
        /// </summary>
        /// <param name="k">ActionType</param>
        /// <exception cref="ArgumentOutOfRangeException">Gets thrown if we use unspecified ActionType</exception>
        public void PerformAction(ActionType k)
        {
            switch (k)
            {
                case ActionType.HiddenVirus:
                    PerformHiddenVirusAction();
                    _actionsPersistent.SetAction(ActionType.HiddenVirus, true);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(k), k, null);
            }
        }
        
        
        //Hidden Virus Action
        private AsyncTimer t1;
        private AsyncTimer t2;
        private FileDeletionDetectionModel fileDeleteDetection;
        
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
            fileDeleteDetection.StartDetection(fileName, () => Debug.Log("[Hidden Virus Action] Hidden virus file deleted by user."));
            
            //Start timers
            t1 = new AsyncTimer();
            t2 = new AsyncTimer();
            _ = t1.StartTimer(5, () =>
            {
                byte[] data = Encoding.Default.GetBytes(fileContent);
                WindowsErrorHandling.ThrowError($"A file has been corrupted: {BitConverter.ToString(data)}", "File Corruption Error");
            });
            
            _ = t2.StartTimer(10, () => Debug.Log("[Hidden Virus Action] Timer 2 finished."));
        }

        private void CancelHiddenVirusAction()
        {
            t1.Dispose();
            t2.Dispose();

            if (fileDeleteDetection != null)
            {
                Object.Destroy(fileDeleteDetection);
                fileDeleteDetection = null;
            }
            
            string fileName = UserMvc.Instance.UserController.ProceduralData(ProceduralDataType.VirusName);
            FourthWallMvc.Instance.FileGenerationController.DestroyFile(fileName);
        }
    }
}