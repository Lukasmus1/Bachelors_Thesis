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
        
        private ChatProfile profileModel;
        private ContactProperties _props;
        
        public void SetProperties(GameObject messagesWin)
        {
            messagesWindow = messagesWin;
            
            profileModel = GetComponent<ChatProfile>();
            _props = GetComponent<ContactProperties>();
            
            _props.usernameText.text = profileModel.Username;
            SetStatusText();
            
            gameObject.SetActive(true);
        }
        
        public void OnClick()
        {
            ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile = GetComponent<ChatProfile>();
            messagesWindow.SetActive(true); //This needs to be called before setting properties
            ChatTerminalMvc.Instance.MessageSystemController.SetProperties(); //Must be called after activating the messages window
            ChatTerminalMvc.Instance.MessageSystemController.StartMessaging();
        }

        private void SetStatusText()
        {
            switch (profileModel.Status)
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