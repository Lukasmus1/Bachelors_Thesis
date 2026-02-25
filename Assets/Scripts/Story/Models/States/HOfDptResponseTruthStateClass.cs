using System;

namespace Story.Models.States
{
    [Serializable]
    public class HOfDptResponseTruthStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.HOfDptResponseTruth;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

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