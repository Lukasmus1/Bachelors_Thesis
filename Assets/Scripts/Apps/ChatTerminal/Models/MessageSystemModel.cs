using System.Collections;
using System.Threading.Tasks;
using Apps.ChatTerminal.Commons;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    public class MessageSystemModel
    {
        public ChatProfile profile;
        
        public void StartMessaging()
        {
            _ = TypingAsync();
        }

        private async Task TypingAsync()
        {
            bool isIndexOverMessages = profile.CurrentMessageIndex >= profile.Messages.Count;
            
            //Create message history
            for (int i = 0; i < profile.CurrentMessageIndex; i++)
            {
                foreach (string oldMsg in profile.Messages[i])
                {
                    CreateMessage(oldMsg);
                }

                if (i == profile.CurrentMessageIndex - 1 && isIndexOverMessages)
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
            
            foreach (string message in profile.Messages[profile.CurrentMessageIndex])
            {
                //Simulate typing delay based on typing speed divided by number of chars
                var delayMs = (int)(message.Length / profile.TypingSpeed * 1000);
                await Task.Delay(delayMs);
                CreateMessage(message);
            }
    
            profile.CurrentMessageIndex++;
            
            profile.Status = MessageStatus.Offline;
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