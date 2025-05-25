using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using TMPro;
using UnityEngine;
using Font = System.Drawing.Font;

namespace DesktopGeneration
{
    public class FontScript
    {
        private string _userFontFile;
        private List<GameObject> _desktopIconObjects;
        
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
            
            foreach (GameObject desktopIconObject in _desktopIconObjects)
            {
                TMP_Text textComponent = desktopIconObject.GetComponentInChildren<TMP_Text>();
                textComponent.font = fontAsset;
            }
        }
    }
}
