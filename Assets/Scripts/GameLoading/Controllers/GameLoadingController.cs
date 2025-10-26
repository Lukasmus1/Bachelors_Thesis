using Saving.Commons;

namespace GameLoading.Controllers
{
    public class GameLoadingController
    {
        
        public void LoadGame()
        {
            //Try to load a saved game
            SavingMvc.Instance.SavingController.LoadGame();
         
        }
        
    }
}