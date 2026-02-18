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
        public readonly List<ChatProfile> profiles = new();
        
        private void Awake()
        {
            usernameText.text = UserMvc.Instance.UserController.Username;
            
            UpdateContactData();
        }

        /// <summary>
        /// Updates the contact list with loaded chat profiles.
        /// </summary>
        public void UpdateContactData()
        {
            List<ChatProfileModel> profilesFromJson = ChatTerminalMvc.Instance.ChatTerminalController.GetChatProfiles();
            
            foreach (ChatProfileModel profile in profilesFromJson)
            {
                if (!profile.IsLoaded)
                {
                    if (profiles.Any(x => x.UserID == profile.UserID))
                    {
                        Destroy(profiles.First(x => x.UserID == profile.UserID).gameObject);
                    }
                    continue;
                }
                GameObject newContact = Instantiate(contactPrefab, contactsParent);
                
                var newProfile = newContact.AddComponent<ChatProfile>();
                newProfile.LoadData(profile);
                profiles.Add(newProfile);
                
                newContact.GetComponent<ContactView>().SetProperties(messagesWindow, newProfile);
            }
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