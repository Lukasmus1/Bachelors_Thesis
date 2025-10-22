using System.Collections.Generic;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    public class ChatProfile : MonoBehaviour
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public MessageStatus Status { get; set; }
        public Sprite ProfilePicture { get; set; }
        public float TypingSpeed { get; set; }
        public int CurrentMessageIndex { get; set; }
        public List<List<string>> Messages { get; set; }
        public bool IsLoaded { get; set; }
        
        public void LoadData(ChatProfileModel data)
        {
            UserID = data.UserID;
            Username = data.Username;
            Status = data.Status;
            ProfilePicture = data.ProfilePicture;
            TypingSpeed = data.TypingSpeed;
            Messages = data.Messages;
            CurrentMessageIndex = data.CurrentMessageIndex;
            IsLoaded = true;
        }
    }
}