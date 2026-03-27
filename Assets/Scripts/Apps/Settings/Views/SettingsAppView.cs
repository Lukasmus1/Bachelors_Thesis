using Desktop.Commons;
using Desktop.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.Settings.Views
{
    public class SettingsAppView : MonoBehaviour
    {
        [Header("Wallpaper")]
        [SerializeField] private TMP_Text currentWallpaperText;
        [SerializeField] private GameObject chooseWallpaperPopup;
        
        [Header("Color Scheme")]
        [SerializeField] private Image currentColorSchemeImage;
        [SerializeField] private TMP_Text currentColorSchemeText;
        [SerializeField] private GameObject chooseColorSchemePopup;

        //[Header("Sounds")]
        
        private void Awake()
        {
            chooseWallpaperPopup.GetComponent<ChangeWallpaperPopupView>().onChangeWallpaper += UpdateWallpaperText;
            
            UpdateWallpaperText();
            UpdateColorSchemeTextAndImage();
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
