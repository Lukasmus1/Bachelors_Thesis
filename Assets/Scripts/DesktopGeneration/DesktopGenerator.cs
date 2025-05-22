using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DesktopGeneration
{
    public class DesktopGenerator : MonoBehaviour
    {
        //References for activating desktop generation
        [SerializeField] private GameObject desktopObject;
        [SerializeField] private GameObject createDesktopButton;
        
        //References for the wallpaper generator
        [Header("Wallpaper")]
        [SerializeField] private RawImage wallpaperImage;
        
        //References for the color scheme generator
        [Header("Wallpaper")] 
        [SerializeField] private Image bottomBarBackground;
        
        //References for icon generation
        [Header("Icons")]
        [SerializeField] private List<Sprite> iconSprites;
        //Bottom bar
        [SerializeField] private List<GameObject> bottomBarIconObjects;
        
        private WallpaperGenerator _wallpaperGenerator;
        private ColorSchemeGeneration _colorSchemeGenerator;
        private IconGeneration _iconGenerator;
        
        public void GenerateDesktop()
        {
            Debug.Log("Generating desktop...");
            
            //Generating desktop
            //Wallpaper
            _wallpaperGenerator = new WallpaperGenerator();
            GenerateWallpaper();
            
            //Color scheme
            _colorSchemeGenerator = new ColorSchemeGeneration(bottomBarBackground);
            _colorSchemeGenerator.GenerateColorScheme();
            
            //Bottom bar icons
            _iconGenerator = new IconGeneration(iconSprites, bottomBarIconObjects);
            
        }

        private void GenerateWallpaper()
        {
            //Getting a random wallpaper path
            _wallpaperGenerator.SetRandomWallpaper(wallpaperImage);
            
            //Enabling the desktop object
            desktopObject.SetActive(true);
            createDesktopButton.SetActive(false);
        }
    }
}
