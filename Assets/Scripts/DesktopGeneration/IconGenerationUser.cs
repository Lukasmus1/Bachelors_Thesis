using System.Collections.Generic;
using System.IO;
using System.Text;
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
            DirectoryInfo directoryInfo = new(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
            int iconIndex = 0;
            foreach (FileSystemInfo item in directoryInfo.GetFileSystemInfos())
            {
                //Removing the file extension from the name
                StringBuilder sb = new(item.Name);
                sb.Remove(item.Name.Length - item.Extension.Length, item.Extension.Length);
             
                //Setting the text of an icon
                DesktopIconObjects[iconIndex].GetComponentInChildren<TMP_Text>().text = sb.ToString();
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
