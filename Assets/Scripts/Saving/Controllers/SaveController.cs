using Saving.Models;

namespace Saving.Controllers
{
    public class SaveController
    {
        private readonly SaveLogic _modelLogic = new();
        
        /// <summary>
        /// Tries to load a saved game.
        /// </summary>
        /// <returns>Returns true if successfully loaded and false if not.</returns>
        public bool LoadGame()
        {
            return _modelLogic.LoadGame();
        }

        /// <summary>
        /// Saves the current game state.
        /// </summary>
        public void SaveGame()
        {
            _modelLogic.SaveGame();
        }
    }
}