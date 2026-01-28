using System;
using Apps.ChatTerminal.Models;
using Apps.ChatTerminal.Views;
using UnityEngine;

namespace Apps.ChatTerminal.Controllers
{
    public class MessageSystemController
    {
        public Action<string> openedContact;
        
        private MessageSystemView _messageSystemView;
        private readonly MessageSystemModel _messageSystemModel = new();
        
        public void SetView(MessageSystemView view)
        {
            _messageSystemView = view;
        }
        
        public ChatProfile CurrentProfile 
        {
            get => _messageSystemModel.CurrentProfile;
            set => _messageSystemModel.CurrentProfile = value;
        }
        
        public void CreateMessage(ChatMessage content)
        {
            _messageSystemView.CreateMessage(content);    
        }
        
        public void CreateDivider()
        {
            _messageSystemView.CreateDivider();    
        }
        
        public void StartMessaging(MonoBehaviour coroutineHost)
        {
            _messageSystemModel.StartMessaging(coroutineHost);
        }
        
        public void StopMessaging(MonoBehaviour coroutineHost)
        {
            _messageSystemModel.StopMessaging(coroutineHost);
        }
        
        public void PrepareMessageView()
        {
            _messageSystemView.SetProperties();
            _messageSystemView.ClearMessages();
        }
    }
}