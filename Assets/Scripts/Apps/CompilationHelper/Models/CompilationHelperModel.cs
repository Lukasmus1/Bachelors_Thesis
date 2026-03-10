using System;
using System.Collections;
using Apps.CompilationHelper.Controllers;
using Commons;
using UnityEngine;

namespace Apps.CompilationHelper.Models
{
    public class CompilationHelperModel
    {
        public Action<int> onCompilationProgressUpdateSeconds;

        private int _compilationTimeSeconds;
        public int CompilationTimeSeconds
        {
            get => _compilationTimeSeconds == 0 ? throw new Exception("Called CompilationTimeSeconds getter before it was set. Make sure to call StartCompilation before trying to access CompilationTimeSeconds.") : _compilationTimeSeconds;
            private set => _compilationTimeSeconds = value;
        }
        private bool isCompilationRunning = false;

        private float elapsed;
        /// <inheritdoc cref="CompilationHelperController.GetCurrentCompilationTime"/> 
        public float GetCurrentCompilationTime()
        {
            return elapsed;
        }
        
        /// <inheritdoc cref="CompilationHelperController.IsCompilationRunning"/>
        public bool IsCompilationRunning()
        {
            return (elapsed < CompilationTimeSeconds) && isCompilationRunning;
        }
        
        /// <summary>
        /// Starts the compilation process.
        /// </summary>
        /// <param name="compilationTimeSeconds">Time of the compilation</param>
        public void StartCompilation(int compilationTimeSeconds)
        {
            CompilationTimeSeconds = compilationTimeSeconds;
            
            MonoBehaviour mbRef = Tools.GetScriptReferenceLinker().GetMonoBehavior();
            mbRef.StartCoroutine(CompilationCoroutine());
            isCompilationRunning = true;
        }

        /// <summary>
        /// Coroutine that simulates the compilation process and updates the progress every second.
        /// </summary>
        /// <returns>IEnumerator for coroutine</returns>
        private IEnumerator CompilationCoroutine()
        {
            float updateInterval = 0; 
            const float updateFrequency = 1f; // Update every second
            var secondCounter = 0;
            
            for (elapsed = 0; elapsed < CompilationTimeSeconds; elapsed += Time.deltaTime)
            {
                updateInterval += Time.deltaTime;

                if (updateInterval >= updateFrequency)
                {
                    secondCounter++;
                    onCompilationProgressUpdateSeconds?.Invoke(secondCounter);
                    updateInterval = 0f; // Reset the interval
                }
                
                yield return null;
            }
        }
    }
}