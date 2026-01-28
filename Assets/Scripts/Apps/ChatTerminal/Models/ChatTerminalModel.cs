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
        public List<ChatProfileModel> LoadedChatProfilesFromJson
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
        
        /// <summary>
        /// Load chat profiles from a JSON file located in the Resources folder.
        /// </summary>
        /// <exception cref="Exception">Did not find the chat.json in resources folder</exception>
        private void LoadChatProfiles()
        {
            var chatJson = Resources.Load<TextAsset>("chat");

            Sprite[] icons = Resources.LoadAll<Sprite>("Prefabs/Apps/ChatTerminal/PersonIcons");
            
            if (chatJson == null)
            {
                throw new Exception("Chat JSON file not found in Resources folder.");
            }
            
            var chatProfiles = JsonConvert.DeserializeObject<ChatProfilesWrapper>(chatJson.text);
            foreach (ChatProfileModel profile in chatProfiles.ChatProfiles)
            {
                Sprite icon = icons.FirstOrDefault(sprite => sprite.name == profile.ProfilePicture);
                if (icon != null)
                {
                    profile.ProfilePictureBytes = icon.texture.EncodeToPNG();
                }
                _loadedChatProfiles.Add(profile);
            }
        }

        /// <summary>
        /// Set the current message index for a specific chat profile.
        /// </summary>
        /// <param name="profileId">ID of the profile.</param>
        /// <param name="messageIndex">Index of the message.</param>
        /// <param name="loadedProfiles">Already loaded profiles</param>
        public void SetChatProfileMessageIndex(string profileId, int messageIndex, List<ChatProfile> loadedProfiles)
        {
            if (loadedProfiles.Count > 0)
            {
                ChatProfile profile = loadedProfiles.Find(profile => profile.UserID == profileId);
                if (profile != null)
                {
                    profile.CurrentMessageIndex = messageIndex;
                }
                else
                {
                    Debug.LogError($"Chat profile with ID {profileId} not found.");
                }
            }
            else
            {
                ChatProfileModel profile = LoadedChatProfilesFromJson.Find(profile => profile.UserID == profileId);
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

        /// <summary>
        /// Set "loaded" flag to true for a specific chat profile.
        /// </summary>
        /// <param name="profileId"></param>
        public void LoadNewProfile(string profileId)
        {
            ChatProfileModel profile = _loadedChatProfiles.FirstOrDefault(x => x.UserID == profileId);
            if (profile == null)
            {
                throw new NullReferenceException($"Profile with ID {profileId} not found.");
            }
            
            profile.IsLoaded = true;
        }
    }
}