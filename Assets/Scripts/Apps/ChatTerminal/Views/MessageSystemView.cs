using System;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User.Commons;

namespace Apps.ChatTerminal.Views
{
    public class MessageSystemView : MonoBehaviour
    {
        [Header("Message")]
        [SerializeField] private GameObject recievedMessagePrefab;
        [SerializeField] private GameObject sentMessagePrefab;
        [SerializeField] private Transform messagePrefabParent;
        [SerializeField] private GameObject messageDividerPrefab;
        
        [Header("Contact Info")]
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private Image profilePictureImage;
        [SerializeField] private TMP_Text statusText;

        private void OnEnable()
        {
            ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.MessageStatusChanged += SetStatusText;
            SetStatusText(ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.Status);
        }
        private void OnDisable()
        {
            ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.MessageStatusChanged -= SetStatusText;
        }
        
        /// <summary>
        /// Sets the properties of the message system view based on the current profile.
        /// </summary>
        public void SetProperties()
        {
            usernameText.text = ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.Username;
            profilePictureImage.sprite = ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.ProfilePicture;
            SetStatusText(ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.Status);
        }
        
        /// <summary>
        /// Clears all messages from the message view.
        /// </summary>
        public void ClearMessages()
        {
            foreach (Transform child in messagePrefabParent)
            {
                Destroy(child.gameObject);
            }
        }
        
        /// <summary>
        /// Sets the status text based on the message status.
        /// </summary>
        /// <param name="status">Status to set</param>
        /// <exception cref="ArgumentOutOfRangeException">Gets thrown if we use unspecified MessageStatus</exception>
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
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Creates a message in the message view.
        /// </summary>
        /// <param name="messageContent">Content of the message</param>
        public void CreateMessage(ChatMessage messageContent)
        {
            GameObject msg = Instantiate(messageContent.Sender == "Player" ? sentMessagePrefab : recievedMessagePrefab, messagePrefabParent);

            var props = msg.GetComponent<MessageProperties>();
            
            props.usernameText.text = messageContent.Sender == "Player" ? UserMvc.Instance.UserController.Username : ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.Username;
            props.profilePicture.sprite = messageContent.Sender == "Player" ? UserMvc.Instance.UserController.GetProfilePicture() : ChatTerminalMvc.Instance.MessageSystemController.CurrentProfile.ProfilePicture;
            props.messageText.text = messageContent.Text;
            
            msg.SetActive(true);
        }
        
        /// <summary>
        /// Creates a divider in the message view.
        /// </summary>
        public void CreateDivider()
        { 
            Instantiate(messageDividerPrefab, messagePrefabParent).SetActive(true);
        }
    }
}