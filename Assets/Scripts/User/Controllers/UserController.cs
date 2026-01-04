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