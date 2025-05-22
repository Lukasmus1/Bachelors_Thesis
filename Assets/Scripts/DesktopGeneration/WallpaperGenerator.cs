using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace DesktopGeneration
{
    public class WallpaperGenerator
    {
        private List<string> _wallpapersPaths = new();
        
        public WallpaperGenerator()
        {
            PopulateWallpapers();
        }

        public void SetRandomWallpaper(RawImage bg)
        {
            //Getting a random index 
            int randomIndex = Random.Range(0, _wallpapersPaths.Count);
            
            //Returning a random wallpaper path
            string randomPath = _wallpapersPaths[randomIndex];
            
            //Get image bytes and create a texture
            byte[] imageBytes= File.ReadAllBytes(randomPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            
            //Setting the texture to the RawImage
            bg.texture = texture;
            
            //Setting the wallpaper to the desktop isntance
            Desktop.Instance.Wallpaper = texture;
        }
        
        private void PopulateWallpapers()
        {
            //Getting all files from a directory
            DirectoryInfo directoryInfo = new("Assets/Images/Desktop/DesktopWallpapers");
            foreach (FileInfo file in directoryInfo.GetFiles("*.png"))
            {
                //Populating wallpaper paths
                _wallpapersPaths.Add(file.FullName);
            }   
        }
    }
}
