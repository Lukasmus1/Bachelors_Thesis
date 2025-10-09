using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DesktopGeneration.Controllers;
using DesktopGeneration.Models;
using TMPro;

namespace DesktopGeneration.Views
{
    public class DesktopGeneratorView : MonoBehaviour
    {
        //DesktopGenerator Controller
        private DesktopGeneratorController _controller;
        
        //References for activating desktop generation
        [SerializeField] private GameObject desktopObject;
        [SerializeField] private GameObject createDesktopButton;
        
        //References for the wallpaper generator
        [Header("Wallpaper")]
        [SerializeField] private RawImage wallpaperImage;
        
        //References for the color scheme generator
        [Header("Bottom Bar")] 
        [SerializeField] private Image bottomBarBackground;
        
        //References for icon generation
        [Header("Icons")]
        [SerializeField] private List<GameObject> desktopIconObjects;

        private void Awake()
        {
            _controller = new DesktopGeneratorController();
        }

        /// <summary>
        /// Exposed method for the Generate Desktop button.
        /// </summary>
        public void GenerateUserDesktopButton()
        {
            SetDesktopWallpaper(_controller.GetUserWallpaper());
            SetColorScheme(_controller.GetUserColorScheme());
            SetIcons(_controller.GetUserIcons());
            
            ToggleDesktop(true);
        }

        /// <summary>
        /// Sets the desktop wallpaper.
        /// </summary>
        /// <param name="wallpaper">Wallpaper as Texture2D</param>
        private void SetDesktopWallpaper(Texture2D wallpaper)
        {
            //Setting the wallpaper image
            wallpaperImage.texture = wallpaper;
            
            //Setting the wallpaper in the desktop model
            Desktop.Instance.Wallpaper = wallpaper;
        }

        /// <summary>
        /// Sets the color scheme.
        /// </summary>
        /// <param name="clr"></param>
        private void SetColorScheme(Color clr)
        {
            //Setting slight transparency
            clr.a = 0.998f;
            
            Desktop.Instance.ColorScheme = clr;
            
            //Getting the current color scheme
            Color colorScheme = Desktop.Instance.ColorScheme;
            
            //Setting the color scheme
            bottomBarBackground.color = colorScheme;
        }

        /// <summary>
        /// Sets the icons and their properties on the desktop.
        /// </summary>
        /// <param name="icons">List of icons</param>
        private void SetIcons(List<IconClass> icons)
        {
            TMP_FontAsset userFont = _controller.GetUserFont();
            
            for (int i = 0; i < icons.Count; i++)
            {
                IconClass icon = icons[i];
                GameObject iconObject = desktopIconObjects[i];
                
                //Getting the icon prefab default size to calculate the relative scale 
                //NewScale / OldScale 
                var oldIconSize = new Vector2(iconObject.GetComponent<RectTransform>().sizeDelta.x, iconObject.GetComponent<RectTransform>().sizeDelta.y); 
                var iconRelativeScale = new Vector2(icon.Size.x / oldIconSize.x, icon.Size.y / oldIconSize.y);
                
                //Setting the icon size
                iconObject.GetComponent<RectTransform>().localScale = new Vector3(iconRelativeScale.x, iconRelativeScale.y, 1f);
                iconObject.GetComponentInChildren<TMP_Text>().text = icon.Name;
                iconObject.GetComponent<RectTransform>().anchoredPosition = icon.Position;
                iconObject.GetComponentInChildren<RawImage>().texture = icon.Image;
                iconObject.GetComponentInChildren<TMP_Text>().font = userFont;

                iconObject.SetActive(true);
            }
        }
        
        
        /// <summary>
        /// Helper method to enable or disable the desktop object and the create desktop button.
        /// </summary>
        private void ToggleDesktop(bool enable)
        {
            //Enabling the desktop object
            desktopObject.SetActive(enable);
            createDesktopButton.SetActive(!enable);
        }
    }
}