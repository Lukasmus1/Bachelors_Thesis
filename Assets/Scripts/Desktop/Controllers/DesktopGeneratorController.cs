using System;
using System.Collections.Generic;
using System.Linq;
using Desktop.Abstracts;
using Desktop.Models;
using Desktop.Models.IconGeneration;
using Desktop.Views;
using TMPro;
using UnityEngine;

namespace Desktop.Controllers
{
    public class DesktopGeneratorController
    {
        private WallpaperGeneration _wallpaperGenerator;
        private ColorGeneration _colorSchemeGenerator;
        private IconGenerator _iconGenerator;
        private readonly IconGenerationHelper _iconGeneratorHelper = new();
        private DesktopGeneratorView _desktopGeneratorView;

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

        public Vector2 GenerateRandomIconPosition(Vector2 iconSize)
        {
            return _iconGeneratorHelper.GenerateRandomIconPosition(iconSize);
        }
        
        /// <summary>
        /// This is a helper for clearing flags. Used when loading a save.
        /// </summary>
        public static void ClearFlags()
        {
            DesktopModel.Instance.flags.Clear();
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
        
        /// <summary>
        /// Sets a flag for each app, so I can't open it twice for example.
        /// </summary>
        /// <param name="flag">Name of the app</param>
        /// <param name="value">True if app is open, False if not</param>
        public void SetDesktopFlag(string flag, bool value)
        {
            DesktopModel.Instance.SetFlag(flag, value);
        }
        
        /// <summary>
        /// Saves the icon into the desktop model for persistence.
        /// </summary>
        /// <param name="icon">The icon to save</param>
        public void SetDesktopIconIntoContext(IconClassOnObject icon)
        {
            IconClass iconClass = DesktopModel.Instance.Icons.FirstOrDefault(x => x.Name == icon.IconName);
            if (iconClass != null)
            {
                DesktopModel.Instance.Icons.Remove(iconClass);
            }
            
            DesktopModel.Instance.Icons.Add(new IconClass(icon));
        }

        public void SetDesktopView(DesktopGeneratorView view)
        {
            _desktopGeneratorView = view;
        }
        
        /// <summary>
        /// Uses icon's IconClass.name to enable or disable the icon. 
        /// </summary>
        /// <param name="name">GameObject name</param>
        /// <param name="active">Should it be enabled?</param>
        public void ToggleIcon(string name, bool active)
        {
            DesktopModel.Instance.ToggleIcon(name, active);
            
            _desktopGeneratorView.ToggleIcon(name, active);
        }
    }
}