using System.Collections.Generic;
using System.IO;
using DesktopGeneration.Abstracts;
using TMPro;
using UnityEngine;

namespace DesktopGeneration
{
    public class IconGenerationUser : IconGeneration
    {
        public IconGenerationUser(List<GameObject> desktopIcons) : base(desktopIcons)
        {
        }

        public override void GenerateIcons()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
            int iconIndex = 0;
            foreach (FileSystemInfo item in directoryInfo.GetFileSystemInfos())
            {
                //Setting the text of an icon
                DesktopIconObjects[iconIndex].GetComponentInChildren<TMP_Text>().text = item.Name;
                Transform[] images = DesktopIconObjects[iconIndex].GetComponentsInChildren<Transform>();
                foreach (Transform image in images)
                {
                    if (image.name == "Icon")
                    {
                        
                    }
                }
                DesktopIconObjects[iconIndex].SetActive(true);
                iconIndex++;
            }
        }
    }
}
