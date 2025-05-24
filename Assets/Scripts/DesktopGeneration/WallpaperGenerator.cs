using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.UI;

namespace DesktopGeneration
{
    public class WallpaperGenerator
    {
        private List<string> _wallpapersPaths = new();
        
        private RawImage _wallpaperImage;
        
        public WallpaperGenerator(RawImage wallpaperImage)
        {
            _wallpaperImage = wallpaperImage;
            PopulateWallpapers();
        }

        public void SetUserWallpaper()
        {
            //Using the registry to get the current user's wallpaper path
            string wallpaperPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallPaper", null);
            SetWallpaper(wallpaperPath);
        }
        
        public void SetRandomWallpaper()
        {
            //Getting a random index 
            int randomIndex = Random.Range(0, _wallpapersPaths.Count);
            
            //Returning a random wallpaper path
            string randomPath = _wallpapersPaths[randomIndex];
            
            SetWallpaper(randomPath);
        }
        
        private void SetWallpaper(string wallpaperPath)
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
        
        private void PopulateWallpapers()
        {
            //Getting all files from a directory
            DirectoryInfo directoryInfo = new("Assets/Images/Desktop/DesktopWallpapers");
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                //Populating wallpaper paths
                _wallpapersPaths.Add(file.FullName);
            }   
        }
    }
}
