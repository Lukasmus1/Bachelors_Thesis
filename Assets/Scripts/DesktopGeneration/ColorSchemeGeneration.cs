using System.Diagnostics;
using System.Linq;
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
        
        public void GenerateUserColorScheme()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "-NoProfile -Command \"(Get-ItemProperty 'HKCU:\\Software\\Microsoft\\Windows\\DWM').AccentColor\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            output = output.Trim();
            
            //ulong (32-bit) to hex conversion and reversing the string, 'cause PowerShell returns the color in opposite order
            string hexColor = new string(ulong.Parse(output).ToString("X").Reverse().ToArray());
            if (ColorUtility.TryParseHtmlString("#" + hexColor, out Color userColor))
            {
                SetColorScheme(userColor);
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
