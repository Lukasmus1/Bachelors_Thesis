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
    public class DefaultStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.Default;
        public override int NextState { get; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }

        public override void OnExit()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }
    }
}