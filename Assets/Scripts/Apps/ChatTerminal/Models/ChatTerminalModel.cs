using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    [Serializable]
    public class ChatTerminalModel
    {
        private List<ChatProfileModel> _loadedChatProfiles = new();
        public List<ChatProfileModel> LoadedChatProfiles
        {
            get
            {
                if (_loadedChatProfiles.Count == 0)
                {
                    LoadChatProfiles();
                }
                return _loadedChatProfiles;
            }
            set => _loadedChatProfiles = value;
        }
        
        private void LoadChatProfiles()
        {
            var chatJson = Resources.Load<TextAsset>("chat");

            if (chatJson == null)
            {
                throw new Exception("Chat JSON file not found in Resources folder.");
            }
            
            var chatProfiles = JsonConvert.DeserializeObject<ChatProfilesWrapper>(chatJson.text);
            foreach (ChatProfileModel profile in chatProfiles.ChatProfiles)
            {
                _loadedChatProfiles.Add(profile);
            }
        }

        public void SetChatProfileMessageIndex(string profileId, int messageIndex)
        {
            ChatProfileModel profile = LoadedChatProfiles.Find(profile => profile.UserID == profileId);
            if (profile != null)
            {
                profile.CurrentMessageIndex = messageIndex;
            }
            else
            {
                Debug.LogError($"Chat profile with ID {profileId} not found.");
            }
        }
    }
}