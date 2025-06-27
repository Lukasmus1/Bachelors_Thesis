using System.Collections.Generic;
using DesktopGeneration.Abstracts;
using DesktopGeneration.IconGeneration;
using TMPro;
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
        
        
        private WallpaperGeneration _wallpaperGeneratorRandom;
        private IColorGeneration _colorSchemeGenerator;
        private Abstracts.IconGeneration _iconGenerator;
        private FontScript _fontGenerator;
        
        public void GenerateDesktopButton()
        {
            Debug.Log("Generating desktop...");
            
            //Generating desktop
            //Wallpaper
            _wallpaperGeneratorRandom = new WallpaperGeneratorRandom(wallpaperImage);
            
            //Color scheme
            _colorSchemeGenerator = new ColorGenerationRandom(bottomBarBackground);
            _colorSchemeGenerator.GenerateColorScheme();
            
            //Bottom bar icons
            //_iconGenerator = new IconGeneration(iconSprites, bottomBarIconObjects);
            
            GenerateDesktop();
        }

        public void GenerateUserDesktopButton()
        {
            Debug.Log("Generating user desktop...");
            
            //Generating desktop
            //Wallpaper
            _wallpaperGeneratorRandom = new WallpaperGeneratorUser(wallpaperImage);
            
            //Color scheme
            _colorSchemeGenerator = new ColorGenerationUser(bottomBarBackground);
            
            //Bottom bar icons
            //tbd
            
            //Desktop icons
            _iconGenerator = new IconGenerationUser(desktopIconObjects);
            
            //Font
            _fontGenerator = new FontScript(desktopIconObjects);
            _fontGenerator.SetUserFont();
            
            //Generate the desktop
            GenerateDesktop();
        }

        private void GenerateDesktop()
        {
            //Wallpaper
            _wallpaperGeneratorRandom.GenerateWallpaper();
            
            //Color scheme
            _colorSchemeGenerator.GenerateColorScheme();
            
            //Bottom bar icons
            //tbd
            
            //Desktop icons
            _iconGenerator.GenerateIcons();
            
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
