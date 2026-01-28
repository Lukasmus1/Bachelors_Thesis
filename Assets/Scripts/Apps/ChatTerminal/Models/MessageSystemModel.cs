using System;
using System.Collections;
using Apps.ChatTerminal.Commons;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    public class MessageSystemModel
    {
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
                foreach (ChatMessage message in CurrentProfile.Messages[i])
                {
                    
                    //Simulate typing delay based on typing speed divided by number of chars
                    var delayS = (int)(message.Text.Length / CurrentProfile.TypingSpeed);
                    yield return new WaitForSeconds(delayS);

                    CreateMessage(message);
                }

                CreateDivider();
                CurrentProfile.SeenMessagesIndex++;
            }

            CurrentProfile.Status = MessageStatus.Offline;
        }

        private void InterruptedMessaging(int startIndex)
        {
            CurrentProfile.Status = MessageStatus.Offline;
            CurrentProfile.SeenMessagesIndex = startIndex;
        }
        
        private static void CreateMessage(ChatMessage content)
        {
            ChatTerminalMvc.Instance.MessageSystemController.CreateMessage(content);
        }

        private static void CreateDivider()
        {
            ChatTerminalMvc.Instance.MessageSystemController.CreateDivider();
        }
    }
}