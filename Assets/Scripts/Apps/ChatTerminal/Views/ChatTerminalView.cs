using System;
using Desktop.Commons;
using Desktop.Models;
using UnityEngine;

namespace Apps.ChatTerminal.Views
{
    public class ChatTerminalView : MonoBehaviour
    {
        private void Awake()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }
        
        private void OnDestroy()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }
    }
}