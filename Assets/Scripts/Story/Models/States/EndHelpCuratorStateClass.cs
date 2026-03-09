using System;

namespace Story.Models.States
{
    [Serializable]
    public class EndHelpCuratorStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.EndingFightForCurator;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            //AI will use different processes to hide in, the player must turn off using task manager
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