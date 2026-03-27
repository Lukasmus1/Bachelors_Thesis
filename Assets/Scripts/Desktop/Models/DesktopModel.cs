using System;
using System.Collections.Generic;
using Desktop.Commons;
using TMPro;
using UnityEngine;
using User.Commons;

namespace Desktop.Models
{
    [Serializable]
    public class DesktopModel
    {
        //Singleton instance
        private static DesktopModel _instance;
        public static DesktopModel Instance
        {
            get
            {
                _instance ??= new DesktopModel();
                return _instance;
            }
            set => _instance = value;
        }
        
        //Public
        public string wallpaperName;

        /// <summary>
        /// Returns the current wallpaper texture based on the wallpaper name stored in the model.
        /// </summary>
        /// <returns>Texture2D of the current wallpaper</returns>
        public Texture2D GetCurrentWallpaper()
        {
            var wallpaper = Resources.Load<Sprite>("wallpapers/" + wallpaperName);
            
            return wallpaper.texture;
        }

        /// <summary>
        /// Gets a specific wallpaper by its name.
        /// </summary>
        /// <param name="desiredWallpaper">Wallpaper name</param>
        /// <returns>Texture2D of the desired wallpaper</returns>
        /// <exception cref="KeyNotFoundException">Gets thrown if the wallpaper is not found</exception>
        public Texture2D GetWallpaper(string desiredWallpaper)
        {
            var wallpaper = Resources.Load<Sprite>("wallpapers/" + desiredWallpaper);
            return wallpaper.texture ?? throw new KeyNotFoundException($"Wallpaper {desiredWallpaper} not found");
        }
        
        private string colorScheme;
        /// <summary>
        /// Gets the current set color scheme.
        /// </summary>
        /// <returns>Current color scheme string</returns>
        public string GetColorScheme()
        {
            if (string.IsNullOrEmpty(colorScheme))
            {
                //Random default color
                colorScheme = "#E7B2FF";
            }
            return colorScheme;
        }
        
        /// <summary>
        /// Sets a new color scheme
        /// </summary>
        /// <param name="color">String of the color</param>
        public void SetColorScheme(string color)
        {
            if (!color.StartsWith("#"))
            {
                color = "#" + color;
            }
            
            colorScheme = color;
        }
        
        public List<IconClass> Icons { get; set; } = new();
        public Dictionary<string, bool> flags = new();

        public void ToggleIcon(string name, bool value)
        {
            IconClassOnObject icon = DesktopMvc.Instance.DesktopGeneratorController.Icons.Find(i => i.IconName == name);
            if (icon == null)
            {
                throw new KeyNotFoundException($"Icon {name} not found");
            }
            
            icon.IsActive = value;
        }
        
        public void SetFlag(string name, bool value)
        {
            if (flags.ContainsKey(name))
            {
                flags[name] = value;
            }
            else
            {
                flags.Add(name, value);
            }
        }
    }
}
