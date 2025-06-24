using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DesktopGeneration.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace DesktopGeneration
{
    public class IconGenerationUser : IconGeneration
    {
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
                if (Path.GetFileNameWithoutExtension(allFiles[iconIndex]) == "desktop")
                {
                    Debug.Log(true);
                }
                
                //Setting the text of an icon
                DesktopIconObjects[iconIndex].GetComponentInChildren<TMP_Text>().text = Path.GetFileNameWithoutExtension(allFiles[iconIndex]);
                
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
    }
}
