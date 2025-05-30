using Microsoft.Win32;
using UnityEngine;
using UnityEngine.UI;

namespace DesktopGeneration
{
    public class ColorGenerationUser : IColorGeneration
    {
        private Image _bottomBarBackground; 

        public ColorGenerationUser(Image bottomBarBackground)
        {
            _bottomBarBackground = bottomBarBackground;
        }
        
        public void GenerateColorScheme()
        {
            //Reading the accent color from the Windows registry
            object regValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", null);
            
            //Converting the int registry value to a hex color string
            string hexColor = ((int)regValue).ToString("X")[2..];
            
            //Parsing the hex color to a Color object
            if (ColorUtility.TryParseHtmlString($"#{hexColor}", out Color color))
            {
                //check, jeslti má user nastavené jinou barvu pro taskbar a cosi idk ->colorprevalence
                Debug.Log(hexColor);
                SetColorScheme(color);
            }
        }
        
        private void SetColorScheme(Color clr)
        {
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