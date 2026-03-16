using System;
using System.Collections;
using Apps.Commons;
using Apps.CompilationHelper.Commons;
using Apps.CompilationHelper.Controllers;
using Desktop.Commons;
using FourthWall.Commons;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.CompilationHelper.Views
{
    public class CompilationHelperView : AppsCommon
    {
        [SerializeField] private GameObject forAILayout;
        [SerializeField] private GameObject againstAILayout;
        
        [SerializeField] private Slider progressBarSlider;
        
        [SerializeField] private GameObject kpFileMovingUI;
        [SerializeField] private Slider deletionProgressBar;
        [SerializeField] private Button moveKpButton;
        [SerializeField] private Slider moveCooldownSlider;

        private void Start()
        {
            CompilationHelperMvc.Instance.CompilationHelperController.onCompilationFailed += Cleanup;
            CompilationHelperMvc.Instance.CompilationHelperController.onCompilationFinished += Cleanup;
        }

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds -= UpdateProgressBar;
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds -= UpdateDeletionProgressBar;
            CompilationHelperMvc.Instance.CompilationHelperController.onCompilationFailed -= Cleanup;
            CompilationHelperMvc.Instance.CompilationHelperController.onCompilationFinished -= Cleanup;
            
            moveKpButton.interactable = false;
        }

        /// <summary>
        /// Enables the layout for the fight for AI ending.
        /// </summary>
        public void EnableForAILayout()
        {
            forAILayout.SetActive(true);
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
            moveCooldownSlider.maxValue = deletionTimeSeconds * 2f/3f; // The cooldown for moving the K-P file is 2/3 of the deletion time.
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
            
            if (deletionProgressBar.value >= deletionProgressBar.maxValue)
            {
                CompilationHelperMvc.Instance.CompilationHelperController.InvokeCompilationFailed();
            }
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

        /// <summary>
        /// Moves the K-P compilation file to a new location chosen by the player.
        /// </summary>
        public void OnMoveKpButtonClick()
        {
            // Model
            string currentKpPath = FourthWallMvc.Instance.CompilationSimulationController.GetKpCompilationPath();
            string newPath = EditorUtility.OpenFolderPanel("New K-P's Compilation Location", currentKpPath, "");
            
            if (string.IsNullOrEmpty(newPath))
            {
                return;
            }

            if (!FourthWallMvc.Instance.CompilationSimulationController.MoveKpCompilationPath(newPath))
            {
                return;
            }
            
            // UI
            deletionProgressBar.value = 0;
            StartCoroutine(MoveCooldownCoroutine());
        }

        private IEnumerator MoveCooldownCoroutine()
        {
            moveKpButton.interactable = false;
            moveCooldownSlider.value = moveCooldownSlider.maxValue;

            while (moveCooldownSlider.value > 0)
            {
                moveCooldownSlider.value -= Time.deltaTime;
                yield return null;
            }
            
            moveCooldownSlider.value = 0;
            moveKpButton.interactable = true;
        }
    }
}