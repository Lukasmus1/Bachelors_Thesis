using System;
using System.Collections.Generic;
using Apps.Commons;
using Apps.FileViewer.Commons;
using Apps.FileViewer.Models;
using Desktop.Commons;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.Autostereogram.Views
{
    /// <summary>
    /// View to be placed on the GameObject representing the Autostereogram app. (ASG = Autostereogram)
    /// </summary>
    public class AutostereogramView : AppsCommon
    {
        //ASG Images
        [Header("ASG Images")]
        [SerializeField] private GameObject autostereogramImageHolder;
        private Color32[] _asgImagePixels;
        private RectTransform _asgImageRectTransform;
        
        [SerializeField] private GameObject autostereogramMovingImageHolder;
        private RectTransform _asgMovingImageRectTransform;
        
        [SerializeField] private Image overlappingLayer;
        private Texture2D _overlappingLayerTexture;
        private Color[] _overlappingPixels;
        
        //ASG Slider
        [Header("ASG Slider")]
        [SerializeField] private Slider movingImageSlider;

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
            
            //Set the autostereogram image to the opened file's image
            GameObject openedFile = FileViewerMvc.Instance.FileLoaderController.OpenedFile;
            Sprite imageToSet = openedFile.GetComponentInChildren<Image>().sprite;
            if (imageToSet == null)
            {
                gameObject.SetActive(false);
                NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.Error, "This file cannot be opened as an autostereogram.");
                return;
            }
            
            autostereogramImageHolder.GetComponent<Image>().sprite = imageToSet;
            autostereogramMovingImageHolder.GetComponent<Image>().sprite = imageToSet;

            if (openedFile.GetComponent<FileProps>().intendedFileType != FileType.Autostereogram)
            {
                return;
            }
            
            //Get RectTransforms for easier manipulation
            _asgImageRectTransform = autostereogramImageHolder.GetComponent<RectTransform>();
            _asgMovingImageRectTransform = autostereogramMovingImageHolder.GetComponent<RectTransform>();
            
            //Initialize overlapping pixels array
            _overlappingPixels = new Color[(int)_asgMovingImageRectTransform.rect.width * (int)_asgMovingImageRectTransform.rect.height];
            
            //Get the pixels of both images as Color32 arrays
            _asgImagePixels = autostereogramImageHolder.GetComponent<Image>().sprite.texture.GetPixels32();
            
            //Create the overlapping layer texture
            CreateOverlappingLayerTexture();
            
            //Sets the slider's bounds 
            movingImageSlider.maxValue = _asgMovingImageRectTransform.rect.width;
            movingImageSlider.minValue = 0;
            
            //Set the slider initial value
            movingImageSlider.value = movingImageSlider.maxValue;
        }

        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }

        /// <summary>
        /// Method to be called when the moving image slider value is changed.
        /// Moves the autostereogram moving image and checks for overlapping pixels.
        /// </summary>
        public void SetMovingImage()
        {
            autostereogramMovingImageHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(movingImageSlider.value, autostereogramMovingImageHolder.GetComponent<RectTransform>().anchoredPosition.y);
            
            CheckAndSetOverlappingPixels();
        }

        /// <summary>
        /// Moves the slider by one unit relative to pixel size to the left or right.
        /// Useful for button controls.
        /// </summary>
        /// <param name="left">Should the slider move to the left?</param>
        public void MoveByOne(bool left)
        {
            movingImageSlider.value += left ? -2 : 2;
        }

        /// <summary>
        /// Checks for overlapping pixels between the autostereogram image and the moving image.
        /// Sets the overlapping layer texture pixels accordingly.
        /// </summary>
        private void CheckAndSetOverlappingPixels()
        {
            //The texture dimensions divided by 2 because the images are scaled by 2 in the UI
            int asgImageHeight = (int)_asgImageRectTransform.rect.height / 2;
            int asgImageWidth = (int)_asgImageRectTransform.rect.width / 2;
            int overlappingWidth = (int)_asgMovingImageRectTransform.anchoredPosition.x / 2;

            Array.Clear(_overlappingPixels, 0 , _overlappingPixels.Length);

            //If we don't have overlapping area, return
            if (overlappingWidth <= 0 || overlappingWidth > asgImageWidth)
            {
                _overlappingLayerTexture.SetPixels(_overlappingPixels);
                _overlappingLayerTexture.Apply();
                return;
            }
            
            //Check for overlapping pixels and set them to black in the overlapping layer texture
            for (var y = 0; y < asgImageHeight; y++)
            {
                for (int x = overlappingWidth; x < asgImageWidth; x++)
                {
                    int asgIndex = x + y * asgImageWidth;
                    int movingIndex = (x - overlappingWidth) + y * asgImageWidth;
                    
                    if (CompareColor32(_asgImagePixels[asgIndex], _asgImagePixels[movingIndex]))
                    {
                        _overlappingPixels[movingIndex] = Color.black;
                    }
                }
            }

            //Apply the changes to the overlapping layer texture
            _overlappingLayerTexture.SetPixels(_overlappingPixels);
            _overlappingLayerTexture.Apply();
        }


        /// <summary>
        /// Compares two Color32 objects for equality.
        /// </summary>
        /// <param name="c1">First color</param>
        /// <param name="c2">Second color</param>
        /// <returns>True if these 2 color are equal</returns>
        private static bool CompareColor32(Color32 c1, Color32 c2) => c1.r == c2.r && c1.g == c2.g && c1.b == c2.b;
        
        /// <summary>
        /// Creates a transparent overlapping layer texture on top of the autostereogram moving image.
        /// </summary>
        private void CreateOverlappingLayerTexture()
        {
            var width = (int)_asgMovingImageRectTransform.rect.width / 2;
            var height = (int)_asgMovingImageRectTransform.rect.height / 2;

            var pixels = new List<Color>(width * height);
            for (var i = 0; i < width * height; i++)
            {
                pixels.Add(Color.clear);
            }
            
            var overlapTex = new Texture2D(width, height)
            {
                filterMode = FilterMode.Point,
            };

            overlapTex.SetPixels(pixels.ToArray());
            overlapTex.Apply();
            _overlappingLayerTexture = overlapTex;
            overlappingLayer.sprite = Sprite.Create(overlapTex, new Rect(0, 0, overlapTex.width, overlapTex.height), new Vector2(0.5f, 0.5f));
        }
    }
}