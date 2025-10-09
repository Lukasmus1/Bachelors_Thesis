using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace DesktopGeneration.Abstracts
{
    public abstract class WallpaperGeneration
    {
        public abstract Texture2D GetWallpaperTexture();
        
        protected static Texture2D GetWallpaper(string wallpaperPath)
        {
            //Get image bytes and create a texture
            byte[] imageBytes= File.ReadAllBytes(wallpaperPath);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            
            //Returning the wallpaper texture
            return texture;
        }
    }
}