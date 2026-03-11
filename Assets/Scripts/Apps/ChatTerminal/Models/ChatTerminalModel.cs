using System;
using System.Collections.Generic;
using System.Linq;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Controllers;
using FourthWall.Commons;
using Newtonsoft.Json;
using UnityEngine;
using User.Commons;
using User.Models;

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
                UpdateProceduralData(profile.Messages);
                
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
                    UpdateProceduralData(secondaryChat.Messages);
                }
                
                _loadedChatProfiles.Add(profile);
            }
        }

        /// <summary>
        /// Replaces the tags in the chat messages with the corresponding procedural data for the user.
        /// </summary>
        /// <param name="messages">Messages to replace the tags in</param>
        private void UpdateProceduralData(List<List<ChatMessage>> messages)
        {
            foreach (ChatMessage t in messages.SelectMany(chatMessages => chatMessages))
            {
                UpdateProceduralData(t);
            }
        }
        
        /// <summary>
        /// Replaces the tags in the chat messages with the corresponding procedural data for the user.
        /// </summary>
        /// <param name="messageGroups">MessageGroup to replace the tags in</param>
        private void UpdateProceduralData(List<MessageGroup> messageGroups)
        {
            foreach (ChatMessage chatMessage in messageGroups.SelectMany(group => group.MessagesGroup))
            {
                UpdateProceduralData(chatMessage);
            }
        }
        
        /// <summary>
        /// Replaces the specific tags in the chat message with the corresponding procedural data for the user.
        /// </summary>
        /// <param name="chatMessage">ChatMessage to edit</param>
        private void UpdateProceduralData(ChatMessage chatMessage)
        {
            chatMessage.Text = chatMessage.Text.Replace("{lastFileLocation}",
                UserMvc.Instance.UserController.ProceduralData(UserDataType.LastFileLocation));
            
            chatMessage.Text = chatMessage.Text.Replace("{curatorLocation}",
                UserMvc.Instance.UserController.ProceduralData(UserDataType.CuratorLocation));
            
            chatMessage.Text = chatMessage.Text.Replace("{aiUploadURL}",
                UserMvc.Instance.UserController.ProceduralData(UserDataType.AiUploadUrl));
            
            chatMessage.Text = chatMessage.Text.Replace("{realName}",
                FourthWallMvc.Instance.UserInformationController.GetUserRealName());
            //More procedural data can be added here as needed
        }
        
        /// <summary>
        /// Gets the message group with the given ID from the secondary chat profile of the user with the given ID.
        /// </summary>
        /// <param name="userID">ID of the user</param>
        /// <param name="messageGroupID">ID of the message group</param>
        /// <returns>MessageGroup of given secondary message</returns>
        public MessageGroup GetSecondaryMessageGroup(string userID, string messageGroupID)
        {
            ChatProfileModel profile = _loadedChatProfiles.FirstOrDefault(profile => profile.UserID == userID);
            if (profile == null)
            {
                Debug.LogError($"Chat profile with ID {userID} not found.");
                return null;
            }
            
            MessageGroup secondaryMessageGroup = profile.SecondaryChatProfileMessages.FirstOrDefault(messageGroup => messageGroup.MessageGroupID == messageGroupID);
            if (secondaryMessageGroup == null)
            {
                Debug.LogError($"Message group with ID {messageGroupID} not found for user ID {userID}.");
                return null;
            }

            return secondaryMessageGroup;
        }

        /// <summary>
        /// Insert a new message group into the messages list of a primary chat profile at the correct index and increase the message index of the profile.
        /// </summary>
        /// <param name="userID">ID of the profile</param>
        /// <param name="messageGroupID">ID of the message group to display</param>
        /// <param name="loadedProfiles">Loaded profiles in the view class</param>
        /// <param name="immediatelyDisplay">Should the message be displayed immediately?</param>
        public void QueueSecondaryMessage(string userID, string messageGroupID, List<ChatProfile> loadedProfiles, bool immediatelyDisplay = true)
        {
            ChatProfile profile = loadedProfiles.FirstOrDefault(profile => profile.UserID == userID);
            if (profile == null)
            {
                Debug.LogError($"Chat profile with ID {userID} not found.");
                return;
            }

            ChatProfileModel profileModel = _loadedChatProfiles.FirstOrDefault(p => p.UserID == userID);
            if (profileModel == null)
            {
                //This really should never happen, but it keeps the IDE happy
                throw new NullReferenceException($"Chat profile with ID {userID} not found.");
            }

            MessageGroup secondaryMessageGroup = profileModel.SecondaryChatProfileMessages.FirstOrDefault(messageGroup => messageGroup.MessageGroupID == messageGroupID);
            if (secondaryMessageGroup == null)
            {
                Debug.LogError($"Message group with ID {messageGroupID} not found for user ID {userID}.");
                return;
            }
            
            //First increase the message index and then insert the new message group at the correct index in the primary chat profile's messages list
            profile.Messages.Insert(profile.CurrentMessageIndex + 1, secondaryMessageGroup.MessagesGroup);
            
            if (immediatelyDisplay)
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
            if (loadedProfiles.Count > 0 && profile)
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
            ChatProfileModel profile = LoadedChatProfilesFromJson.FirstOrDefault(x => x.UserID == profileId);
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
        public void UnloadProfile(string profileId)
        {
            ChatProfileModel profile = LoadedChatProfilesFromJson.FirstOrDefault(x => x.UserID == profileId);
            if (profile == null)
            {
                throw new NullReferenceException($"Profile with ID {profileId} not found.");
            }
            
            profile.IsLoaded = false;
        }

        /// <summary>
        /// Changes the username of a specific chat profile in both the JSON model and the loaded profile in the view.
        /// </summary>
        /// <param name="userID">ID of the user</param>
        /// <param name="newName">New username</param>
        /// <param name="loadedProfiles">Loaded profiles in the view</param>
        public void ChangeUsername(string userID, string newName, List<ChatProfile> loadedProfiles)
        {
            ChatProfileModel modelJson = LoadedChatProfilesFromJson.FirstOrDefault(x => x.UserID == userID);
            ChatProfile modelLoaded = loadedProfiles.FirstOrDefault(x => x.UserID == userID);
            
            if (modelJson == null || !modelLoaded)
            {
                Debug.LogError($"Chat profile with ID {userID} not found.");
                return;
            }
            
            modelJson.Username = newName;
            modelLoaded.Username = newName;
        }
    }
}