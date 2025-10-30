using System;
using Desktop.Commons;
using Desktop.Views;
using Saving.Commons;
using Story.Commons;
using UnityEngine;

namespace Desktop
{
    public class Bootstrapper : MonoBehaviour
    {
        private DesktopGeneratorView _desktopGeneratorView;
        private StoryMvc _storyMvc;
        
        private void Start()
        {
            _desktopGeneratorView = GetComponentInChildren<DesktopGeneratorView>();
            _storyMvc = StoryMvc.Instance;
            
            if (!SavingMvc.Instance.SavingController.LoadGame())
            {
                //New Game
                _desktopGeneratorView.GenerateRandomDesktop();
                _storyMvc.StoryController.InitNew();
            }
        }

        private void OnApplicationQuit()
        {
            _desktopGeneratorView.SaveExistingIcons();
            SavingMvc.Instance.SavingController.SaveGame();
        }
    }
}