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
            //Generating random color
            Color clr = Random.ColorHSV();
            
            return clr;
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