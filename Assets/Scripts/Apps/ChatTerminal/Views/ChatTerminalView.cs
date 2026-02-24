using System.Collections.Generic;
using System.Linq;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using Apps.Commons;
using Desktop.Commons;
using FourthWall.Commons;
using TMPro;
using UnityEngine;
using User.Commons;
using User.Models;

namespace Apps.ChatTerminal.Views
{
    public class ChatTerminalView : AppsCommon
    {
        [SerializeField] private TMP_Text usernameText;

        [Header("Contacts")]
        [SerializeField] private GameObject contactPrefab;
        [SerializeField] private Transform contactsParent;
        
        [Header("Messages Window")]
        [SerializeField] private GameObject messagesWindow;
        
        /// <summary>
        /// This is public because I am using it in the controller.
        /// Ideally, this should be in the model itself, unfortunately this is not serializable.
        /// </summary>
        private  List<ChatProfile> _profiles = new();
        public List<ChatProfile> Profiles
        {
            get
            {
                if (_profiles.Count == 0)
                {
                    UpdateContactData();
                }
                return _profiles;
            }
            set => _profiles = value;
        }
        
        private void Awake()
        {
            usernameText.text = UserMvc.Instance.UserController.Username;
            
            UpdateContactData();
        }

        /// <summary>
        /// Updates the contact list with loaded chat _profiles.
        /// </summary>
        public void UpdateContactData()
        {
            List<ChatProfileModel> profilesFromJson = ChatTerminalMvc.Instance.ChatTerminalController.GetChatProfiles();
            
            foreach (ChatProfileModel profile in profilesFromJson)
            {
                ChatProfile existingProfile = _profiles.FirstOrDefault(x => x.UserID == profile.UserID);
                
                if (!profile.IsLoaded)
                {
                    if (existingProfile != null)
                    {
                        Destroy(existingProfile.gameObject);
                    }
                    continue;
                }
                if (existingProfile != null)
                {
                    continue;
                }
                
                GameObject newContact = Instantiate(contactPrefab, contactsParent);
                
                var newProfile = newContact.AddComponent<ChatProfile>();
                newProfile.LoadData(profile);
                _profiles.Add(newProfile);
                
                newContact.GetComponent<ContactView>().SetProperties(messagesWindow, newProfile);
            }
        }

        /// <summary>
        /// Unloads a chat profile by destroying its game object and removing it from the list of loaded _profiles.
        /// </summary>
        /// <param name="profileId">ID of the profile</param>
        public void UnloadProfile(string profileId)
        {
            ChatProfile profileToUnload = _profiles.FirstOrDefault(profile => profile.UserID == profileId);
            if (profileToUnload == null)
            {
                return;
            }
            
            Destroy(profileToUnload.gameObject);
            _profiles.Remove(profileToUnload);
        }
        
        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }

        protected override void OnDisableChild()
        {
            messagesWindow.SetActive(false);
            
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }
    }
}