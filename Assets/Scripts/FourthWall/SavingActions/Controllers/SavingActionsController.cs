using FourthWall.SavingActions.Models;
using Saving.Commons;
using Story.Models;

namespace FourthWall.SavingActions.Controllers
{
    public class SavingActionsController
    {
        private readonly SavingActionModel _savingActionModel = new();

        /// <summary>
        /// Performs the action based on the old ending when a new save is found.
        /// </summary>
        public void PerformActionOnFindingNewSave()
        {
            Endings prevEnd = SavingMvc.Instance.SavingController.GetOldEnding();

            _savingActionModel.PerformActionOnOldEnding(prevEnd);
        }
    }
}