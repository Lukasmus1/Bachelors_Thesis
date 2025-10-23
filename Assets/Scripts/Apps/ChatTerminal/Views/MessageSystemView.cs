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
        [SerializeField] private GameObject messageDividerPrefab;
        
        [Header("Contact Info")]
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private Image profilePictureImage;
        [SerializeField] private TMP_Text statusText;

        private void OnEnable()
        {
            ChatTerminalMvc.Instance.MessageSystemController.SetView(this);
            ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.MessageStatusChanged += SetStatusText;
        }
        private void OnDisable()
        {
            ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.MessageStatusChanged -= SetStatusText;
        }
        
        public void SetProperties()
        {
            usernameText.text = ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.Username;
            //profilePictureImage.sprite = currentProfile.ProfilePicture;
            SetStatusText(ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.Status);
        }
        
        public void ClearMessages()
        {
            foreach (Transform child in messagePrefabParent)
            {
                Destroy(child.gameObject);
            }
        }
        
        private void SetStatusText(MessageStatus status)
        {
            switch (status)
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
        
        public void CreateDivider()
        { 
            Instantiate(messageDividerPrefab, messagePrefabParent).SetActive(true);
        }
    }
}