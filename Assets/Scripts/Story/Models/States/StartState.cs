using System;
using Apps.ChatTerminal.Commons;
using Apps.Commons;
using Story.Views;

namespace Story.Models.States
{
    [Serializable]
    public class StartState : IState
    {
        public int State => (int)StatesEnum.Start;
        
        public void OnEnter()
        {
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
            }
        }
    }
}