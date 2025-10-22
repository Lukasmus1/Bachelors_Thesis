using System;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.ChatTerminal.Views
{
    public class MessageSystemView : MonoBehaviour
    {
        [Header("Message")]
        [SerializeField] private GameObject messagePrefab;
        [SerializeField] private Transform messagePrefabParent;
        
        [Header("Contact Info")]
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private Image profilePictureImage;
        [SerializeField] private TMP_Text statusText;

        private void OnEnable()
        {
            ChatTerminalMvc.Instance.MessageSystemController.SetView(this);
        }

        public void SetProperties()
        {
            usernameText.text = ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.Username;
            //profilePictureImage.sprite = currentProfile.ProfilePicture;
            SetStatusText();
        }
        
        private void SetStatusText()
        {
            switch (ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.Status)
            {
                case MessageStatus.NewMessage:
                    statusText.text = "NEW MESSAGE";
                    statusText.color = Color.red;
                    break;
                
                case MessageStatus.Typing:
                    statusText.text = "TYPING...";
                    statusText.color = Color.black;
                    break;
                
                case MessageStatus.Offline:
                    statusText.text = "OFFLINE";
                    statusText.color = Color.black;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void CreateMessage(string messageContent)
        {
            GameObject msg = Instantiate(messagePrefab, messagePrefabParent);
            var props = msg.GetComponent<MessageProperties>();
            
            props.usernameText.text = ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.Username;
            //props.profilePicture.sprite = currentProfile.ProfilePicture;
            props.messageText.text = messageContent;
            
            msg.SetActive(true);
        }
    }
}