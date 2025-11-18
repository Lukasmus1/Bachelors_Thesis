using System.Collections.Generic;
using System.Linq;
using Apps.ChatTerminal.Models;
using Apps.ChatTerminal.Views;

namespace Apps.ChatTerminal.Controllers
{
    public class ChatTerminalController
    {
        public ChatTerminalModel chatTerminalModel = new();
        
        private ChatTerminalView _chatTerminalView;
        public void SetChatTerminalView(ChatTerminalView chatTerminalView)
        {
            _chatTerminalView = chatTerminalView;
        }
        
        
        public List<ChatProfileModel> GetChatProfiles()
        {
            return chatTerminalModel.LoadedChatProfilesFromJson;
        }

        public ChatProfileModel GetChatProfile(string profileId)
        {
            return chatTerminalModel.LoadedChatProfilesFromJson.FirstOrDefault(profile => profile.UserID == profileId);
        }

        public void SetChatProfileMessageIndex(string profileId, int index)
        {
            chatTerminalModel.SetChatProfileMessageIndex(profileId, index, _chatTerminalView.profiles);
        }

        public void LoadNewProfile(string profileId)
        {
            chatTerminalModel.LoadNewProfile(profileId);
            _chatTerminalView.UpdateContactData();
        }
    }
}