using System.Collections.Generic;
using System.Linq;
using Apps.ChatTerminal.Models;
using NUnit.Framework;

namespace Apps.ChatTerminal.Controllers
{
    public class ChatTerminalController
    {
        private readonly ChatTerminalModel _chatTerminalModel = new();
        
        public List<ChatProfileModel> GetChatProfiles()
        {
            return _chatTerminalModel.LoadedChatProfiles;
        }

        public ChatProfileModel GetChatProfile(string profileId)
        {
            return _chatTerminalModel.LoadedChatProfiles.FirstOrDefault(profile => profile.UserID == profileId);
        }

        public void SetChatProfileMessageIndex(string profileId, int index)
        {
            _chatTerminalModel.SetChatProfileMessageIndex(profileId, index);
        }
    }
}