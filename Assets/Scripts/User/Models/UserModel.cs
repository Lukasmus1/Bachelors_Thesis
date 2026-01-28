using System;
using System.Collections.Generic;
using Apps.VigenereCipher.Commons;
using UnityEngine;

namespace User.Models
{
    [Serializable]
    public class UserModel
    {
        public string Username { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime StartDate { get; set; }
        public List<ProceduralDataEntry> ProceduralData { get; set; }
        
        public void InitUser()
        {
            StartDate = DateTime.Now;

            ProceduralData = new List<ProceduralDataEntry>
            {
                new(ProceduralDataType.VignereCode, VigenereMvc.Instance.VigenereController.GenerateVigenereKey(5)),
            };
            
            SetProfilePicture();
        }
        
        public void SetProfilePicture()
        {
            var defaultProfilePic = Resources.Load<Texture2D>("Prefabs/Apps/ChatTerminal/PersonIcons/PFP");
            ProfilePicture = defaultProfilePic.EncodeToPNG();
        }
    }
    
    [Serializable]
    public class ProceduralDataEntry
    {
        public ProceduralDataType dataType;
        public string dataValue;
        
        public ProceduralDataEntry(ProceduralDataType type, string value)
        {
            dataType = type;
            dataValue = value;
        }
    }
}