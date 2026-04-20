using System;
using Commons;
using Desktop.Views;
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
            StoryMvc.Instance.StoryController.SetEnding(Endings.FightForCuratorFail);
            
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