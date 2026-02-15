using System;
using System.Collections.Generic;

namespace Apps.ChatTerminal.Models
{
    [Serializable]
    public class SecondaryChatProfileModel
    {
        public string UserID;
        public List<MessageGroup> Messages;
    }

    [Serializable]
    public class MessageGroup
    {
        public string MessageGroupID { get; set; }
        public List<ChatMessage> MessagesGroup { get; set; }
    }

}