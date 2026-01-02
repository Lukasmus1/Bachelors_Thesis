using System;
using System.Collections.Generic;
using Apps.Autostereogram.Commons;
using UnityEngine;
using UnityEngine.UI;

namespace User.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public Texture2D ProfilePicture { get; set; }
        public DateTime StartDate { get; set; }
        public Dictionary<ProceduralDataType, string> ProceduralData { get; set; }
        
        public void InitUser()
        {
            StartDate = DateTime.Now;

            ProceduralData = new Dictionary<ProceduralDataType, string>
            {
                { ProceduralDataType.VignereCode, ProceduralDataGeneration.GenerateVigenereCode(5) },
            };
        }
    }
}