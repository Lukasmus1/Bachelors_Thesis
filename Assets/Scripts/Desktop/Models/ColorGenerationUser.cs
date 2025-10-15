using Microsoft.Win32;
using UnityEngine;

namespace Desktop.Models
{
    public class ColorGenerationUser : ColorGeneration
    {
        public override Color GenerateUserColorScheme()
        {
            //Reading the accent color from the Windows registry
            object regValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", null);
            
            //Converting the int registry value to a hex color string
            string hexColor = ((int)regValue).ToString("X")[2..];

            //Parsing the hex color to a Color object
            return ColorUtility.TryParseHtmlString($"#{hexColor}", out Color color) ? 
                color : 
                GenerateRandomColorScheme();
        }
    }
}