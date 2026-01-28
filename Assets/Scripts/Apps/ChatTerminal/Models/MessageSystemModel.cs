using System;
using System.Collections;
using System.Threading.Tasks;
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

        private bool _stopMessaging;
        private bool _isMessaging;
        
        public void StartMessaging(MonoBehaviour coroutineHost)
        {
            _stopMessaging = false;
            coroutineHost.StartCoroutine(TypingAsync());
        }

        public void StopMessaging()
        {
            if (!_isMessaging)
            {
                return;
            }
            
            _stopMessaging = true;
            CurrentProfile.Status = MessageStatus.Offline;
        }
        
        /// <summary>
        /// Simulate typing messages with delay based on typing speed.
        /// </summary>
        private IEnumerator TypingAsync()
        {
            bool isIndexOverMessages = CurrentProfile.CurrentMessageIndex >= CurrentProfile.Messages.Count;
            
            //Save start index in case of cancellation
            int startIndex = CurrentProfile.SeenMessagesIndex;
            
            //Try to type new messages
            //If canceled, revert seen index to start index
            //Create message history
            for (int i = 0; i < CurrentProfile.SeenMessagesIndex; i++)
            {
                _isMessaging = true;
                
                foreach (ChatMessage oldMsg in CurrentProfile.Messages[i])
                {
                    if (_stopMessaging)
                    {
                        InterruptedMessaging(startIndex);
                        yield break;
                    }
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
                    if (_stopMessaging)
                    {
                        InterruptedMessaging(startIndex);
                        yield break;
                    }
                        
                    //Simulate typing delay based on typing speed divided by number of chars
                    var delayS = (int)(message.Text.Length / CurrentProfile.TypingSpeed);
                    yield return new WaitForSeconds(delayS);
                    
                    if (_stopMessaging)
                    {
                        InterruptedMessaging(startIndex);
                        yield break;
                    }

                    CreateMessage(message);
                }

                CreateDivider();
                CurrentProfile.SeenMessagesIndex++;
            }

            CurrentProfile.Status = MessageStatus.Offline;
            _isMessaging = false;
        }

        private void InterruptedMessaging(int startIndex)
        {
            CurrentProfile.Status = MessageStatus.Offline;
            CurrentProfile.SeenMessagesIndex = startIndex;
            _isMessaging = false;
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