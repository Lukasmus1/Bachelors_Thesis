using System.Collections.Generic;
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
                newContact.AddComponent<ChatProfile>().LoadData(profile);
                newContact.GetComponent<ContactView>().SetProperties(messagesWindow);
            }
        }
        
        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }
        
        private void OnDisable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }
    }
}