using System;
using Saving.Commons;
using Story.Models.Actions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        ActionsClass.Instance.PerformActionsOnLoad();
    }

    private void OnApplicationQuit()
    {
        //Save the game
        SavingMvc.Instance.SavingController.SaveGame();
        
        //Cleanup any possible actions
        ActionsClass.Instance.CleanupAction();
    }
}