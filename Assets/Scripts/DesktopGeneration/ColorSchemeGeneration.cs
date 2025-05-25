using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace DesktopGeneration
{
    public class ColorSchemeGeneration
    {
        private Image _bottomBarBackground; 
            
        public ColorSchemeGeneration(Image bottomBarBackground)
        {
            _bottomBarBackground = bottomBarBackground;
        }

        public void GenerateUserColorScheme()
        {
            //Reading the accent color from the Windows registry
            object regValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "AccentColor", null);
            
            //Converting the int registry value to a hex color string
            string hexColor = new(((int)regValue).ToString("X").Reverse().ToArray());
            
            //Parsing the hex color to a Color object
            if (ColorUtility.TryParseHtmlString($"#{hexColor}", out Color color))
            {
                Debug.Log(hexColor);
                SetColorScheme(color);
            }
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
