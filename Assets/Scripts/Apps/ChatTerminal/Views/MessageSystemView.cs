using System;
using System.Collections.Generic;
using System.Linq;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using Story.Models.Choices;
using Story.Models.Choices.ChoiceClasses;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
        
        [Header("Choice Message")]
        [SerializeField] private GameObject choiceMessagePrefab;
        [SerializeField] private GameObject choicePrefab;
        [SerializeField] private Transform choiceBubbleParent;
        private const string SEPARATOR = "||";

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

        
        public void CreateChoiceMessage(ChatMessage content)
        {
            GameObject choiceMsg = Instantiate(choiceMessagePrefab, messagePrefabParent);
            Transform choicesParent = choiceMsg.transform.Find("Choices");
            
            string choiceId = content.MessageID;
            List<ChoiceActionClass> choiceClass = ChoiceFactory.GetChoiceClass(choiceId);

            string[] choicesText = content.Text.Split(SEPARATOR);
            foreach (string choice in choicesText)
            {
                string choiceCopy = choice;
                int choiceID = GetChoiceIndex(ref choiceCopy);
                
                GameObject choiceButton = Instantiate(choicePrefab, choicesParent);
                
                var choiceView = choiceButton.GetComponent<ChoiceView>();
                choiceView.SetBubbleParent(choiceBubbleParent);
                choiceView.SetText(choiceCopy);
                
                UnityAction action = choiceClass.FirstOrDefault(c => c.ChoiceID == choiceID)?.ChoiceAction;
                if (action == null)
                {
                    throw new Exception("Couldn't get the choice action");
                }
                
                choiceButton.GetComponent<Button>().onClick.AddListener(action);
            }
        }

        /// <summary>
        /// Gets the index of the choice from the choice text. The choice text is in the format "{index}Choice Text".
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static int GetChoiceIndex(ref string choice)
        {
            int startIndex = choice.IndexOf('{');
            int endIndex = choice.IndexOf('}');

            if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
            {
                throw new Exception("Couldn't get the choice index number");
            }
            
            string indexString = choice.Substring(startIndex + 1, endIndex - startIndex - 1);
            choice = choice.Remove(startIndex, endIndex - startIndex + 1);
            
            return int.TryParse(indexString, out int index) ? index : throw new Exception("Couldn't parse the choice index number");
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