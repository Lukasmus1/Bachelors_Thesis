using System;
using Commons;
using Saving.Commons;
using Story.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class FailedFightForAIEndingStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FailedFightForAIEnding;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            StoryMvc.Instance.StoryController.SetEnding(Endings.FightForAIFail);
            
            var timer = new AsyncTimer();

            //Gives the player 10 seconds to read the final message.
            _ = timer.StartTimer(10, (() =>
            {
                SavingMvc.Instance.SavingController.DeleteProgressAndQuit();
                
                timer.Dispose();
            }));
        }

        //This is the final state, player doesn't leave it
        public override void OnExit()
        { }

        public override void LoadFromState()
        {
            OnEnter();
        }
    }
}