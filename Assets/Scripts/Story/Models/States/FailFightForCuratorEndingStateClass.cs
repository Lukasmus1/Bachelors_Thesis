using System;
using Saving.Commons;
using Story.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class FailFightForCuratorEndingStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FailFightForCuratorEnding;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            //The Ending is set in JumpscareView.cs
        }

        public override void OnExit()
        {
        }

        public override void LoadFromState()
        {
            SavingMvc.Instance.SavingController.QuitWithoutSaving();
        }
    }
}