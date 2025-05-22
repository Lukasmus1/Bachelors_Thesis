using UnityEngine;
using UnityEngine.UI;

namespace DesktopGeneration
{
    public class ColorSchemeGeneration
    {
        private Image _bottomBarBackground; 
            
        public ColorSchemeGeneration(Image bottomBarBackground)
        {
            _bottomBarBackground = bottomBarBackground;
        }
        
        public void GenerateColorScheme()
        {
            //Generating a random color
            Color randomColor = Random.ColorHSV();
            randomColor.a = 0.95f;
            
            //Setting the color scheme
            Desktop.Instance.ColorScheme = randomColor;
            
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
