using System.Collections.Generic;
using System.Linq;
using Desktop.Commons;
using Desktop.Controllers;
using Desktop.Models;
using DesktopGeneration.Models;
using Saving.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Desktop.Views
{
    public class DesktopGeneratorView : MonoBehaviour
    {
        //DesktopGenerator Controller
        private readonly DesktopGeneratorController _controller = DesktopMvc.Instance.DesktopGeneratorController;
        
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
        [SerializeField] private GameObject iconPrefab;
        [SerializeField] private Transform iconParent;

        private void Awake()
        {
            SavingMvc.Instance.SavingController.OnGameLoaded += RefreshContext;
        }
        
        public void OnDestroy()
        {
            SavingMvc.Instance.SavingController.OnGameLoaded -= RefreshContext;
        }

        /// <summary>
        /// Exposed method for the Generate Desktop button.
        /// </summary>
        public void GenerateUserDesktopButton()
        {
            SetDesktopWallpaper(_controller.GetUserWallpaper());
            SetColorScheme(_controller.GetUserColorScheme());
            SetIcons(_controller.GetUserIcons(iconPrefab.GetComponent<RectTransform>().sizeDelta));
            
            ToggleDesktop(true);
        }

        /// <summary>
        /// Generate a random desktop.
        /// </summary>
        public void GenerateRandomDesktop()
        {
            SetDesktopWallpaper(_controller.GetRandomWallpaper());
            SetColorScheme(_controller.GetRandomColorScheme());
            SaveExistingIcons();
        }
        
        /// <summary>
        /// Refreshes the desktop.
        /// </summary>
        private void RefreshContext()
        {
            SetDesktopWallpaper(DesktopModel.Instance.GetWallpaper());
            SetColorScheme(ColorUtility.TryParseHtmlString("#" + DesktopModel.Instance.colorScheme, out Color color) ? color : Color.darkBlue);
            SetIcons(DesktopModel.Instance.Icons);
        }
        
        /// <summary>
        /// Saves the existing icons on the desktop.
        /// </summary>
        public void SaveExistingIcons()
        {
            foreach (GameObject icon in desktopIconObjects.Where(icon => icon.activeSelf))
            {
                _controller.SetDesktopIconIntoContext(icon.GetComponent<IconClassOnObject>());
            }
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
            DesktopModel.Instance.wallpaper = wallpaper.EncodeToPNG();
            
            //Enabling the wallpaper image
            wallpaperImage.gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets the color scheme.
        /// </summary>
        /// <param name="clr"></param>
        private void SetColorScheme(Color clr)
        {
            //Setting slight transparency
            clr.a = 0.998f;
            
            //Saving the color scheme in the desktop model
            DesktopModel.Instance.colorScheme = ColorUtility.ToHtmlStringRGBA(clr);
            
            //Setting the color scheme
            bottomBarBackground.color = clr;
        }
        
        /// <summary>
        /// Sets the icons and their properties on the desktop.
        /// </summary>
        /// <param name="icons">List of icons</param>
        private void SetIcons(List<IconClass> icons)
        {
            TMP_FontAsset userFont = _controller.GetUserFont();

            foreach (IconClass iconClass in icons)
            {
                //If it is already instantiated, just update its properties
                GameObject iconObject = desktopIconObjects.FirstOrDefault(x => x.GetComponent<IconClassOnObject>().IconName == iconClass.Name);
                if (iconObject == null)
                {
                    iconObject = Instantiate(iconPrefab, iconParent);
                }

                iconObject.GetComponent<IconScript>().SetProperties(iconClass, userFont);
                
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