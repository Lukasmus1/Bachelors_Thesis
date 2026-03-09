using System;
using Apps.Commons;
using Apps.CompilationHelper.Commons;
using Apps.CompilationHelper.Controllers;
using Desktop.Commons;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.CompilationHelper.Views
{
    public class CompilationHelperView : AppsCommon
    {
        [SerializeField] private Slider progressBarSlider;
        
        [SerializeField] private GameObject kpFileMovingUI;
        [SerializeField] private Slider deletionProgressBar;

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }

        private void OnDestroy()
        {
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds -= UpdateProgressBar;
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds -= UpdateDeletionProgressBar;
        }

        /// <summary>
        /// Sets up the compilation progress bar.
        /// </summary>
        /// <param name="compilationTimeSeconds">Time for finishing compilation</param>
        public void SetupProgressBar(int compilationTimeSeconds)
        {
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds += UpdateProgressBar;
            
            progressBarSlider.maxValue = compilationTimeSeconds;
            progressBarSlider.value = 0;
        }

        /// <summary>
        /// Sets up the deletion progress bar.
        /// </summary>
        /// <param name="deletionTimeSeconds">Time for deletion of K-P</param>
        public void SetupDeletionProgressBar(int deletionTimeSeconds)
        {
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds += UpdateDeletionProgressBar;
            
            deletionProgressBar.maxValue = deletionTimeSeconds;
            deletionProgressBar.value = 0;
        }
        
        /// <summary>
        /// Updates compilation progress bar with the given seconds.
        /// </summary>
        /// <param name="seconds">Value to update the progress bar with</param>
        private void UpdateProgressBar(int seconds)
        {
            progressBarSlider.value = seconds;    
        }
        
        /// <summary>
        /// Updates deletion progress bar with the given seconds.
        /// </summary>
        private void UpdateDeletionProgressBar(int _)
        {
            deletionProgressBar.value++;    
        }

        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }

        /// <inheritdoc cref="CompilationHelperController.EnableKpCompilationFileMoving"/>
        public void EnableKpCompilationFileMoving()
        {
            kpFileMovingUI.SetActive(true);
        }
    }
}