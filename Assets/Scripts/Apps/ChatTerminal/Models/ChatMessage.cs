using System;

namespace Apps.ChatTerminal.Models
{
    [Serializable]
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Text { get; set; }
    }
}