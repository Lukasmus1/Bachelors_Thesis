using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Desktop.Models
{
    public class UserDesktopModel
    {
        public Texture2D Wallpaper { get; set; }
        public Color? ColorScheme { get; set; } = null;
        public List<IconClass> Icons { get; set; }
        public TMP_FontAsset Font { get; set; }
    }
}