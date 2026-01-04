using System;

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

        public override void LoadFromState()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }
    }
}