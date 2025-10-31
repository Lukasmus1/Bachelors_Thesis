using System;
using System.Collections.Generic;
using System.Linq;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using Desktop.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User.Commons;

namespace Apps.ChatTerminal.Views
{
    public class ChatTerminalView : MonoBehaviour
    {
        [SerializeField] private TMP_Text usernameText;

        [Header("Contacts")]
        [SerializeField] private GameObject contactPrefab;
        [SerializeField] private Transform contactsParent;
        
        [Header("Messages Window")]
        [SerializeField] private GameObject messagesWindow;
        
        private readonly List<ChatProfile> _profiles = new();
        
        private void Awake()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatTerminalView(this);
            usernameText.text = UserMvc.Instance.DesktopGeneratorController.Username;
            
            var profiles = ChatTerminalMvc.Instance.ChatTerminalController.GetChatProfiles();
            SetContactData(profiles);
        }

        private void SetContactData(List<ChatProfileModel> profiles)
        {
            foreach (ChatProfileModel profile in profiles)
            {
                if (!profile.IsLoaded)
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
        
        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }
        
        private void OnDisable()
        {
            messagesWindow.SetActive(false);
            
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }

    }
}