using System;
using Apps.Commons;
using Desktop.Commons;
using Desktop.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.Settings.Views
{
    public class SettingsAppView : AppsCommon
    {
        [Header("Wallpaper")]
        [SerializeField] private TMP_Text currentWallpaperText;
        [SerializeField] private GameObject chooseWallpaperPopup;
        
        [Header("Color Scheme")]
        [SerializeField] private Image currentColorSchemeImage;
        [SerializeField] private TMP_Text currentColorSchemeText;
        [SerializeField] private GameObject chooseColorSchemePopup;

        //[Header("Sounds")]

        private void Start()
        {
            chooseWallpaperPopup.GetComponent<ChangeWallpaperPopupView>().onChangeWallpaper += UpdateWallpaperText;
        }

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
            
            UpdateWallpaperText();
            UpdateColorSchemeTextAndImage();
        }

        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }
        
        private void OnDestroy()
        {
            chooseWallpaperPopup.GetComponent<ChangeWallpaperPopupView>().onChangeWallpaper -= UpdateWallpaperText;    
        }
        
        /// <summary>
        /// Opens the window for selecting a wallpaper.
        /// </summary>
        public void OpenChooseWallpaperPopup()
        {
            chooseWallpaperPopup.SetActive(true);
        }

        /// <summary>
        /// Updates the current wallpaper text to reflect the current wallpaper set in the desktop model.
        /// </summary>
        private void UpdateWallpaperText()
        {
            currentWallpaperText.text = DesktopMvc.Instance.DesktopGeneratorController.GetCurrentWallpaperName();
        }

        /// <summary>
        /// Opens the windows for selecting a color scheme
        /// </summary>
        public void OpenChooseColorSchemePopup()
        {
            chooseColorSchemePopup.SetActive(true);
        }

        /// <summary>
        /// Changes the current color scheme 
        /// </summary>
        public void ChangeColorScheme()
        {
            var fcp = chooseColorSchemePopup.GetComponent<FlexibleColorPicker>();
            
            DesktopMvc.Instance.DesktopGeneratorController.ChangeColorScheme(fcp.color);
            
            UpdateColorSchemeTextAndImage();
            
            CancelChooseColorSchemePopup();
        }

        /// <summary>
        /// Hides the change color scheme popup
        /// </summary>
        public void CancelChooseColorSchemePopup()
        {
            chooseColorSchemePopup.SetActive(false);
        }
        
        /// <summary>
        /// Updates the color scheme text and preview image
        /// </summary>
        private void UpdateColorSchemeTextAndImage()
        {
            currentColorSchemeText.text = DesktopModel.Instance.GetColorScheme();
            
            ColorUtility.TryParseHtmlString(DesktopModel.Instance.GetColorScheme(), out Color c);
            currentColorSchemeImage.color = c;
        }
    }
}
