using UnityEngine;

namespace DesktopGeneration
{
    public class Desktop
    {
        //Singleton instance
        private static Desktop _instance;
        public static Desktop Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Desktop();
                }
                return _instance;
            }
        }
        
        //Public
        public Texture2D Wallpaper { get; set; }
        public Color ColorScheme { get; set; }
        
        //Private
        
    }
}
