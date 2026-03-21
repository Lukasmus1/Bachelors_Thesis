using System;
using System.Collections;
using System.Collections.Generic;
using Apps.Commons;
using Apps.CompilationHelper.Commons;
using Apps.CompilationHelper.Controllers;
using Apps.CompilationHelper.Models.ScatteredFilesActions;
using Desktop.Commons;
using FourthWall.Commons;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using User.Commons;
using User.Models;

namespace Apps.CompilationHelper.Views
{
    public class CompilationHelperView : AppsCommon
    {
        public Action onAllFilesDeleted;
        [SerializeField] private GameObject againstAILayout;
        [SerializeField] private Slider curatorProgressBarSlider;
        [SerializeField] private GameObject filesArea;
        [SerializeField] private GameObject kpFinalPathLayout;
        private readonly List<FileAction> fileActions = new();
        private int deletedFiles = 0;
        private bool aiProgressBarActive;
        
        [SerializeField] private GameObject forAILayout;
        [SerializeField] private Slider aiProgressBarSlider;
        [SerializeField] private GameObject kpFileMovingUI;
        [SerializeField] private Slider deletionProgressBar;
        [SerializeField] private Button moveKpButton;
        [SerializeField] private Slider moveCooldownSlider;

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
        /// Enables the specific layout based on the argument.
        /// </summary>
        /// <param name="aiLayout">Should a for AI layout be enabled?</param>
        public void EnableLayout(bool aiLayout)
        {
            againstAILayout.SetActive(!aiLayout); // just in case
            forAILayout.SetActive(aiLayout);
        }

        /// <summary>
        /// Sets up the compilation progress bar.
        /// </summary>
        /// <param name="compilationTimeSeconds">Time for finishing compilation</param>
        /// <param name="aiProgressBar">Should I setup AI progress bar?</param>
        public void SetupProgressBar(int compilationTimeSeconds, bool aiProgressBar)
        {
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds += UpdateProgressBar;

            if (aiProgressBar)
            {
                CompilationHelperMvc.Instance.CompilationHelperController.onCompilationFailed += Cleanup;
                CompilationHelperMvc.Instance.CompilationHelperController.onCompilationFinished += Cleanup;
                
                aiProgressBarSlider.maxValue = compilationTimeSeconds;
                aiProgressBarSlider.value = 0;
            }
            else
            {
                onAllFilesDeleted += OnAllFilesDeleted;
                
                curatorProgressBarSlider.maxValue = compilationTimeSeconds;
                curatorProgressBarSlider.value = 0;
                foreach (Transform child in filesArea.transform)
                {
                    var view = child.GetComponent<FileStatusView>();
                    FileAction action = view.GetAction(); 
                    fileActions.Add(action);
                    
                    action.onDeleteFile += OnFileDeleted;
                }
            }
            
            aiProgressBarActive = aiProgressBar;
        }

        /// <summary>
        /// Gets called everytime a file is deleted.
        /// </summary>
        private void OnFileDeleted()
        {
            deletedFiles++;

            if (deletedFiles != fileActions.Count) 
                return;
            
            onAllFilesDeleted?.Invoke();
            foreach (FileAction fileAction in fileActions)
            {
                fileAction.onDeleteFile -= OnFileDeleted;
            }
        }

        /// <summary>
        /// Actions that happen when all files are successfully deleted.
        /// </summary>
        private void OnAllFilesDeleted()
        {
            filesArea.SetActive(false);
            kpFinalPathLayout.SetActive(true);
        }

        /// <summary>
        /// Opens the final K-P's compilation path.
        /// </summary>
        public void OpenFinalKpPath()
        {
            string kpPath = UserMvc.Instance.UserController.ProceduralData(UserDataType.KpLocation);
            FourthWallMvc.Instance.FileGenerationController.OpenFileExplorer(kpPath);
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
            if (aiProgressBarActive)
                aiProgressBarSlider.value = seconds;
            else
                curatorProgressBarSlider.value = seconds;
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