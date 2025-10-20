using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using Desktop.Commons;
using TMPro;
using UnityEngine;
using User.Commons;

namespace Apps.ChatTerminal.Views
{
    public class ChatTerminalView : MonoBehaviour
    {
        [SerializeField] private TMP_Text usernameText;

        [Header("Contacts")]
        [SerializeField] private GameObject contactPrefab;
        [SerializeField] private Transform contactsParent;
        
        private void Awake()
        {
            usernameText.text = UserMvc.Instance.DesktopGeneratorController.Username;
            
            List<ChatProfile> profiles = ChatTerminalMvc.Instance.ChatTerminalController.GetChatProfiles();
            SetContactData(profiles);
        }

        private void SetContactData(List<ChatProfile> profiles)
        {
            foreach (ChatProfile profile in profiles)
            {
                var props = Instantiate(contactPrefab, contactsParent).GetComponent<ContactProperties>();

                props.usernameText.text = profile.Username;
                props.statusText.text = profile.Status.ToUpper(); //ToUpper just in case
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