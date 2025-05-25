using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace DesktopGeneration
{
    public class ColorGenerationRandom : IColorGeneration
    {
        private Image _bottomBarBackground; 
            
        public ColorGenerationRandom(Image bottomBarBackground)
        {
            _bottomBarBackground = bottomBarBackground;
        }
        

        public void GenerateColorScheme()
        {
            SetColorScheme(Random.ColorHSV());
        }

        private void SetColorScheme(Color clr)
        {
            //Setting alpha value for the color
            clr.a = 0.97f;
            
            //Setting the color scheme
            Desktop.Instance.ColorScheme = clr;
            UpdateColorScheme();
        }
        
        private void UpdateColorScheme()
        {
            //Getting the current color scheme
            Color colorScheme = Desktop.Instance.ColorScheme;
            
            //Setting the color scheme
            _bottomBarBackground.color = colorScheme;
        }
    }
}
