using System;
using Desktop.Commons;
using Desktop.Views;
using Saving.Commons;
using UnityEngine;

namespace Desktop
{
    public class Bootstrapper : MonoBehaviour
    {
        private DesktopGeneratorView _desktopGeneratorView;
        private void Start()
        {
            _desktopGeneratorView = GetComponentInChildren<DesktopGeneratorView>();
            
            if (!SavingMvc.Instance.SavingController.LoadGame())
            {
                //New Game
                _desktopGeneratorView.GenerateRandomDesktop();
            }
        }

        private void OnApplicationQuit()
        {
            SavingMvc.Instance.SavingController.SaveGame();
        }
    }
}