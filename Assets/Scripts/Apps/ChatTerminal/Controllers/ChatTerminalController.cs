using System.Collections.Generic;
using Apps.ChatTerminal.Models;
using NUnit.Framework;

namespace Apps.ChatTerminal.Controllers
{
    public class ChatTerminalController
    {
        private readonly ChatTerminalModel _chatTerminalModel = new();
        
        public List<ChatProfile> GetChatProfiles()
        {
            return _chatTerminalModel.LoadedChatProfiles;
        }
    }
}