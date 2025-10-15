using System.Collections.Generic;
using Desktop.Abstracts;
using Desktop.Models;
using DesktopGeneration.Models;
using DesktopGeneration.Models.IconGeneration;
using TMPro;
using UnityEngine;

namespace Desktop.Controllers
{
    public class DesktopGeneratorController
    {
        private WallpaperGeneration _wallpaperGenerator;
        private ColorGeneration _colorSchemeGenerator;
        private IconGenerator _iconGenerator;
        
        public Texture2D GetUserWallpaper()
        {
            _wallpaperGenerator = new WallpaperGeneratorUser();
            return _wallpaperGenerator.GetWallpaperTexture();
        }

        public Color GetUserColorScheme()
        {
            _colorSchemeGenerator = new ColorGenerationUser();
            return _colorSchemeGenerator.GenerateUserColorScheme();
        }
        
        public List<IconClass> GetUserIcons()
        {
            _iconGenerator = new IconGeneratorUser();
            return _iconGenerator.GenerateIcons();
        }

        public TMP_FontAsset GetUserFont()
        {
            var fontScript = new FontScript();
            return fontScript.GetUserFont();
        }

        public void SetDesktopFlag(string flag, bool value)
        {
            DesktopModel.Instance.SetFlag(flag, value);
        }
    }
}