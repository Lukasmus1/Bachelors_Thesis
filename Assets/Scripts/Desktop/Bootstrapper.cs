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
            if (SavingMvc.Instance.SavingController.LoadGame())
            {
                //GameLoaded
                
            }
            else
            {
                //New Game
                _desktopGeneratorView = GetComponentInChildren<DesktopGeneratorView>();
                
                _desktopGeneratorView.GenerateRandomDesktop();
            }
        }

        private void OnApplicationQuit()
        {
            SavingMvc.Instance.SavingController.SaveGame();
        }
    }
}