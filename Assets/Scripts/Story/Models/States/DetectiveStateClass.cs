using System;

namespace Story.Models.States
{
    [Serializable]
    public class DetectiveStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.Detective;
        public override int NextState { get; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public override void LoadFromState()
        {
            
        }
    }
}