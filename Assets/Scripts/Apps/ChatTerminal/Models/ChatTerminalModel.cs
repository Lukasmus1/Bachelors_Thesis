using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
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
    }
}