using System.Collections.Generic;
using System.Linq;
using Apps.ChatTerminal.Models;
using Apps.ChatTerminal.Views;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;

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
        
        public void SaveGameData()
        {
            foreach (ChatProfile chatProfile in _chatTerminalView.profiles)
            {
                chatTerminalModel.UpdateProfileData(chatProfile);
            }
        }
        
        public List<ChatProfileModel> GetChatProfiles()
        {
            return chatTerminalModel.LoadedChatProfilesFromJson;
        }

        public ChatProfileModel GetChatProfile(string profileId)
        {
            return chatTerminalModel.LoadedChatProfilesFromJson.FirstOrDefault(profile => profile.UserID == profileId);
        }

        /// <summary>
        /// Set the message index of a chat profile and instantiate a new message notification.
        /// </summary>
        /// <param name="profileId">ID of the profile</param>
        /// <param name="index">Index to set the messages to</param>
        public void SetChatProfileMessageIndex(string profileId, int index)
        {
            chatTerminalModel.SetChatProfileMessageIndex(profileId, index, _chatTerminalView.profiles);
            NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewMessage);
        }

        /// <summary>
        /// Loads a new chat profile into the chat terminal.
        /// </summary>
        /// <param name="profileId">ID of the profile to load</param>
        public void LoadNewProfile(string profileId)
        {
            chatTerminalModel.LoadNewProfile(profileId);
            _chatTerminalView.UpdateContactData();
        }

        /// <summary>
        /// Unloads a new chat profile into the chat terminal.
        /// </summary>
        /// <param name="profileId">ID of the profile to unload</param>
        public void UnloadProfile(string profileId)
        {
            chatTerminalModel.UnloadNewProfile(profileId);
            _chatTerminalView.UpdateContactData();
        }
    }
}