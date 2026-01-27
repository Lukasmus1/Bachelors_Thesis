using Desktop.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.VirusFinder.Views
{
    public class VirusFinderView : MonoBehaviour
    {
        [SerializeField] private Slider scanProgressBar;
        [SerializeField] private TMP_Text percentageText;
        [SerializeField] private TMP_Text resultText;
        
        
        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }
        
        private void OnDisable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
            
            resultText.text = "";
            scanProgressBar.value = 0;
            percentageText.text = "0%";
        }
    }
}