using System;
using System.Collections.Generic;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    public class ChatTerminalModel
    {
        private List<ChatProfile> _loadedChatProfiles = new();
        public List<ChatProfile> LoadedChatProfiles
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
            
            var chatProfiles = JsonUtility.FromJson<ChatProfilesWrapper>(chatJson.text);
            foreach (ChatProfile profile in chatProfiles.ChatProfiles)
            {
                LoadedChatProfiles.Add(profile);
            }
        }
    }
}