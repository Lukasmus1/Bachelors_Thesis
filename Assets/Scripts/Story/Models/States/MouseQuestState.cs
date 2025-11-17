using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;
using Story.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class MouseQuestState : IState
    {
        public int State => (int)StatesEnum.Start;
        public int NextState => (int)StatesEnum.Default;
        
        public void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("headOfDpt");
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("headOfDpt", 0);
            
            NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewMessage);
            
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("MouseQuest", true);
        }

        public void OnExit()
        {

        }
        
        public void ChangeState()
        {
            StoryMvc.Instance.StoryController.CurrentState = StateFactory.GetState(NextState);
        }
    }
}