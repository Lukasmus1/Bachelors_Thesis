using System;
using Commons;
using Saving.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class FirstChoiceStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FirstChoice;
        public override int NextState { get; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            AsyncTimer t = new();

            _ = t.StartTimer(5, () =>
            {
                SavingMvc.Instance.SavingController.QuitAndSaveGame();
            });
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