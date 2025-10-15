using System.Collections.Generic;
using System.IO;
using Desktop.Abstracts;
using UnityEngine;

namespace Desktop.Models
{
    public class WallpaperGeneratorRandom : WallpaperGeneration
    {
        private List<string> _wallpapersPaths = new();
        
        public WallpaperGeneratorRandom()
        {
            PopulateWallpapers();
        }

        public override Texture2D GetWallpaperTexture()
        {
            return GetWallpaper(GetRandomPath());
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

        private string GetRandomPath()
        {
            //Getting a random index 
            int randomIndex = Random.Range(0, _wallpapersPaths.Count);
            
            //Returning a random wallpaper path
            return _wallpapersPaths[randomIndex];
        }
    }
}
