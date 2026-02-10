using System;
using UnityEngine;
using User.Models;

namespace User.Controllers
{
    public class UserController
    {
        public UserModel userModel = new();
        
        public string Username
        {
            get => userModel.Username;
            set => userModel.Username = value;
        }
        
        public Sprite GetProfilePicture()
        {
            var tex = new Texture2D(2, 2);
            tex.LoadImage(userModel.ProfilePicture);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        
        /// <summary>
        /// Returns the value of a procedural data entry based on the provided UserDataType.
        /// </summary>
        /// <param name="dataType">Data type to get</param>
        /// <returns>Value of the procedural type</returns>
        public string ProceduralData(UserDataType dataType)
        {
            return userModel.ProceduralData.Find(data => data.dataType == dataType)?.dataValue;
        }
        
        /// <summary>
        /// Returns the value of the persistent data entry based on the provided UserDataType. If no entry is found, it returns false.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public bool GetPersistentData(UserDataType dataType)
        {
            return userModel.PersistentData.Find(data => data.dataType == dataType)?.dataValue ?? false;
        }

        /// <summary>
        /// Sets the value of a persistent data entry based on the provided UserDataType.
        /// </summary>
        /// <param name="dataType">Data type to set</param>
        /// <param name="value">Value to set</param>
        public void SetPersistentData(UserDataType dataType, bool value)
        {
            userModel.PersistentData.Find(data => data.dataType == dataType).dataValue = value;
        }
        
        public DateTime GetStartDate()
        {
            return userModel.StartDate;
        }
        
        public void InitUser()
        {
            userModel.InitUser();
        }
    }
}