using System;
using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    public class ChatProfile : MonoBehaviour
    {
        public string UserID { get; private set; }
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
                    UpdateStatusBasedOnMessages();
                }
                else
                {
                    _status = value;
                    MessageStatusChanged?.Invoke(_status);
                }
            }
        }
        public Sprite ProfilePicture { get; set; }
        public float TypingSpeed { get; set; }
        
        public int currentMessageIndex;
        public int CurrentMessageIndex
        {
            get => currentMessageIndex;
            set
            {
                currentMessageIndex = value;
                UpdateStatusBasedOnMessages();
            }
        }
        
        private void UpdateStatusBasedOnMessages()
        {
            _status = CurrentMessageIndex >= SeenMessagesIndex ? MessageStatus.NewMessage : MessageStatus.Offline;
            MessageStatusChanged?.Invoke(_status);
        }
        public int SeenMessagesIndex { get; set; }
        public List<List<ChatMessage>> Messages { get; private set; }
        public bool IsLoaded { get; private set; }
        
        public void LoadData(ChatProfileModel data)
        {
            UserID = data.UserID;
            Username = data.Username;
            CurrentMessageIndex = data.CurrentMessageIndex;
            SeenMessagesIndex = data.SeenMessagesIndex;
            
            var tex = new Texture2D(2, 2);
            tex.LoadImage(data.ProfilePictureBytes);
            ProfilePicture = ProfilePicture = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            
            TypingSpeed = data.TypingSpeed;
            Messages = data.Messages;
            Status = data.Status;
            IsLoaded = true;
        }
    }
}