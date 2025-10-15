using Saving.Models;

namespace Saving.Controllers
{
    public class SaveController
    {
        private SaveLogic _modelLogic;
        
        public void LoadGame()
        {
            _modelLogic.LoadGame();
        }
    }
}