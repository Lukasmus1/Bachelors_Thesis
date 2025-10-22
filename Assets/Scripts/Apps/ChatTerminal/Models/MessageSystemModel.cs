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
            foreach (string message in profile.Messages[profile.CurrentMessageIndex])
            {
                //Simulate typing delay based on typing speed divided by number of words
                //splitting might not be perfect, but for this purpose it's sufficient
                var delayMs = (int)(message.Split(' ').Length / profile.TypingSpeed * 1000);
                await Task.Delay(delayMs);
                CreateMessage(message);
            }
    
            profile.CurrentMessageIndex++;
        }
        
        private void CreateMessage(string content)
        {
            
            ChatTerminalMvc.Instance.MessageSystemController.CreateMessage(content);
        }
    }
}