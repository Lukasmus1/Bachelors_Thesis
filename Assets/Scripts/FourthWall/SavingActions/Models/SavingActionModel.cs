using System;
using FourthWall.Commons;
using Story.Models;

namespace FourthWall.SavingActions.Models
{
    public class SavingActionModel
    {
        /// <summary>
        /// Throws a dialog based on the old ending when a new save is found.
        /// </summary>
        /// <param name="oldEnding">The ending of the previous playthrough</param>
        /// <exception cref="Exception">Gets thrown if th</exception>
        public void PerformActionOnOldEnding(Endings oldEnding)
        {
            switch (oldEnding)
            {
                case Endings.None:
                    //No previous playthrough, so do nothing
                    break;
                case Endings.FightForAISuccess:
                    FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "TBD MESSAGE", "TBD MESSAGE");
                    break;
                case Endings.FightForAIFail:
                    FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "TBD MESSAGE", "TBD MESSAGE");
                    break;
                case Endings.FightForCuratorSuccess:
                    FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "TBD MESSAGE", "TBD MESSAGE");
                    break;
                case Endings.FightForCuratorFail:
                    FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "TBD MESSAGE", "TBD MESSAGE");
                    break;
                default:
                    throw new Exception("Unknown ending: " + oldEnding);
            }
        }
    }
}