using Desktop.Views;
using Saving.Commons;
using Story.Commons;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private SceneAsset mainScene;
    [SerializeField] private SceneAsset registringScene;

    public static bool LoadedNewGame = false;
    
    private void Awake()
    {
        if (!SavingMvc.Instance.SavingController.LoadGame())
        {
            //New Game
            SceneManager.LoadScene(registringScene.name);
            LoadedNewGame = true;
            return;
        }
        
        //Loaded Game
        SceneManager.LoadScene(mainScene.name);
    }
}