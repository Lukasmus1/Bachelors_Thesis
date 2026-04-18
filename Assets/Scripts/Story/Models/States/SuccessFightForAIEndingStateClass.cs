using System;
using Apps.ChatTerminal.Commons;
using FourthWall.Commons;
using Saving.Commons;
using Story.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class SuccessFightForAIEndingStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.SuccessFightForAIEnding;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("itDept", "itThankingPlayerEnding", true);
            
            StoryMvc.Instance.StoryController.SetEnding(Endings.FightForAISuccess);
        }

        //This is the final state, player doesn't leave it
        public override void OnExit()
        {}

        public override void LoadFromState()
        {
            FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "You already helped us enough, thank you! <3", "Thank you!");
            SavingMvc.Instance.SavingController.QuitWithoutSaving();
        }
    }
}