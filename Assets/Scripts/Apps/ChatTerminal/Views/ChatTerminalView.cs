using System;
using Desktop.Commons;
using Desktop.Models;
using TMPro;
using UnityEngine;
using User.Commons;

namespace Apps.ChatTerminal.Views
{
    public class ChatTerminalView : MonoBehaviour
    {
        [SerializeField] private TMP_Text usernameText;
        
        private void Awake()
        {
            usernameText.text = UserMvc.Instance.DesktopGeneratorController.Username;
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