using System;
using System.Collections;
using System.Linq;
using Apps.ChatTerminal.Commons;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    public class MessageSystemModel
    {
        private const float PLAYER_TYPING_SPEED = 30f;

        //Choice messages helpers
        private bool _isPaused = false;
        private ChatMessage _messageToInsert;
        private int _currentMessageIndex;
        
        private ChatProfile _currentProfile;
        public ChatProfile CurrentProfile
        {
            get => _currentProfile;
            set
            {
                if (_currentProfile != null && _currentProfile.SeenMessagesIndex <= _currentProfile.CurrentMessageIndex)
                {
                    _currentProfile.Status = MessageStatus.NewMessage;
                }
                _currentProfile = value;
            }
        }

        private Coroutine _runningCoroutine;
        
        public void StartMessaging(MonoBehaviour coroutineHost)
        {
            _runningCoroutine = coroutineHost.StartCoroutine(TypingAsync());
        }

        public void StopMessaging(MonoBehaviour coroutineHost)
        {
            if (_runningCoroutine == null)
            {
                return;
            }
            
            InterruptedMessaging(CurrentProfile.SeenMessagesIndex);
            coroutineHost.StopCoroutine(_runningCoroutine);
            _runningCoroutine = null;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Simulate typing messages with delay based on typing speed.
        /// </summary>
        private IEnumerator TypingAsync()
        {
            bool isIndexOverMessages = CurrentProfile.CurrentMessageIndex >= CurrentProfile.Messages.Count;
            
            //Try to type new messages
            //If canceled, revert seen index to start index
            //Create message history
            for (int i = 0; i < CurrentProfile.SeenMessagesIndex; i++)
            {
                foreach (ChatMessage oldMsg in CurrentProfile.Messages[i])
                {
                    CreateMessage(oldMsg);
                }

                if (i == CurrentProfile.SeenMessagesIndex - 1 && isIndexOverMessages)
                {
                    break;
                }

                CreateDivider();
            }

            if (isIndexOverMessages)
            {
                Debug.Log("No more messages remaining");
                yield break;
            }

            CurrentProfile.Status = MessageStatus.Typing;

            for (int i = CurrentProfile.SeenMessagesIndex; i <= CurrentProfile.CurrentMessageIndex; i++)
            {
                _currentMessageIndex = i;
                for (var j = 0; j < CurrentProfile.Messages[i].Count; j++)
                {
                    ChatMessage message = CurrentProfile.Messages[i][j];
                    
                    //If the message is a choice 
                    if (message.Sender == "CHOICE")
                    {
                        CreateChoiceMessage(message);

                        TogglePause(true);
                        
                        yield return new WaitUntil(() => !_isPaused);
                        
                        //Deletes old choice message
                        int indexOfDeletedMessage = CurrentProfile.Messages[i].FindIndex(x => x.MessageID == message.MessageID); 
                        CurrentProfile.Messages[i].RemoveAt(indexOfDeletedMessage);
                        //Adds the player's choice to the message history and creates the message in the view
                        CurrentProfile.Messages[i].Insert(indexOfDeletedMessage, _messageToInsert);
                        
                        CreateMessage(_messageToInsert);
                        
                        continue;
                    }
                    
                    //Simulate typing delay based on typing speed divided by number of chars
                    float delayS;
                    if (message.Sender == "Player")
                    {
                        delayS = message.Text.Length / PLAYER_TYPING_SPEED;
                    }
                    else
                    {
                        delayS = message.Text.Length / CurrentProfile.TypingSpeed;
                    }

#if UNITY_EDITOR
                    delayS = 2f;             
#endif
                    yield return new WaitForSeconds(delayS);
                    
                    CreateMessage(message);
                    if (message.Sender == "Player")
                    {
                        ChatTerminalMvc.Instance.MessageSystemController.PlayOutcomingMessageNotif();
                    }
                    else
                    {
                        ChatTerminalMvc.Instance.MessageSystemController.PlayIncomingMessageNotif();
                    }
                    
                    ChatTerminalMvc.Instance.MessageSystemController.messageTyped?.Invoke(message.MessageID);
                    
#if UNITY_EDITOR
                    continue;             
#endif
                    //Small delay between each message
                    yield return new WaitForSeconds(0.5f);
                }
                CurrentProfile.SeenMessagesIndex++;
            }

            CurrentProfile.Status = MessageStatus.Offline;
        }

        /// <summary>
        /// Queues a secondary message from a choice, which allows for adding messages to the message system model based on user choices. ONLY USE THIS WHEN THE MESSAGE SYSTEM IS CURRENTLY PAUSED DURING CHOICE MAKING
        /// </summary>
        /// <param name="userID">ID of the user</param>
        /// <param name="messageGroupID">ID of the message group</param>
        /// <exception cref="Exception">Gets thrown with more details in description</exception>
        public void QueueSecondaryMessageFromChoice(string userID, string messageGroupID)
        {
            if (!_isPaused)
            {
                throw new Exception("Cannot alter the message queue if not currently paused.");
            }
            
            MessageGroup messages = ChatTerminalMvc.Instance.ChatTerminalController.GetSecondaryMessageGroup(userID, messageGroupID);
            if (messages == null)
            {
                throw new Exception($"Message group with ID {messageGroupID} not found for user ID {userID}.");
            }

            foreach (ChatMessage message in messages.MessagesGroup)
            {
                CurrentProfile.Messages[_currentMessageIndex].Add(message);
            }
        }
        
        private void InterruptedMessaging(int startIndex)
        {
            CurrentProfile.Status = MessageStatus.Offline;
            CurrentProfile.SeenMessagesIndex = startIndex;
        }

        public void TogglePause(bool value)
        {
            _isPaused = value;
        }
        
        public void AppendMessage(ChatMessage content)
        {
            _messageToInsert = content;
        }
        
        private static void CreateMessage(ChatMessage content)
        {
            ChatTerminalMvc.Instance.MessageSystemController.CreateMessage(content);
        }

        private static void CreateChoiceMessage(ChatMessage content)
        {
            ChatTerminalMvc.Instance.MessageSystemController.CreateChoiceMessage(content);
        }

        private static void CreateDivider()
        {
            ChatTerminalMvc.Instance.MessageSystemController.CreateDivider();
        }
    }
}