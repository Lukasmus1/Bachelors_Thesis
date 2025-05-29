using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace DesktopGeneration
{
    public abstract class WallpaperGeneration
    {
        protected RawImage _wallpaperImage;

        protected WallpaperGeneration(RawImage wallpaperImage)
        {
            _wallpaperImage = wallpaperImage;
        }
        
        public abstract void GenerateWallpaper();
        
        protected void SetWallpaper(string wallpaperPath)
        {
            //Get image bytes and create a texture
            byte[] imageBytes= File.ReadAllBytes(wallpaperPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            
            //Setting the texture to the RawImage
            _wallpaperImage.texture = texture;
            
            //Setting the wallpaper to the desktop instance
            Desktop.Instance.Wallpaper = texture;
        }
    }
}