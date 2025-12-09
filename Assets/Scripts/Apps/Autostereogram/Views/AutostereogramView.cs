using System.Collections.Generic;
using System.Linq;
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
        [Header("ASG Images")]
        [SerializeField] private GameObject autostereogramImageHolder;
        private Color32[] _asgImagePixels;
        private RectTransform _asgImageRectTransform;
        
        [SerializeField] private GameObject autostereogramMovingImageHolder;
        private Color32[] _asgMovingImagePixels;
        private RectTransform _asgMovingImageRectTransform;
        
        [SerializeField] private Image overlappingLayer;
        private Texture2D _overlappingLayerTexture;
        private Color[] _overlappingPixels;
        
        //ASG Slider
        [Header("ASG Slider")]
        [SerializeField] private Slider movingImageSlider;

        private void Awake()
        {
            //Get RectTransforms for easier manipulation
            _asgImageRectTransform = autostereogramImageHolder.GetComponent<RectTransform>();
            _asgMovingImageRectTransform = autostereogramMovingImageHolder.GetComponent<RectTransform>();
            
            //Initialize overlapping pixels array
            _overlappingPixels = new Color[(int)_asgMovingImageRectTransform.rect.width * (int)_asgMovingImageRectTransform.rect.height];
            
            //Get the pixels of both images as Color32 arrays
            _asgImagePixels = autostereogramImageHolder.GetComponent<Image>().sprite.texture.GetPixels32();
            _asgMovingImagePixels = autostereogramMovingImageHolder.GetComponent<Image>().sprite.texture.GetPixels32();
            
            //Create the overlapping layer texture
            CreateOverlappingLayerTexture();
            
            //Sets the slider's bounds 
            movingImageSlider.maxValue = _asgMovingImageRectTransform.anchoredPosition.x;
            movingImageSlider.minValue = _asgImageRectTransform.anchoredPosition.x;
            
            //Set the slider initial value
            movingImageSlider.value = _asgImageRectTransform.position.x;
        }
        
        /// <summary>
        /// Method to be called when the moving image slider value is changed.
        /// Moves the autostereogram moving image and checks for overlapping pixels.
        /// </summary>
        public void SetMovingImage()
        {
            autostereogramMovingImageHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(movingImageSlider.value, autostereogramImageHolder.GetComponent<RectTransform>().anchoredPosition.y);
            
            CheckForOverlappingPixels();
        }

        /// <summary>
        /// Moves the slider by one unit to the left or right.
        /// Useful for button controls.
        /// </summary>
        /// <param name="left">Should the slider move to the left?</param>
        public void MoveByOne(bool left)
        {
            movingImageSlider.value += left ? -1 : 1;
        }

        private void CheckForOverlappingPixels()
        {
            var overlappingWidth = (int)(430 - _asgMovingImageRectTransform.anchoredPosition.x);
            var asgImageHeight = (int)_asgImageRectTransform.rect.height;
            var asgImageWidth = (int)_asgImageRectTransform.rect.width;
            var movingImageWidth = (int)_asgMovingImageRectTransform.rect.width;

            if (overlappingWidth <= 0)
            {
                return;
            }

            for (int y = 0; y < asgImageHeight; y++)
            {
                for (int x = 0; x < overlappingWidth; x++)
                {
                    var movingIdx = y * movingImageWidth + x;
                    var staticIdx = y * asgImageWidth + (asgImageWidth - overlappingWidth + x);

                    if (CompareColor32(_asgMovingImagePixels[movingIdx], _asgImagePixels[staticIdx]))
                    {
                        _overlappingPixels[movingIdx] = Color.black;
                    }
                    else
                    {
                        _overlappingPixels[movingIdx] = Color.clear;
                    }
                }
            }

            _overlappingLayerTexture.SetPixels(_overlappingPixels);
            _overlappingLayerTexture.Apply();
        }

        /// <summary>
        /// Compares two Color32 objects for equality.
        /// </summary>
        /// <param name="c1">First color</param>
        /// <param name="c2">Second color</param>
        /// <returns>True if these 2 color are equal</returns>
        private static bool CompareColor32(Color32 c1, Color32 c2) => c1.a == c2.a && c1.r == c2.r && c1.g == c2.g && c1.b == c2.b;
        
        /// <summary>
        /// Creates a transparent overlapping layer texture on top of the autostereogram moving image.
        /// </summary>
        private void CreateOverlappingLayerTexture()
        {
            var width = (int)_asgMovingImageRectTransform.rect.width;
            var height = (int)_asgMovingImageRectTransform.rect.height;

            var pixels = new List<Color>(width * height);
            for (var i = 0; i < width * height; i++)
            {
                pixels.Add(Color.clear);
            }
            
            var overlapTex = new Texture2D(width, height);
            overlapTex.SetPixels(pixels.ToArray());
            overlapTex.Apply();
            _overlappingLayerTexture = overlapTex;
            overlappingLayer.sprite = Sprite.Create(overlapTex, new Rect(0, 0, overlapTex.width, overlapTex.height), new Vector2(0.5f, 0.5f));
        }
    }
}