using System.Collections.Generic;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    public class ChatProfile : MonoBehaviour
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Status { get; set; } = "";
        public Sprite ProfilePicture { get; set; }
        public float TypingSpeed { get; set; }
        public List<List<string>> Messages { get; set; } = new();
        public bool IsLoaded { get; set; }
        
        public void LoadData(ChatProfileModel data)
        {
            UserID = data.UserID;
            Username = data.Username;
            Status = data.Status;
            ProfilePicture = data.ProfilePicture;
            TypingSpeed = data.TypingSpeed;
            Messages = data.Messages;
            IsLoaded = true;
        }
    }
}