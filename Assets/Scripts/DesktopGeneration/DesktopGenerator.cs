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
        //Desktop icons
        [SerializeField] private List<GameObject> desktopIconObjects;
        
        
        private WallpaperGenerator _wallpaperGenerator;
        private IColorGeneration _colorSchemeGenerator;
        private IconGeneration _iconGenerator;
        private FontScript _fontGenerator;
        
        public void GenerateDesktopButton()
        {
            Debug.Log("Generating desktop...");
            
            //Generating desktop
            //Wallpaper
            _wallpaperGenerator = new WallpaperGenerator(wallpaperImage);
            GenerateWallpaper();
            
            //Color scheme
            _colorSchemeGenerator = new ColorGenerationRandom(bottomBarBackground);
            _colorSchemeGenerator.GenerateColorScheme();
            
            //Bottom bar icons
            //_iconGenerator = new IconGeneration(iconSprites, bottomBarIconObjects);
            
        }

        public void GenerateUserDesktopButton()
        {
            Debug.Log("Generating user desktop...");
            
            //Generating desktop
            //Wallpaper
            _wallpaperGenerator = new WallpaperGenerator(wallpaperImage);
            _wallpaperGenerator.SetUserWallpaper();
            EnableDesktop();
            
            //Color scheme
            _colorSchemeGenerator = new ColorGenerationUser(bottomBarBackground);
            
            //Bottom bar icons
            //tbd
            
            //Desktop icons
            _iconGenerator = new IconGeneration(desktopIconObjects);
            print(_iconGenerator.GenerateUserDesktopIcons());
            
            //Font
            _fontGenerator = new FontScript(desktopIconObjects);
            _fontGenerator.SetUserFont();
            
            
            //Generate the desktop
            GenerateDesktop();
        }

        private void GenerateDesktop()
        {
            //Color scheme
            _colorSchemeGenerator.GenerateColorScheme();
        }

        private void GenerateWallpaper()
        {
            //Getting a random wallpaper path
            _wallpaperGenerator.SetRandomWallpaper();
            
            EnableDesktop();
        }

        private void EnableDesktop()
        {
            //Enabling the desktop object
            desktopObject.SetActive(true);
            createDesktopButton.SetActive(false);
        }
    }
}
