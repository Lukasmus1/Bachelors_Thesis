using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DesktopGeneration.Abstracts;
using Microsoft.Win32;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace DesktopGeneration
{
    public class IconGenerationUser : IconGeneration
    {
        private static readonly Dictionary<string, Guid> SpecialIcons = new Dictionary<string, Guid>
        {
            { "This PC", KnownFolder.ThisPC },
            { "Network", KnownFolder.Network },
            { "Recycle Bin", KnownFolder.RecycleBin },
            { "User Files", KnownFolder.UserFiles },
            { "Control Panel", KnownFolder.ControlPanel }
        };

        public IconGenerationUser(List<GameObject> desktopIcons) : base(desktopIcons)
        {
        }

        public override void GenerateIcons()
        {
            List<string> allFiles = new();
    
            //Desktop of current user
            string userDesktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            
            //Adding files and directories from the user's desktop
            allFiles.AddRange(Directory.GetFiles(userDesktop));
            allFiles.AddRange(Directory.GetDirectories(userDesktop));
            
            CheckForSpecialIcons(allFiles);
    
            //Shared desktop for all users
            string commonDesktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDesktopDirectory);
            if (Directory.Exists(commonDesktop))
            {
                allFiles.AddRange(Directory.GetFiles(commonDesktop));
            }
    
            //Remopving duplicit files
            allFiles = allFiles.Distinct().ToList();
            
            //Removing desktop.ini files (hidden config folder file)
            allFiles.Remove(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\desktop.ini");
            allFiles.Remove(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDesktopDirectory) + @"\desktop.ini");
            
            for (int iconIndex = 0; iconIndex < allFiles.Count; iconIndex++)
            {
                string iconName;
                
                //Checking if the icon is a special icon
                iconName = allFiles[iconIndex].StartsWith("::") ? GetSpecialIconName(allFiles[iconIndex]) : Path.GetFileNameWithoutExtension(allFiles[iconIndex]);
                
                //Setting the text of an icon
                DesktopIconObjects[iconIndex].GetComponentInChildren<TMP_Text>().text = iconName;
                
                //Getting the icon image
                Transform[] images = DesktopIconObjects[iconIndex].GetComponentsInChildren<Transform>();
                foreach (Transform image in images)
                {
                    if (image.name != "Icon")
                    {
                        continue;
                    }
                    
                    //Setting the icon image
                    image.GetComponent<RawImage>().texture = WindowsIconUtil.GetFileIcon(allFiles[iconIndex]);
                    break;
                }
                DesktopIconObjects[iconIndex].SetActive(true);
            }
        }

        private static void CheckForSpecialIcons(List<string> allFiles)
        {
            const string keyPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel";
            foreach (var icon in SpecialIcons)
            {
                Debug.Log((int)Registry.GetValue(keyPath, icon.Value.ToString("B"), 0));
                if ((int)Registry.GetValue(keyPath, icon.Value.ToString("B"), 0) == 0)
                {
                    allFiles.Add("::" + icon.Value.ToString("B"));
                }
            }
        }
        
        private static string GetSpecialIconName(string stringGuid)
        {
            foreach (KeyValuePair<string, Guid> icon in SpecialIcons)
            {
                if (("::" + icon.Value.ToString("B")).Equals(stringGuid))
                {
                    return icon.Key;
                }
            }
            
            return "Unknown Icon";
        }
    }
}
