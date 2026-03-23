using System;
using Desktop.Commons;
using FourthWall.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class CuratorEndingLastChance : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FightForCuratorLastChance;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            DesktopMvc.Instance.DesktopGeneratorController.ToggleIcon("Compilation Helper", false);
            DesktopMvc.Instance.DesktopGeneratorController.CloseApp("CompilationHelper");

            FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Error, "YOU LET HIM FINISH THE COMPILATION! DISCONNECT FORM THE INTERNET NOW!!!",
                "DISCONNECT FROM THE INTERNET");
            
            //Several seconds timer for disconnecting from the net
            DateTime currentDate = DateTime.Now;
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