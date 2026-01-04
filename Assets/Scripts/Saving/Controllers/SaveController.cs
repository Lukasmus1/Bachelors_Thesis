using System;
using Saving.Models;
using UnityEditor;

namespace Saving.Controllers
{
    public class SaveController
    {
        private readonly SaveLogic _modelLogic = new();

        public event Action OnGameLoaded;
        
        /// <summary>
        /// Tries to load a saved game.
        /// </summary>
        /// <returns>Returns true if successfully loaded and false if not.</returns>
        public bool LoadGame()
        {
            if (!_modelLogic.LoadGame())
            {
                return false;
            }
            
            OnGameLoaded?.Invoke();
            return true;
        }

        /// <summary>
        /// Saves the current game state.
        /// </summary>
        public void SaveGame()
        {
            _modelLogic.SaveGame();
        }
        
        /// <summary>
        /// Quits the game, saving first.
        /// </summary>
        public void QuitAndSaveGame()
        {
            SaveGame();
            
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}