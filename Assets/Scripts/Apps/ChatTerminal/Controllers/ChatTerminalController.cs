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
            foreach (ChatProfile chatProfile in _chatTerminalView.Profiles)
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
        /// Gets the MessageGroup object of a secondary message group.
        /// </summary>
        /// <param name="userID">ID of the user's secondary message</param>
        /// <param name="messageGroupID">ID of the secondary message group</param>
        /// <returns></returns>
        public MessageGroup GetSecondaryMessageGroup(string userID, string messageGroupID)
        {
            return chatTerminalModel.GetSecondaryMessageGroup(userID, messageGroupID);
        }
        
        /// <summary>
        /// Gets the concatenated text of all messages in a secondary message group, separated by new lines.
        /// </summary>
        /// <param name="userID">ID of the user's secondary message</param>
        /// <param name="messageGroupID">ID of the secondary message group</param>
        /// <returns></returns>
        public string GetSecondaryMessageGroupConcat(string userID, string messageGroupID)
        {
            var result = "";
            MessageGroup messages = GetSecondaryMessageGroup(userID, messageGroupID);

            foreach (ChatMessage msg in messages.MessagesGroup)
            {
                result += msg.Text + "\n";
            }
            
            return result;
        }
        
        /// <summary>
        /// Set the message index of a chat profile and instantiate a new message notification.
        /// </summary>
        /// <param name="profileId">ID of the profile</param>
        public void IncreaseChatProfileMessageIndex(string profileId)
        {
            chatTerminalModel.IncreaseChatProfileMessageIndex(profileId, _chatTerminalView.Profiles);
            NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewMessage);
        }

        /// <summary>
        /// Display a new secondary message group in the chat terminal and instantiate a new message notification.
        /// </summary>
        /// <param name="userID">ID of the user's messages</param>
        /// <param name="messageGroupID">ID of the message group</param>
        /// <param name="immediatelyDisplay">Should the message be display immediately?</param>
        public void QueueSecondaryMessage(string userID, string messageGroupID, bool immediatelyDisplay = true)
        {
            chatTerminalModel.QueueSecondaryMessage(userID, messageGroupID, _chatTerminalView.Profiles, immediatelyDisplay);
        }
        
        /// <summary>
        /// Loads a new chat profile into the chat terminal.
        /// </summary>
        /// <param name="profileId">ID of the profile to load</param>
        public void LoadNewProfile(string profileId)
        {
            chatTerminalModel.LoadNewProfile(profileId);
            NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewMessage);
            _chatTerminalView.UpdateContactData();
        }

        /// <summary>
        /// Unloads a chat profile from the chat terminal.
        /// </summary>
        /// <param name="profileId">ID of the profile to unload</param>
        public void UnloadProfile(string profileId)
        {
            chatTerminalModel.UnloadProfile(profileId);
            _chatTerminalView.UnloadProfile(profileId);
        }

        /// <summary>
        /// Changes the username of a specific chat profile in both the JSON model and the loaded profile in the view.
        /// </summary>
        /// <param name="userID">ID of the user</param>
        /// <param name="newName">New username</param>
        public void ChangeUsername(string userID, string newName)
        {
            chatTerminalModel.ChangeUsername(userID, newName, _chatTerminalView.Profiles);
        }
    }
}