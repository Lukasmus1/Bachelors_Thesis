using DesktopGeneration.Abstracts;
using Microsoft.Win32;
using UnityEngine.UI;

namespace DesktopGeneration
{
    public class WallpaperGeneratorUser : WallpaperGeneration
    {
        public WallpaperGeneratorUser(RawImage wallpaperImage) : base(wallpaperImage) {}

        public override void GenerateWallpaper()
        {
            //Using the registry to get the current user's wallpaper path
            string wallpaperPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallPaper", null);
            SetWallpaper(wallpaperPath);
        }
    }
}