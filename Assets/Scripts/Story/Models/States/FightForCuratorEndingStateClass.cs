using System;
using FourthWall.Commons;
using Saving.Commons;
using Story.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class FightForCuratorEndingStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.SuccessFightForCuratorEnding;
        public override int NextState { get; set; } = (int)StatesEnum.Default;
        
        public override void OnEnter()
        {
            StoryMvc.Instance.StoryController.SetEnding(Endings.FightForCuratorSuccess);
        }

        //Won't be called, this is the end state
        public override void OnExit()
        { }

        public override void LoadFromState()
        {
            FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "Delete this game!", "...");
            
            SavingMvc.Instance.SavingController.QuitAndSaveGame();
        }
    }
}