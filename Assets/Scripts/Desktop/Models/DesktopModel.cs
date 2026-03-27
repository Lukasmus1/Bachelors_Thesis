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

        public Texture2D GetWallpaper()
        {
            var wallpaper = Resources.Load<Sprite>("wallpapers/" + wallpaperName);
            
            return wallpaper.texture;

            // var texture = new Texture2D(2, 2);
            // texture.LoadImage(wallpaperName);
            // return texture;
        }
        
        private string colorScheme;
        public string GetColorScheme()
        {
            if (string.IsNullOrEmpty(colorScheme))
            {
                //Random default color
                colorScheme = "#E7B2FF";
            }
            return colorScheme;
        }
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
