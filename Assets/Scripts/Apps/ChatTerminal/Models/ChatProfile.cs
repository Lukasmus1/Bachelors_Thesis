using System;
using System.Collections.Generic;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    [Serializable]
    public class ChatProfile
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public Sprite ProfilePicture { get; set; }
        public float TypingSpeed { get; set; }
        public List<List<string>> Messages { get; set; } = new();
        public bool IsLoaded { get; set; } = false;
    }
}