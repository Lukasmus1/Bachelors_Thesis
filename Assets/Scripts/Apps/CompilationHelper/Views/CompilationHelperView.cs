using System;
using Apps.Commons;
using Desktop.Commons;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.CompilationHelper.Views
{
    public class CompilationHelperView : AppsCommon
    {
        [SerializeField] private Slider progressBarSlider;

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }

        public void SetupProgressBar(int compilationTimeSeconds)
        {
            DesktopMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds += UpdateProgressBar;
            
            progressBarSlider.maxValue = compilationTimeSeconds;
            progressBarSlider.value = 0;
            
            gameObject.SetActive(true);
        }
        
        private void UpdateProgressBar(int seconds)
        {
            progressBarSlider.value = seconds;    
        }

        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }
    }
}