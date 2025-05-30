using System.Collections.Generic;
using System.IO;
using DesktopGeneration.Abstracts;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.UI;

namespace DesktopGeneration
{
    public class WallpaperGeneratorRandom : WallpaperGeneration
    {
        private List<string> _wallpapersPaths = new();
        
        public WallpaperGeneratorRandom(RawImage wallpaperImage) : base(wallpaperImage)
        {
            PopulateWallpapers();
        }

        public override void GenerateWallpaper()
        {
            //Getting a random index 
            int randomIndex = Random.Range(0, _wallpapersPaths.Count);
            
            //Returning a random wallpaper path
            string randomPath = _wallpapersPaths[randomIndex];
            
            SetWallpaper(randomPath);
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
