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
        
        public string ProceduralData(ProceduralDataType dataType)
        {
            return userModel.ProceduralData.Find(data => data.dataType == dataType)?.dataValue;
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