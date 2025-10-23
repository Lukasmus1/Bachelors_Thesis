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
        public ChatProfile profile;
        
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
            profile.Status = MessageStatus.Offline;
        }
        
        private async Task TypingAsync(CancellationToken token)
        {
            bool isIndexOverMessages = profile.CurrentMessageIndex >= profile.Messages.Count;
            
            //Save start index in case of cancellation
            int startIndex = profile.SeenMessagesIndex;
            
            //Try to type new messages
            //If cancelled, revert seen index to start index
            try
            {
                //Create message history
                for (int i = 0; i < profile.SeenMessagesIndex; i++)
                {
                    foreach (string oldMsg in profile.Messages[i])
                    {
                        token.ThrowIfCancellationRequested();
                        CreateMessage(oldMsg);
                    }

                    if (i == profile.SeenMessagesIndex - 1 && isIndexOverMessages)
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
                
                profile.Status = MessageStatus.Typing;
                
                for (int i = profile.SeenMessagesIndex; i <= profile.CurrentMessageIndex; i++)
                {
                    foreach (string message in profile.Messages[i])
                    {
                        //Simulate typing delay based on typing speed divided by number of chars
                        var delayMs = (int)(message.Length / profile.TypingSpeed * 1000);
                        await Task.Delay(delayMs, token);

                        CreateMessage(message);
                    }

                    CreateDivider();
                    profile.SeenMessagesIndex++;
                }
                
                profile.Status = MessageStatus.Offline;
            }
            catch (OperationCanceledException)
            {
                profile.Status = MessageStatus.Offline;
                profile.SeenMessagesIndex = startIndex;
            }
        }
        
        private static void CreateMessage(string content)
        {
            ChatTerminalMvc.Instance.MessageSystemController.CreateMessage(content);
        }

        private static void CreateDivider()
        {
            ChatTerminalMvc.Instance.MessageSystemController.CreateDivider();
        }
    }
}