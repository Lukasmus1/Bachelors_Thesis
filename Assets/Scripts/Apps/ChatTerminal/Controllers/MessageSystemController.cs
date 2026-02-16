using System;
using Apps.ChatTerminal.Models;
using Apps.ChatTerminal.Views;
using UnityEngine;

namespace Apps.ChatTerminal.Controllers
{
    public class MessageSystemController
    {
        public Action<string> openedContact;
        public Action<string> messageTyped;
        
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
        
        /// <summary>
        /// Creates a new message in the message system view based on the provided content.
        /// </summary>
        /// <param name="content">Content of the message</param>
        public void CreateMessage(ChatMessage content)
        {
            _messageSystemView.CreateMessage(content);    
        }

        public void CreateChoiceMessage(ChatMessage content)
        {
            _messageSystemView.CreateChoiceMessage(content);
        }
        
        /// <summary>
        /// Creates a divider between message groups.
        /// </summary>
        public void CreateDivider()
        {
            _messageSystemView.CreateDivider();    
        }
        
        /// <summary>
        /// Starts the messaging process, which simulates typing messages with a delay based on the typing speed.
        /// </summary>
        /// <param name="coroutineHost">MonoBehaviour used for Coroutine</param>
        public void StartMessaging(MonoBehaviour coroutineHost)
        {
            _messageSystemModel.StartMessaging(coroutineHost);
        }
        
        /// <summary>
        /// Stops the messaging process, which interrupts the typing of messages and reverts the seen index to the start index.
        /// </summary>
        /// <param name="coroutineHost">MonoBehaviour used for Coroutine</param>
        public void StopMessaging(MonoBehaviour coroutineHost)
        {
            _messageSystemModel.StopMessaging(coroutineHost);
        }

        /// <summary>
        /// Toggles the pause state of the message system, allowing for pausing and resuming the messaging process.
        /// </summary>
        /// <param name="value">Should the system be paused?</param>
        public void ToggleMessagePause(bool value)
        {
            _messageSystemModel.TogglePause(value);
        }
        
        /// <summary>
        /// Prepares the message view by setting properties and clearing existing messages, ensuring a clean state for new interactions.
        /// </summary>
        public void PrepareMessageView()
        {
            _messageSystemView.SetProperties();
            _messageSystemView.ClearMessages();
        }
    }
}