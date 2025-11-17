using System;
using Apps.ChatTerminal.Commons;
using Apps.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;
using Story.Commons;
using Story.Controllers;
using Story.Views;

namespace Story.Models.States
{
    [Serializable]
    public class DefaultState : IState
    {
        public int State => (int)StatesEnum.Default;
        public int NextState => (int)StatesEnum.Default;
        
        public void OnEnter()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }

        public void OnExit()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }

        public void ChangeState()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }
    }
}