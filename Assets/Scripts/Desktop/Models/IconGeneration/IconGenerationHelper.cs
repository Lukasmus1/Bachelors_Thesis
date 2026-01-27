using UnityEngine;

namespace Desktop.Models.IconGeneration
{
    public class IconGenerationHelper
    {
        //Magic number -> the screen reference resolution is 2560x1440, minus the bottom bar height (55)
        private readonly Vector2 _screenBounds = new(2560, 1440 - 55);
        
        /// <summary>
        /// Generates a random position for an icon within the screen bounds, taking into account the icon size.
        /// </summary>
        /// <param name="iconSize">The size of the icon</param>
        /// <returns>Random position for the icon</returns>
        public Vector2 GenerateRandomIconPosition(Vector2 iconSize)
        {
            Vector2 iconHalfSize = iconSize / 2;
            float xPos = Random.Range(iconHalfSize.x, _screenBounds.x - iconHalfSize.x);
            float yPos = Random.Range(iconHalfSize.y, _screenBounds.y - iconHalfSize.y);
            
            return new Vector2(xPos, yPos);
        }
    }
}