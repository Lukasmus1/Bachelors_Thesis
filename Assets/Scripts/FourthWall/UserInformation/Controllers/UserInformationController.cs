using System;
using Commons;
using FourthWall.UserInformation.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FourthWall.UserInformation.Controllers
{
    public class UserInformationController
    {
        private readonly UserInformationModel userModel = new();
        private readonly Screenshot screenshot = new();
        
        /// <summary>
        /// Gets the real name of the user saved in the system.
        /// </summary>
        /// <returns>String of user's real name</returns>
        public string GetUserRealName()
        {
            return userModel.UserRealName;
        }

        /// <summary>
        /// Gets the real full name of the user saved in the system.
        /// </summary>
        /// <returns>String of the user's real full name</returns>
        public string GetUserRealFullName()
        {
            return userModel.UserRealFullName;
        }
        
        /// <summary>
        /// Gets a screenshot of the current screen and returns it as a Texture2D via a callback.
        /// </summary>
        /// <param name="monoBehaviour">MonoBehavior used for coroutine</param>
        /// <param name="callback">Callback function to get the texture</param>
        public void Screenshot(MonoBehaviour monoBehaviour, Action<Texture2D> callback)
        {
            screenshot.GetScreenshot(monoBehaviour, callback);
        }

        /// <summary>
        /// Sets up the detection of disconnection from the internet.
        /// </summary>
        /// <param name="callback">What should happen after the user disconnects from the internet</param>
        public void SetupInternetDisconnectDetection(Action callback)
        {
            GameObject scriptHolder = Tools.GetScriptHolder();
            var model = scriptHolder.AddComponent<InternetDetectionModel>();
            model.StartDetection(callback);
        }

        /// <summary>
        /// Tries to stop the current internet disconnection detection.
        /// </summary>
        public void StopRunningInternetDisconnectDetection()
        {
            GameObject scriptHolder = Tools.GetScriptHolder();
            if (scriptHolder.TryGetComponent(out InternetDetectionModel model))
            {
                model.StopDetection();
            }
            else
            {
                Debug.LogWarning("Trying to stop internet disconnect detection, but no detection is running.");
            }
        }
    }
}