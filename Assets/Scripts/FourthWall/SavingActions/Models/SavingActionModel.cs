using System;
using FourthWall.Commons;
using FourthWall.FileGeneration.Models;
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
                case Endings.PlayerHelpsAI:
                    FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "You already helped me once, can you please do it again?", "Thank you");
                    break;
                case Endings.AIWins:
                    FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "I already won once, I will win again", "...");
                    break;
                case Endings.AILoses:
                    FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "Why did you download this again...", "Quit now");
                    break;
                default:
                    throw new Exception("Unknown ending: " + oldEnding);
            }
        }
    }
}