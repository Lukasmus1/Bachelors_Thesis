using System.Collections.Generic;
using System.Linq;
using Desktop.Commons;
using Desktop.Controllers;
using Desktop.Models;
using Story.Commons;
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
            if (!Bootstrapper.LoadedNewGame)
            {
                return;
            }
            
            GenerateRandomDesktop();
        }

        private void Start()
        {
            if (Bootstrapper.LoadedNewGame)
            {
                StoryMvc.Instance.StoryController.InitNew();
            }
            
            LoadDesktop();
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
        private void GenerateRandomDesktop()
        {
            SetDesktopWallpaper(_controller.GetRandomWallpaper());
            SetColorScheme(_controller.GetRandomColorScheme());
            SaveOrSetupIcons(true);
        }
        
        /// <summary>
        /// Loads the desktop.
        /// </summary>
        private void LoadDesktop()
        {
            SetDesktopWallpaper(DesktopModel.Instance.GetWallpaper());
            //Bright red as a color for error indication
            SetColorScheme(ColorUtility.TryParseHtmlString(DesktopModel.Instance.GetColorScheme(), out Color color) ? color : Color.red);
            SetIcons(DesktopModel.Instance.Icons);
        }
        
        /// <summary>
        /// Saves the existing icons on the desktop.
        /// </summary>
        private void SaveOrSetupIcons(bool shouldSetup)
        {
            foreach (GameObject icon in desktopIconObjects)
            {
                var iconClass = icon.GetComponent<IconClassOnObject>();
                if (shouldSetup)
                {
                    iconClass.SetProps();    
                }
                _controller.SetDesktopIconIntoContext(iconClass);
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
            DesktopModel.Instance.SetColorScheme(ColorUtility.ToHtmlStringRGBA(clr));
            
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
                //Sometimes there are null icons in the list, I don't know why
                if (iconClass.Image == null || string.IsNullOrEmpty(iconClass.Name))
                {
                    continue;
                }
                
                //If it is already instantiated, just update its properties
                GameObject iconObject = desktopIconObjects.FirstOrDefault(x => x.GetComponent<IconClassOnObject>().IconName == iconClass.Name);
                if (iconObject == null)
                {
                    iconObject = Instantiate(iconPrefab, iconParent);
                }

                iconObject.GetComponent<IconScript>().SetProperties(iconClass, userFont);
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
        
        /// <summary>
        /// Sets the active state of an icon based on its IconClass.name.
        /// </summary>
        /// <param name="iconName">GameObject name of the Icon</param>
        /// <param name="active">Should it be active</param>
        public void ToggleIcon(string iconName, bool active)
        {
            desktopIconObjects.FirstOrDefault(x => x.GetComponent<IconClassOnObject>().IconName == iconName)?.SetActive(active);
        }

        /// <summary>
        /// Save before exiting the application.
        /// </summary>
        private void OnApplicationQuit()
        {
            SaveOrSetupIcons(false);
        }
    }
}