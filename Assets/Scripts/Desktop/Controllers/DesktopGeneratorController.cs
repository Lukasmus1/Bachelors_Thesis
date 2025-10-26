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

        //Commons
        public void SetDesktopFlag(string flag, bool value)
        {
            DesktopModel.Instance.SetFlag(flag, value);
        }
        
        public void SetDesktopIconIntoContext(IconClassOnObject icon)
        {
            //In this instance, we immediately convert the texture right back to byte[]
            //This is done 'cause I don't want to change all other instances that rely on this implementation
            var tex = new Texture2D(2, 2);
            tex.LoadImage(icon.Image);
            
            DesktopModel.Instance.Icons.Add(new IconClass(
                icon.Name,
                icon.Size,
                icon.Position,
                tex
            ));
        }
    }
}