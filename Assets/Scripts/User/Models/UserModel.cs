using System;
using System.Collections.Generic;
using System.Linq;
using Apps.CipherSolver.Commons;
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
        
        /// <summary>
        /// Minus values for more AI aligned, plus values for more curator aligned. Used for certain dialogue choices and endings.
        /// </summary>
        public int CuratorAlignment { get; set; }
        
        /// <summary>
        /// Screenshot of the user's game.
        /// </summary>
        public byte[] EncryptedGameScreenshot { get; set; }
        public int ScreenshotWidth { get; set; }
        public int ScreenshotHeight { get; set; }
        public int ScreenshotFormat { get; set; }
        
        /// <summary>
        /// Path to the file where the curator explains the lore.
        /// </summary>
        public string CuratorExplanationFilePath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\_README_.txt";
        
        public void InitUser()
        {
            StartDate = DateTime.Now;

            ProceduralData = new List<ProceduralDataEntry>
            {
                new(UserDataType.VignereCode, CipherMvc.Instance.CipherController.GenerateVigenereKey(5)),
                new(UserDataType.VirusName, FourthWallMvc.Instance.FileGenerationController.GenerateRandomFileName()),
                new(UserDataType.VirusContent, FourthWallMvc.Instance.FileGenerationController.GenerateFileData()),
                new(UserDataType.PictureCodeYear, CipherMvc.Instance.CipherController.GeneratePictureYearCode()),
                new(UserDataType.ScammerName, CipherMvc.Instance.CipherController.GenerateRandomName()),
                new(UserDataType.NumberPatternCode, FourthWallMvc.Instance.NumberPatternController.CreateRandomNumberPattern()),
                new(UserDataType.ImportantFileLocation, FourthWallMvc.Instance.FileGenerationController.CreateImportantHiddenFileLocation()),
                new(UserDataType.LastFileLocation, FourthWallMvc.Instance.FileGenerationController.CreateLastFileLocation()),
            };
            string pictureCode = UserMvc.Instance.UserController.ProceduralData(UserDataType.ScammerName) +
                                 UserMvc.Instance.UserController.ProceduralData(UserDataType.PictureCodeYear);
            
            ProceduralData.Add(new ProceduralDataEntry(
                UserDataType.PictureCode, pictureCode));
            
            
            PersistentData = new List<PersistentData>
            {
                new(UserDataType.DeletedVirusFile, false),
                new(UserDataType.FirstChoiceSideWithCops, false),
                new(UserDataType.LastHelpChoiceHelp, false)
            };
            
            SetProfilePicture();

            CuratorAlignment = 0;
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