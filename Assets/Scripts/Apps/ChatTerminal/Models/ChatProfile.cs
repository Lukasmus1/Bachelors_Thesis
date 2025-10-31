using System;
using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    public class ChatProfile : MonoBehaviour
    {
        public string UserID { get; set; }
        public string Username { get; set; }

        public event Action<MessageStatus> MessageStatusChanged;
        private MessageStatus _status;
        public MessageStatus Status 
        { 
            get => _status;
            set
            {
                if (value == MessageStatus.Offline)
                {
                    _status = CurrentMessageIndex > SeenMessagesIndex ? MessageStatus.NewMessage : MessageStatus.Offline;
                }
                else
                {
                    _status = value;   
                }
                MessageStatusChanged?.Invoke(_status);
            }
        }
        public Sprite ProfilePicture { get; set; }
        public float TypingSpeed { get; set; }
        public int CurrentMessageIndex { get; set; }
        public int SeenMessagesIndex { get; set; }
        public List<List<string>> Messages { get; set; }
        public bool IsLoaded { get; set; }
        
        public void LoadData(ChatProfileModel data)
        {
            UserID = data.UserID;
            Username = data.Username;
            CurrentMessageIndex = data.CurrentMessageIndex;
            SeenMessagesIndex = data.SeenMessagesIndex;
            ProfilePicture = data.ProfilePicture;
            TypingSpeed = data.TypingSpeed;
            Messages = data.Messages;
            Status = data.Status;
            IsLoaded = true;
        }
    }
}