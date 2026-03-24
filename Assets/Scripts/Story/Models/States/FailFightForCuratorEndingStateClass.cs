using System;
using Commons;
using Desktop.Views;

namespace Story.Models.States
{
    [Serializable]
    public class FailFightForCuratorEndingStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FailFightForCuratorEnding;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }

        public override void LoadFromState()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }
    }
}