using Apps.ChatTerminal.Commons;

namespace Apps.ChatTerminal.Models
{
    public class MessageSystemModel
    {
        public ChatProfile profile;

        public void StartMessaging()
        {
            
        }
        
        private void CreateMessage(string content)
        {
            
            ChatTerminalMvc.Instance.MessageSystemController.CreateMessage(content);
        }
    }
}