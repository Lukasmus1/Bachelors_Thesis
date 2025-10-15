using System.Collections.Generic;
using UnityEngine;

namespace DesktopGeneration.Models
{
    public class Desktop
    {
        //Singleton instance
        private static Desktop _instance;
        public static Desktop Instance
        {
            get
            {
                _instance ??= new Desktop();
                return _instance;
            }
        }
        
        //Public
        public Texture2D Wallpaper { get; set; }
        public Color ColorScheme { get; set; }
        public List<IconClass> Icons { get; set; }
        
        
    }
}
