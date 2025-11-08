using System;
using System.Collections;
using System.Threading;
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
        
        private CancellationTokenSource _token;
        
        public void StartMessaging()
        {
            _token = new CancellationTokenSource();
            _ = TypingAsync(_token.Token);
        }

        public void StopMessaging()
        {
            if (_token == null)
            {
                return;
            }
            
            _token?.Cancel();
            CurrentProfile.Status = MessageStatus.Offline;
        }
        
        private async Task TypingAsync(CancellationToken token)
        {
            bool isIndexOverMessages = CurrentProfile.CurrentMessageIndex >= CurrentProfile.Messages.Count;
            
            //Save start index in case of cancellation
            int startIndex = CurrentProfile.SeenMessagesIndex;
            
            //Try to type new messages
            //If cancelled, revert seen index to start index
            try
            {
                //Create message history
                for (int i = 0; i < CurrentProfile.SeenMessagesIndex; i++)
                {
                    foreach (ChatMessage oldMsg in CurrentProfile.Messages[i])
                    {
                        token.ThrowIfCancellationRequested();
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
                    return;
                }

                CurrentProfile.Status = MessageStatus.Typing;

                for (int i = CurrentProfile.SeenMessagesIndex; i <= CurrentProfile.CurrentMessageIndex; i++)
                {
                    foreach (ChatMessage message in CurrentProfile.Messages[i])
                    {
                        //Simulate typing delay based on typing speed divided by number of chars
                        var delayMs = (int)(message.Text.Length / CurrentProfile.TypingSpeed * 1000);
                        await Task.Delay(delayMs, token);

                        CreateMessage(message);
                    }

                    CreateDivider();
                    CurrentProfile.SeenMessagesIndex++;
                }

                CurrentProfile.Status = MessageStatus.Offline;
            }
            catch (OperationCanceledException)
            {
                CurrentProfile.Status = MessageStatus.Offline;
                CurrentProfile.SeenMessagesIndex = startIndex;
            }
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