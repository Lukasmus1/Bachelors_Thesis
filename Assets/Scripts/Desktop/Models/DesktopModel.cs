using System;
using System.Collections.Generic;
using UnityEngine;

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
        public Texture2D Wallpaper { get; set; }
        public Color ColorScheme { get; set; }
        public List<IconClass> Icons { get; set; }
        public Dictionary<string, bool> Flags { get; set; } = new();

        
        public void SetFlag(string name, bool value)
        {
            if (Flags.ContainsKey(name))
            {
                Flags[name] = value;
            }
            else
            {
                Flags.Add(name, value);
            }
        }
    }
}
