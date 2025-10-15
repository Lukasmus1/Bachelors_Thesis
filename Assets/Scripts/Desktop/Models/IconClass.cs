using TMPro;
using UnityEngine;

namespace Desktop.Models
{
    public class IconClass
    {
        public string Name { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Image { get; set; }
        public TMP_FontAsset Font { get; set; }
        
        public IconClass(string name, Vector2 size, Vector2 position, Texture2D image)
        {
            Name = name;
            Size = size;
            Position = position;
            Image = image;
        }
    }
}