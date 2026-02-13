using System;
using System.Collections.Generic;
using Apps.VigenereCipher.Commons;
using FourthWall.Commons;
using UnityEngine;
using User.Commons;

namespace User.Models
{
    [Serializable]
    public class UserModel
    {
        public string Username { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// List of procedurally generated data. 
        /// </summary>
        public List<ProceduralDataEntry> ProceduralData { get; set; }
        
        /// <summary>
        /// List of persistent data that can be changed during the game and is used to track important events or actions of the user.
        /// </summary>
        public List<PersistentData> PersistentData { get; set; }
        
        public void InitUser()
        {
            StartDate = DateTime.Now;

            ProceduralData = new List<ProceduralDataEntry>
            {
                new(UserDataType.VignereCode, CipherMvc.Instance.CipherController.GenerateVigenereKey(5)),
                new(UserDataType.VirusName, FourthWallMvc.Instance.FileGenerationController.GenerateRandomFileName()),
                new(UserDataType.VirusContent, FourthWallMvc.Instance.FileGenerationController.GenerateFileData()),
                new(UserDataType.PictureCodeYear, CipherMvc.Instance.CipherController.GeneratePictureYearCode()),
                new(UserDataType.UserRealName, CipherMvc.Instance.CipherController.GenerateRandomName()),
                //Both the user year and user real name are generated once and then saved to be the same for the entire playthrough
                //The code itself is a combination of the two
                new(UserDataType.PictureCode, 
                    CipherMvc.Instance.CipherController.GenerateRandomName()+ 
                    CipherMvc.Instance.CipherController.GeneratePictureYearCode()), 
            };
            
            PersistentData = new List<PersistentData>
            {
                new(UserDataType.DeletedVirusFile, false)
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
    public class PersistentData
    {
        public UserDataType dataType;
        public bool dataValue;
        
        public PersistentData(UserDataType type, bool value)
        {
            dataType = type;
            dataValue = value;
        }
    }
    
    [Serializable]
    public class ProceduralDataEntry
    {
        public UserDataType dataType;
        public string dataValue;
        
        public ProceduralDataEntry(UserDataType type, string value)
        {
            dataType = type;
            dataValue = value;
        }
    }
}