using System;
using Apps.ChatTerminal.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;
using Story.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class ThomasBareStateClass : StateClass
    {
        public override int State => (int)StatesEnum.ThomasBare;
        public override int NextState => (int)StatesEnum.MysteriousFile;
        
        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("thomasBare");
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("thomasBare", 0);
            
            NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewMessage);
            
            ChatTerminalMvc.Instance.MessageSystemController.openedContact += CheckForContactOpened;
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.openedContact -= CheckForContactOpened;
        }
        
        private void CheckForContactOpened(string profileId)
        {
            if (profileId == "thomasBare")
            {
                ChangeState();
            }
        }
    }
}