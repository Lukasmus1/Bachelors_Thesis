using System;

namespace Apps.ChatTerminal.Models
{
    [Serializable]
    public class ChatMessage
    {
        public string MessageID { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }
        
        public ChatMessage(string messageID, string sender, string text)
        {
            MessageID = messageID;
            Sender = sender;
            Text = text;
        }
    }
}