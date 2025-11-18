using System;
using Apps.ChatTerminal.Commons;
using Apps.FileManager.Commons;
using Apps.FileViewer.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;
using Story.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class MouseQuestState : IState
    {
        public int State => (int)StatesEnum.MouseQuest;
        public int NextState => (int)StatesEnum.ThomasBare;
        
        public void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("headOfDpt");
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("headOfDpt", 0);
            
            NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewMessage);
            
            FileLoaderMvc.Instance.FileLoaderController.SetLoadedFileFlag("MouseQuest", true);
            
            FileViewerMvc.Instance.FileLoaderController.metadataOpened += CheckForMetadataOpened;
        }

        public void OnExit()
        {
            FileViewerMvc.Instance.FileLoaderController.metadataOpened -= CheckForMetadataOpened;
        }
        
        public void ChangeState()
        {
            StoryMvc.Instance.StoryController.CurrentState = StateFactory.GetState(NextState);
        }

        private void CheckForMetadataOpened(string fileName)
        {
            if (fileName == "Mouse Quest Beta")
            {
                ChangeState();
            }
        }
    }
}