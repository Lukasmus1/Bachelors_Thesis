using System;
using System.Collections;
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

        public void StartCompilation(int compilationTimeSeconds)
        {
            CompilationTimeSeconds = compilationTimeSeconds;
            
            MonoBehaviour mbRef = Tools.GetScriptReferenceLinker().GetMonoBehavior();
            mbRef.StartCoroutine(CompilationCoroutine());
        }

        private IEnumerator CompilationCoroutine()
        {
            float updateInterval = 0; 
            const float updateFrequency = 1f; // Update every second
            var secondCounter = 0;
            
            for (float elapsed = 0; elapsed < CompilationTimeSeconds; elapsed += Time.deltaTime)
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