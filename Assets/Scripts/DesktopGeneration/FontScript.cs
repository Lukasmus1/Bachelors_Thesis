using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using TMPro;
using UnityEngine;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;

namespace DesktopGeneration
{
    public class FontScript
    {
        private readonly string _userFontFile;
        private readonly List<GameObject> _desktopIconObjects;
        
        public FontScript(List<GameObject> desktopIconObjects)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts"))
            {
                if (key == null)
                {
                    return;
                }
                
                foreach (string valueName in key.GetValueNames())
                {
                    if (!valueName.Contains(SystemFonts.DefaultFont.Name))
                    {
                        continue;
                    }
                    
                    string fontFile = key.GetValue(valueName).ToString();
                    _userFontFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), fontFile);
                    break;
                }
            }
            
            _desktopIconObjects = desktopIconObjects;
        }

        public void SetUserFont()
        {
            UnityEngine.Font font = new(_userFontFile);
            TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(font);
            
            //Font settings
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
            
            foreach (GameObject desktopIconObject in _desktopIconObjects)
            {
                TMP_Text textComponent = desktopIconObject.GetComponentInChildren<TMP_Text>();
                textComponent.font = fontAsset;
            }
        }
    }
}
