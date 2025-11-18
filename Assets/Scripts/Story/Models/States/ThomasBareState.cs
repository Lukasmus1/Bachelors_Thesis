using System;
using Apps.ChatTerminal.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;

namespace Story.Models.States
{
    [Serializable]
    public class ThomasBareState : IState
    {
        public int State => (int)StatesEnum.ThomasBare;
        public int NextState => (int)StatesEnum.Default;
        
        public void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("thomasBare");
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("thomasBare", 0);
            
            NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewMessage);
        }

        public void OnExit()
        {
            
        }
        
        public void ChangeState()
        {
            
        }
    }
}