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
        
        [Header("Message Terminal Icon")]
        [SerializeField] private RawImage terminalIcon;
        [SerializeField] private Sprite newMessageIcon;
        [SerializeField] private Sprite defaultIcon;
        private readonly List<ChatProfile> _profiles = new();
        
        private void Awake()
        {
            usernameText.text = UserMvc.Instance.DesktopGeneratorController.Username;
            
            List<ChatProfileModel> profiles = ChatTerminalMvc.Instance.ChatTerminalController.GetChatProfiles();
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
                newProfile.MessageStatusChanged += UpdateIcon;
                
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

        private void OnDestroy()
        {
            foreach (ChatProfile profile in _profiles.Where(profile => profile != null))
            {
                profile.MessageStatusChanged -= UpdateIcon;
            }
        }

        private void UpdateIcon(MessageStatus status)
        {
            terminalIcon.texture = _profiles.Any(x => x.Status is MessageStatus.NewMessage or MessageStatus.Typing) ? newMessageIcon.texture : defaultIcon.texture;
        }
    }
}