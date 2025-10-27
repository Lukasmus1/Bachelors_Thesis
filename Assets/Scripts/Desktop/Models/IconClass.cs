using System;
using TMPro;
using UnityEngine;

namespace Desktop.Models
{
    [Serializable]
    public class IconClass
    {
        public string Name { get; set; }
        public float SizeX { get; set; }
        public float SizeY { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public byte[] Image { get; set; }
        
        public IconClass(string name, Vector2 size, Vector2 position, Texture2D image)
        {
            Name = name;
            SizeX = size.x;
            SizeY = size.y;
            PositionX = position.x;
            PositionY = position.y;
            Image = image.EncodeToPNG();
        }
    }
}