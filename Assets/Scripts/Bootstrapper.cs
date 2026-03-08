using FourthWall.Commons;
using Saving.Commons;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private string mainScene;
    [SerializeField] private string registringScene;
    
    public static bool LoadedNewGame = false;
    
    private void Awake()
    {
        if (!SavingMvc.Instance.SavingController.LoadGame())
        {
            //New Game
            FourthWallMvc.Instance.SavingActionsController.PerformActionOnFindingNewSave();
            SceneManager.LoadScene(registringScene);
            LoadedNewGame = true;
            return;
        }
        
        //Loaded Game
        SceneManager.LoadScene(mainScene);
    }
}