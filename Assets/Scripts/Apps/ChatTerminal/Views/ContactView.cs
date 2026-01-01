using System;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.ChatTerminal.Views
{
    public class ContactView : MonoBehaviour
    {
        [SerializeField] private GameObject messagesWindow;
        
        private ChatProfile _profileModel;
        private ContactProperties _props;
        
        private void OnDestroy()
        {
            _profileModel.MessageStatusChanged -= SetStatusText;
        }

        public void SetProperties(GameObject messagesWin, ChatProfile profile)
        {
            messagesWindow = messagesWin;

            _profileModel = profile;
            _profileModel.MessageStatusChanged += SetStatusText;
            
            _props = GetComponent<ContactProperties>();
            
            _props.usernameText.text = _profileModel.Username;
            SetStatusText(_profileModel.Status);
            
            gameObject.SetActive(true);
        }
        
        public void OnClick()
        {
            //This is called to stop any ongoing messaging and set the status to offline, hence this must be called before setting the current profile
            ChatTerminalMvc.Instance.MessageSystemController.StopMessaging();
            ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile = GetComponent<ChatProfile>();
            
            messagesWindow.SetActive(true); //This needs to be called before setting properties
            ChatTerminalMvc.Instance.MessageSystemController.PrepareMessageView(); //Must be called after activating the messages window
            
            ChatTerminalMvc.Instance.MessageSystemController.StartMessaging();
            
            ChatTerminalMvc.Instance.MessageSystemController.openedContact?.Invoke(_profileModel.UserID);
        }

        /// <summary>
        /// Sets the status text based on the provided message status.
        /// </summary>
        /// <param name="status">Message status</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when we provide a new MessageStatus</exception>
        private void SetStatusText(MessageStatus status)
        {
            switch (status)
            {
                case MessageStatus.NewMessage:
                    _props.statusText.text = "NEW MESSAGE";
                    _props.statusText.color = Color.red;
                    break;
                
                case MessageStatus.Typing:
                    _props.statusText.text = "TYPING...";
                    _props.statusText.color = Color.black;
                    break;
                
                case MessageStatus.Offline:
                    _props.statusText.text = "OFFLINE";
                    _props.statusText.color = Color.black;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}