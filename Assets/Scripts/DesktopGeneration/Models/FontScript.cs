using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using TMPro;
using UnityEngine;

namespace DesktopGeneration.Models
{
    public class FontScript
    {
        public TMP_FontAsset GetUserFont()
        {
            return CreateFont(GetUserFontPath());
        }

        private TMP_FontAsset CreateFont(string userFontPath)
        {
            UnityEngine.Font font = new(userFontPath);
            var fontAsset = TMP_FontAsset.CreateFontAsset(font);
            
            //Font settings
            //Dilate
            fontAsset.material.EnableKeyword("DILATE_ON");
            fontAsset.material.SetFloat(Shader.PropertyToID("_FaceDilate"), -0.15f);
            
            //Outline
            fontAsset.material.EnableKeyword("OUTLINE_ON");
            fontAsset.material.SetFloat(Shader.PropertyToID("_OutlineWidth"), 0.2f);
            fontAsset.material.SetColor(Shader.PropertyToID("_OutlineColor"), UnityEngine.Color.black);
            
            //Underlay
            fontAsset.material.EnableKeyword("UNDERLAY_ON");
            fontAsset.material.SetFloat(Shader.PropertyToID("_UnderlayOffsetX"), 1f);
            fontAsset.material.SetFloat(Shader.PropertyToID("_UnderlayOffsetY"), -1f);  
            fontAsset.material.SetFloat(Shader.PropertyToID("_UnderlayDilate"), 1f);
            fontAsset.material.SetFloat(Shader.PropertyToID("_UnderlaySoftness"), 0f);
            
            return fontAsset;
        }
        
        private static string GetUserFontPath()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
            if (key == null)
            {
                return "";
            }
                
            foreach (string valueName in key.GetValueNames())
            {
                if (!valueName.Contains(SystemFonts.DefaultFont.Name))
                {
                    continue;
                }
                    
                var fontFile = key.GetValue(valueName).ToString();
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), fontFile);
            }
            
            return "";
        }
    }
}
