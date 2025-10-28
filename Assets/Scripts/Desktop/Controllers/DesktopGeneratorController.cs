using System.Collections.Generic;
using System.Linq;
using Desktop.Abstracts;
using Desktop.Models;
using Desktop.Models.IconGeneration;
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

        //Random Generators
        public Texture2D GetRandomWallpaper()
        {
            _wallpaperGenerator = new WallpaperGeneratorRandom();
            return _wallpaperGenerator.GetWallpaperTexture();
        }
        
        public Color GetRandomColorScheme()
        {
            _colorSchemeGenerator = new ColorGeneration();
            return _colorSchemeGenerator.GenerateRandomColorScheme();
        }
        
        //User's Generators
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
        
        public List<IconClass> GetUserIcons(Vector3 prefabScale)
        {
            _iconGenerator = new IconGeneratorUser();
            return _iconGenerator.GenerateIcons(prefabScale);
        }

        public TMP_FontAsset GetUserFont()
        {
            var fontScript = new FontScript();
            return fontScript.GetUserFont();
        }

        //Commons
        public void SetDesktopFlag(string flag, bool value)
        {
            DesktopModel.Instance.SetFlag(flag, value);
        }
        
        public void SetDesktopIconIntoContext(IconClassOnObject icon)
        {
            IconClass iconClass = DesktopModel.Instance.Icons.FirstOrDefault(x => x.Name == icon.IconName);
            if (iconClass != null)
            {
                DesktopModel.Instance.Icons.Remove(iconClass);
            }
            
            DesktopModel.Instance.Icons.Add(new IconClass(icon));
        }
    }
}