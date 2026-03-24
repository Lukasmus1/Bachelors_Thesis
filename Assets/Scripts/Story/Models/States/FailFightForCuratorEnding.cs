using System;

namespace Story.Models.States
{
    [Serializable]
    public class FailFightForCuratorEnding : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FailFightForCuratorEnding;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            throw new NotImplementedException("ENDING NOT IMPLEMENTED YET");
        }

        public override void OnExit()
        {
            throw new NotImplementedException("ENDING NOT IMPLEMENTED YET");
        }

        public override void LoadFromState()
        {
            throw new NotImplementedException("ENDING NOT IMPLEMENTED YET");
        }
    }
}