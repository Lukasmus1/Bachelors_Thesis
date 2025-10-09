using DesktopGeneration.Abstracts;
using DesktopGeneration.Models;
using UnityEngine;

namespace DesktopGeneration.Controllers
{
    public class DesktopGeneratorController
    {
        private WallpaperGeneration _wallpaperGenerator;
        private ColorGeneration _colorSchemeGenerator;
        
        public Texture2D GetUserWallpaper()
        {
            _wallpaperGenerator = new WallpaperGeneratorUser();
            return _wallpaperGenerator.GetWallpaperTexture();
        }

        public Color GetUserColorScheme()
        {
            _colorSchemeGenerator = new ColorGenerationUser();
            return _colorSchemeGenerator.GenerateUserColorScheme();
        }

        // public Color GetRandomColorScheme()
        // {
        //     _colorSchemeGenerator = new ColorGeneration();
        //     return ColorGeneration.GenerateRandomColorScheme();
        // }
    }
}