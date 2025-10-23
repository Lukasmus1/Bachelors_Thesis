using System;
using Apps.ChatTerminal.Models;
using Apps.ChatTerminal.Views;

namespace Apps.ChatTerminal.Controllers
{
    public class MessageSystemController
    {
        private MessageSystemView _messageSystemView;
        private readonly MessageSystemModel _messageSystemModel = new();
        
        public void SetView(MessageSystemView view)
        {
            _messageSystemView = view;
        }
        
        public ChatProfile CurrentProfile 
        {
            get => _messageSystemModel.profile;
            set => _messageSystemModel.profile = value;
        }
        
        public void CreateMessage(string content)
        {
            _messageSystemView.CreateMessage(content);    
        }
        
        public void CreateDivider()
        {
            _messageSystemView.CreateDivider();    
        }
        
        public void StartMessaging()
        {
            _messageSystemModel.StartMessaging();
        }
        
        public void PrepareMessageView()
        {
            _messageSystemView.SetProperties();
            _messageSystemView.ClearMessages();
        }
    }
}