using System.Collections.Generic;
using System.IO;
using System.Linq;
using Desktop.Abstracts;
using UnityEngine;

namespace Desktop.Models
{
    public class WallpaperGeneratorRandom : WallpaperGeneration
    {
        private readonly List<Sprite> wallpapers = Resources.LoadAll<Sprite>("Wallpapers").ToList();

        public override Texture2D GetWallpaperTexture()
        {
            return GetRandomPath().texture;
        }
        
        private Sprite GetRandomPath()
        {
            //Getting a random index 
            int randomIndex = Random.Range(0, wallpapers.Count);
            
            //Returning a random wallpaper path
            return wallpapers[randomIndex];
        }
    }
}
