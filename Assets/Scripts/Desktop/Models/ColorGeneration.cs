using UnityEngine;

namespace Desktop.Models
{
    public class ColorGeneration
    {
        /// <summary>
        /// Generates a random color scheme with slight transparency.
        /// </summary>
        /// <returns>Random color scheme</returns>
        public Color GenerateRandomColorScheme()
        {
            //Generating a random hue and random saturation with full brightness
            float h = Random.value;
            float s = Random.value;
            float v = 1;
            
            return Color.HSVToRGB(h, s, v);
        }

        /// <summary>
        /// Used only for user-specific color schemes. Override this to get different user color schemes.
        /// </summary>
        /// <returns>Random color scheme</returns>
        public virtual Color GenerateUserColorScheme()
        {
            return GenerateRandomColorScheme();
        }
    }
}