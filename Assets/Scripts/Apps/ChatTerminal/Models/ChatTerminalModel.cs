using System;
using System.Collections.Generic;
using System.Linq;
using Apps.ChatTerminal.Commons;
using Newtonsoft.Json;
using UnityEngine;

namespace Apps.ChatTerminal.Models
{
    [Serializable]
    public class ChatTerminalModel
    {
        //Primary chat profiles
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
        /// Updates the profile data for a given chat profile (used for saving).
        /// </summary>
        /// <param name="chatProfile">Currently used profile of the person</param>
        public void UpdateProfileData(ChatProfile chatProfile)
        {
            foreach (ChatProfileModel profile in LoadedChatProfilesFromJson)
            {
                if (profile.UserID != chatProfile.UserID)
                {
                    continue;
                }
                
                profile.Status = chatProfile.Status;
                profile.CurrentMessageIndex = chatProfile.CurrentMessageIndex;
                profile.SeenMessagesIndex = chatProfile.SeenMessagesIndex;
                profile.IsLoaded = chatProfile.IsLoaded;
                break;
            }
        }
        
        /// <summary>
        /// Load chat profiles from a JSON file located in the Resources folder.
        /// </summary>
        /// <exception cref="Exception">Did not find the chat.json in resources folder</exception>
        private void LoadChatProfiles()
        {
            var chatJson = Resources.Load<TextAsset>("chat");
            var secondaryChatJson = Resources.Load<TextAsset>("secondaryChat");

            Sprite[] icons = Resources.LoadAll<Sprite>("Prefabs/Apps/ChatTerminal/PersonIcons");
            
            if (!chatJson || !secondaryChatJson)
            {
                throw new Exception("Chat or secondary chatJSON file not found in Resources folder.");
            }
            
            var chatProfiles = JsonConvert.DeserializeObject<ChatProfilesWrapper>(chatJson.text);
            var secondaryChatProfiles = JsonConvert.DeserializeObject<SecondaryChatProfilesWrapper>(secondaryChatJson.text);
            
            foreach (ChatProfileModel profile in chatProfiles.ChatProfiles)
            {
                Sprite icon = icons.FirstOrDefault(sprite => sprite.name == profile.ProfilePicture);
                if (icon)
                {
                    profile.ProfilePictureBytes = icon.texture.EncodeToPNG();
                }
                
                //Link secondary chat messages to primary chat profiles
                SecondaryChatProfileModel secondaryChat = secondaryChatProfiles.ChatProfiles.FirstOrDefault(x => x.UserID == profile.UserID);
                if (secondaryChat == null)
                {
                    Debug.LogWarning($"No secondary chat profile found for user ID {profile.UserID}");
                }
                else
                {
                    profile.SecondaryChatProfileMessages = secondaryChat.Messages;   
                }
                
                _loadedChatProfiles.Add(profile);
            }
        }

        /// <summary>
        /// Insert a new message group into the messages list of a primary chat profile at the correct index and increase the message index of the profile.
        /// </summary>
        /// <param name="userID">ID of the profile</param>
        /// <param name="messageGroupID">ID of the message group to display</param>
        public void QueueSecondaryMessage(string userID, string messageGroupID)
        {
            ChatProfileModel profile = _loadedChatProfiles.FirstOrDefault(profile => profile.UserID == userID);
            if (profile == null)
            {
                Debug.LogError($"Chat profile with ID {userID} not found.");
                return;
            }
            
            MessageGroup secondaryMessageGroup = profile.SecondaryChatProfileMessages.FirstOrDefault(messageGroup => messageGroup.MessageGroupID == messageGroupID);
            if (secondaryMessageGroup == null)
            {
                Debug.LogError($"Message group with ID {messageGroupID} not found for user ID {userID}.");
                return;
            }
            
            //First increase the message index and then insert the new message group at the correct index in the primary chat profile's messages list
            profile.Messages.Insert(profile.CurrentMessageIndex, secondaryMessageGroup.MessagesGroup);
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex(profile.UserID);
        }
        
        /// <summary>
        /// Set the current message index for a specific chat profile.
        /// </summary>
        /// <param name="profileId">ID of the profile.</param>
        /// <param name="loadedProfiles">Already loaded profiles</param>
        public void IncreaseChatProfileMessageIndex(string profileId, List<ChatProfile> loadedProfiles)
        {
            ChatProfile profile = loadedProfiles.Find(profile => profile.UserID == profileId);
            if (loadedProfiles.Count > 0 && profile != null)
            {
                profile.CurrentMessageIndex++;
            }
            else
            {
                ChatProfileModel profileNotLoaded = LoadedChatProfilesFromJson.Find(p => p.UserID == profileId);
                if (profileNotLoaded != null)
                {
                    profileNotLoaded.CurrentMessageIndex++;
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
        /// <param name="profileId">ID of the profile to load</param>
        public void LoadNewProfile(string profileId)
        {
            ChatProfileModel profile = _loadedChatProfiles.FirstOrDefault(x => x.UserID == profileId);
            if (profile == null)
            {
                throw new NullReferenceException($"Profile with ID {profileId} not found.");
            }
            
            profile.IsLoaded = true;
        }

        /// <summary>
        /// Set "loaded" flag to false for a specific chat profile.
        /// </summary>
        /// <param name="profileId">ID of the profile to unload</param>
        public void UnloadNewProfile(string profileId)
        {
            ChatProfileModel profile = _loadedChatProfiles.FirstOrDefault(x => x.UserID == profileId);
            if (profile == null)
            {
                throw new NullReferenceException($"Profile with ID {profileId} not found.");
            }
            
            profile.IsLoaded = false;
        }
    }
}