using GameLoading.Commons;
using GameLoading.Controllers;
using UnityEngine;

namespace GameLoading.Views
{
    public class GameLoadView : MonoBehaviour
    {
        private GameLoadingController _gameLoadingController;
        
        private void Awake()
        {
            _gameLoadingController = LoadingMvc.Instance.GameLoadingController;

            _gameLoadingController.LoadGame();
        }
    }
}
