using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Desktop.Models;
using Desktop.Models.IconGeneration;
using Microsoft.Win32;
using UnityEngine;

namespace DesktopGeneration.Models.IconGeneration
{
    public class IconGeneratorUser : IconGenerator
    {
        private GameObject iconPrefab = Resources.Load<GameObject>("Prefabs/DesktopIconPrefab");
        
        private static readonly Dictionary<string, Guid> SpecialIcons = new()
        {
            { "This PC", KnownFolders.ThisPC },
            { "Network", KnownFolders.Network },
            { "Recycle Bin", KnownFolders.RecycleBin },
            { "User Files", KnownFolders.UserFiles },
            { "Control Panel", KnownFolders.ControlPanel }
        };

        public override List<IconClass> GenerateIcons(Vector3 prefabScale)
        {
            var completeIcons = new List<IconClass>();
            
            List<string> allFiles = new();
    
            //Desktop of current user
            string userDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            //Adding files and directories from the user's desktop
            allFiles.AddRange(Directory.GetFiles(userDesktop));
            allFiles.AddRange(Directory.GetDirectories(userDesktop));
            
            CheckForSpecialIcons(allFiles);
    
            //Shared desktop for all users
            string commonDesktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            if (Directory.Exists(commonDesktop))
            {
                allFiles.AddRange(Directory.GetFiles(commonDesktop));
            }
    
            //Remopving duplicit files
            allFiles = allFiles.Distinct().ToList();
            
            //Removing desktop.ini files (hidden config folder file)
            allFiles.Remove(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\desktop.ini");
            allFiles.Remove(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory) + @"\desktop.ini");

            //Getting the icon positions
            List<WindowsIconPositionUtil.DesktopIcon> iconPositions = WindowsIconPositionUtil.GetDesktopIconPositions();
            iconPositions.ForEach(s => Debug.Log(s));
            
            var icons = new List<IconClass>();
            
            for (int iconIndex = 0; iconIndex < allFiles.Count; iconIndex++)
            {
                //Checking if the icon is a special icon
                string iconName = allFiles[iconIndex].StartsWith("::") ? GetSpecialIconName(allFiles[iconIndex]) : Path.GetFileNameWithoutExtension(allFiles[iconIndex]);
                
                //Getting the icon position
                WindowsIconPositionUtil.DesktopIcon currentIcon = new();
                foreach (WindowsIconPositionUtil.DesktopIcon icon in iconPositions)
                {
                    //The WinApi sometimes returns the icon with its extension, sometimes without it
                    if (icon.Name.Equals(iconName) || icon.Name.Equals(Path.GetFileName(allFiles[iconIndex])))
                    {
                        currentIcon = icon;
                    }
                }
                
                //Getting the icon prefab default size to calculate the relative scale 
                //NewScale / OldScale 
                var iconRelativeScale = new Vector2(WindowsIconPositionUtil.IconSize.x / prefabScale.x, WindowsIconPositionUtil.IconSize.y / prefabScale.y);
                
                completeIcons.Add(new IconClass(
                    iconName,
                    iconRelativeScale,
                    new Vector2(currentIcon.Position.X, -currentIcon.Position.Y),
                    WindowsIconImageUtil.GetFileIcon(allFiles[iconIndex])));
                
                iconPositions.Remove(currentIcon); //Removing the icon for better performance in the next iteration
            }
            
            return completeIcons;
        }

        private static void CheckForSpecialIcons(List<string> allFiles)
        {
            const string keyPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel";
            foreach (var icon in SpecialIcons)
            {
                int registryValue;
                
                //Recycle bin default behavior is different, it is not hidden by default -> arg "defaultValue" of GetValue is 0
                if (icon.Key == "Recycle Bin")
                {
                    registryValue = (int)Registry.GetValue(keyPath, icon.Value.ToString("B"), 0);
                }
                else
                {
                    registryValue = (int)Registry.GetValue(keyPath, icon.Value.ToString("B"), 1);
                }
                
                if (registryValue == 0)
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
