using System.IO;
using Desktop.Abstracts;
using Microsoft.Win32;
using UnityEngine;

namespace Desktop.Models
{
    public class WallpaperGeneratorUser : WallpaperGeneration
    {
        public override Texture2D GetWallpaperTexture()
        {
            //Using the registry to get the current user's wallpaper path
            string wallpaperPath = GetWallpaperPath();
            return GetWallpaper(wallpaperPath);
        }
        
        private static Texture2D GetWallpaper(string wallpaperPath)
        {
            //Get image bytes and create a texture
            byte[] imageBytes= File.ReadAllBytes(wallpaperPath);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            
            //Returning the wallpaper texture
            return texture;
        }

        private static string GetWallpaperPath() => (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallPaper", null);
    }
}