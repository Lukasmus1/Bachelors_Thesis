using System;
using System.Collections.Generic;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    [Serializable]
    public class ChatProfileModel
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public MessageStatus Status { get; set; }
        public byte[] ProfilePicture { get; set; }
        public float TypingSpeed { get; set; }
        public int CurrentMessageIndex { get; set; } = 0;
        public int SeenMessagesIndex { get; set; }
        public List<List<string>> Messages { get; set; } = new();
        public bool IsLoaded { get; set; }
    }
}