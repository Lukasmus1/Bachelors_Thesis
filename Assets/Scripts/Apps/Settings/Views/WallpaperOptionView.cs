using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Apps.Settings.Views
{
    public class WallpaperOptionView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image wallpaperPreview;
        [SerializeField] private TMP_Text wallpaperNameText;

        [SerializeField] private Image highlightImage;
        private Color defaultHighlightColor; // reference color
        private Color highlightColor; // mouse enters the prefab color
        private Color selectedHighlightColor; // user selects the prefab color
        
        private ChangeWallpaperPopupView parentViewScript;
        
        /// <summary>
        /// Initialize the prefab's properties
        /// </summary>
        /// <param name="wallpaper">Wallpaper sprite</param>
        /// <param name="wallpaperName">Wallpaper name</param>
        /// <param name="parentScript">Parent script view</param>
        public void Initialize(Sprite wallpaper, string wallpaperName, ChangeWallpaperPopupView parentScript)
        {
            wallpaperNameText.text = wallpaperName;
            wallpaperPreview.sprite = wallpaper;
            parentViewScript = parentScript;
            
            defaultHighlightColor = highlightImage.color;
            highlightColor = new Color(defaultHighlightColor.r, defaultHighlightColor.g, defaultHighlightColor.b, 0.2f);
            selectedHighlightColor = new Color(defaultHighlightColor.r, 0.7f, defaultHighlightColor.b, 0.5f);
            
            highlightImage.color = Color.clear;

            parentViewScript.onSelectWallpaper += CheckForHighlight;
        }

        /// <summary>
        /// Select the specific wallpaper option.
        /// </summary>
        /// <param name="eventData">PointerEventData</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            parentViewScript.SelectWallpaper(wallpaperNameText.text);
            highlightImage.color = selectedHighlightColor;
        }

        /// <summary>
        /// Mouse enters the wallpaper preview
        /// </summary>
        /// <param name="eventData">PointerEventData</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (parentViewScript.selectedWallpaper == wallpaperNameText.text)
            {
                return;
            }
            
            highlightImage.color = highlightColor;
        }

        /// <summary>
        /// Mouse exits the wallpaper preview
        /// </summary>
        /// <param name="eventData">PointerEventData</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            CheckForHighlight();
        }

        /// <summary>
        /// Checks if the prefab should be highlighted.
        /// </summary>
        private void CheckForHighlight()
        {
            if (parentViewScript.selectedWallpaper == wallpaperNameText.text)
            {
                return;
            }
            
            highlightImage.color = Color.clear;
        }
    }
}