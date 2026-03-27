using System;
using Desktop.Commons;
using Desktop.Models;
using TMPro;
using UnityEngine;

namespace Apps.Settings.Views
{
    public class ChangeWallpaperPopupView : MonoBehaviour
    {
        [SerializeField] private GameObject wallpaperOptionPrefab;
        [SerializeField] private Transform wallpaperContainer;
        [SerializeField] private TMP_Text selectedWallpaperName;

        public Action onSelectWallpaper; 
        public string selectedWallpaper = string.Empty;
        
        public Action onChangeWallpaper;
        
        private void Awake()
        {
            selectedWallpaperName.text = $"<b>Selected Wallpaper:</b> {DesktopModel.Instance.wallpaperName}";
            
            //Load all wallpapers from resources and create an option for each
            Sprite[] wallpapers = Resources.LoadAll<Sprite>("Wallpapers");
            foreach (Sprite wallpaper in wallpapers)
            {
                Instantiate(wallpaperOptionPrefab, wallpaperContainer).GetComponent<WallpaperOptionView>().Initialize(wallpaper, wallpaper.name, this);
            }
        }

        /// <summary>
        /// Select wallpaper and update the selected wallpaper text. Also invokes the onSelectWallpaper action to notify other scripts of the change.
        /// </summary>
        /// <param name="wallpaperName">Name of the selected wallpaper</param>
        public void SelectWallpaper(string wallpaperName)
        {
            selectedWallpaperName.text = $"<b>Selected Wallpaper:</b> {wallpaperName}";
            selectedWallpaper = wallpaperName;
            onSelectWallpaper?.Invoke();
        }
        
        /// <summary>
        /// Changes the wallpaper for the selected one.
        /// </summary>
        public void ChangeWallpaper()
        {
            if (selectedWallpaper == string.Empty)
            {
                return;
            }
            
            DesktopMvc.Instance.DesktopGeneratorController.ChangeWallpaper(selectedWallpaper);
            
            onChangeWallpaper?.Invoke();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Hides the popup.
        /// </summary>
        public void CancelButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}