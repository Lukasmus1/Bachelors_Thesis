using System;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using Apps.ChatTerminal.Views;
using Sounds.Commons;
using UnityEngine;
using AudioType = Sounds.Models.AudioType;

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

        /// <summary>
        /// Plays an incoming message sound.
        /// </summary>
        public void PlayIncomingMessageNotif()
        {
            ChatTerminalView view = ChatTerminalMvc.Instance.ChatTerminalController.GetChatTerminalView();
            AudioClip clip = view.messageRecievedSound;
            
            SoundMvc.Instance.SoundController.PlaySound(clip, view.transform, AudioType.Effects);
        }
        
        /// <summary>
        /// Plays an outcoming message sound.
        /// </summary>
        public void PlayOutcomingMessageNotif()
        {
            ChatTerminalView view = ChatTerminalMvc.Instance.ChatTerminalController.GetChatTerminalView();
            AudioClip clip = view.messageSentSound;
            
            SoundMvc.Instance.SoundController.PlaySound(clip, view.transform, AudioType.Effects);
        }

        /// <summary>
        /// Creates a new choice message in the message system view based on the provided content, allowing for user interaction and decision-making.
        /// </summary>
        /// <param name="content">Content of the message</param>
        public void CreateChoiceMessage(ChatMessage content)
        {
            _messageSystemView.CreateChoiceMessage(content);
        }

        /// <summary>
        /// Appends a choice message to the message system model. ONLY USE THIS WHEN APPENDING CHOICE MESSAGES DURING CHOICE MAKING.
        /// </summary>
        /// <param name="content">Message to append</param>
        public void AppendChoiceMessage(ChatMessage content)
        {
            _messageSystemModel.AppendMessage(content);
        }
        
        /// <summary>
        /// Queues a secondary message from a choice, which allows for adding messages to the message system model based on user choices. ONLY USE THIS WHEN APPENDING CHOICE MESSAGES DURING CHOICE MAKING.
        /// </summary>
        /// <param name="userID">ID of the user</param>
        /// <param name="messageGroupID">ID of the secondary message</param>
        public void QueueSecondaryMessageFromChoice(string userID, string messageGroupID)
        {
            _messageSystemModel.QueueSecondaryMessageFromChoice(userID, messageGroupID);
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