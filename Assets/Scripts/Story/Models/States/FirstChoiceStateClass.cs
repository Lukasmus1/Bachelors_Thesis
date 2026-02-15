using System;

namespace Story.Models.States
{
    [Serializable]
    public class FirstChoiceStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FirstChoice;
        public override int NextState { get; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        public override void LoadFromState()
        {
            throw new NotImplementedException();
        }
    }
}