using System;
using Apps.ChatTerminal.Commons;
using Apps.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;
using Story.Views;

namespace Story.Models.States
{
    [Serializable]
    public class StartState : IState
    {
        public int State => (int)StatesEnum.Start;
        public int NextState => (int)StatesEnum.Default;

        public event Action ChangeState;
        
        public void OnEnter()
        {
            NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewMessage);
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("ind1", 1);
            AppCommonsModel.Instance.AppOpened += CheckForStateChange;
        }

        public void OnExit()
        {
            AppCommonsModel.Instance.AppOpened -= CheckForStateChange;
        }

        private void CheckForStateChange(string appName)
        {
            if (appName == "FileViewer")
            {
                StoryManager.Instance.ChangeState();
                ChangeState?.Invoke();
            }
        }
    }
}