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

        private static string GetWallpaperPath() => (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallPaper", null);
    }
}