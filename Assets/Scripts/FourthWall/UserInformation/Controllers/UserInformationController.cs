using System;
using FourthWall.UserInformation.Models;
using UnityEngine;

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
    }
}