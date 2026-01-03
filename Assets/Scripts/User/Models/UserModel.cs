using System;
using System.Collections.Generic;
using Apps.VigenereCipher.Commons;
using Desktop.Commons;
using Unity.VisualScripting;
using UnityEngine;

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
                { ProceduralDataType.VignereCode, VigenereMvc.Instance.VigenereController.GenerateVigenereKey(5) },
            };
        }
    }
}