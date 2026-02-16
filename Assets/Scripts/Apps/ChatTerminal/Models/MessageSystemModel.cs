using System;
using System.Collections;
using Apps.ChatTerminal.Commons;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    public class MessageSystemModel
    {
        private const float PLAYER_TYPING_SPEED = 30f;

        private bool _isPaused = false;
        
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
                foreach (ChatMessage message in CurrentProfile.Messages[i])
                {
                    //If the message is a choice 
                    if (message.Sender == "CHOICE")
                    {
                        CreateChoiceMessage(message);

                        TogglePause(true);
                        
                        yield return new WaitUntil(() => !_isPaused);
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
                    yield return new WaitForSeconds(delayS);

                    CreateMessage(message);

                    ChatTerminalMvc.Instance.MessageSystemController.messageTyped?.Invoke(message.MessageID);
                    
                    //Small delay between each message
                    yield return new WaitForSeconds(0.5f);
                }
                CurrentProfile.SeenMessagesIndex++;
            }

            CurrentProfile.Status = MessageStatus.Offline;
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