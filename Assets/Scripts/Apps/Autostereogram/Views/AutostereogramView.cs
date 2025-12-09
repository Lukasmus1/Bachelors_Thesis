using UnityEngine;
using UnityEngine.UI;

namespace Apps.Autostereogram.Views
{
    /// <summary>
    /// View to be placed on the GameObject representing the Autostereogram app. (ASG = Autostereogram)
    /// </summary>
    public class AutostereogramView : MonoBehaviour
    {
        //ASG Images
        [SerializeField] private GameObject autostereogramImageHolder;
        [SerializeField] private GameObject autostereogramMovingImageHolder;
        
        //ASG Slider
        [SerializeField] private Slider movingImageSlider;

        private void Awake()
        {
            //Sets the slider's bounds 
            movingImageSlider.maxValue = autostereogramMovingImageHolder.GetComponent<RectTransform>().anchoredPosition.x;
            movingImageSlider.minValue = autostereogramImageHolder.GetComponent<RectTransform>().anchoredPosition.x;
            
            //Set the slider initial value
            movingImageSlider.value = autostereogramImageHolder.GetComponent<RectTransform>().position.x;
        }
        
        public void SetMovingImage()
        {
            autostereogramMovingImageHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(movingImageSlider.value, autostereogramImageHolder.GetComponent<RectTransform>().anchoredPosition.y);
            
            CheckForOverlappingPixels();
        }

        private void CheckForOverlappingPixels()
        {
            Color[] staticImage = autostereogramImageHolder.GetComponent<Texture2D>().GetPixels();
            Color[] movingImage = autostereogramMovingImageHolder.GetComponent<Texture2D>().GetPixels();
            
            
        }
    }
}