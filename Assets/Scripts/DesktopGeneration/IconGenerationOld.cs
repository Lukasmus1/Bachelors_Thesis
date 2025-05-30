using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TMPro;
using UnityEngine;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Color = UnityEngine.Color;


namespace DesktopGeneration
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Shfileinfo
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }
    
    public class IconGenerationOld
    {
        
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, 
            ref Shfileinfo psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr handle);
        
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x000000002;
        
        private List<Sprite> _iconSprites;
        private List<GameObject> _bottomBarIconObjects;
        private List<GameObject> _desktopIconObjects;
        public IconGenerationOld(List<Sprite> iconSprites, List<GameObject> bottomBarIconObjects)
        {
            _iconSprites = iconSprites;
            _bottomBarIconObjects = bottomBarIconObjects;
        }

        public IconGenerationOld(List<GameObject> desktopIcons)
        {
            _desktopIconObjects = desktopIcons;
        }

        public string GenerateUserDesktopIcons()
        {
            //Getting the desktop directory
            DirectoryInfo directoryInfo = new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
            int iconIndex = 0;
            foreach (FileSystemInfo item in directoryInfo.GetFileSystemInfos())
            {
                _desktopIconObjects[iconIndex].GetComponentInChildren<TMP_Text>().text = item.Name;
                Transform[] images = _desktopIconObjects[iconIndex].GetComponentsInChildren<Transform>();
                foreach (Transform image in images)
                {
                    if (image.name == "Icon")
                    {
                        if (item is FileInfo)
                        {
                            Bitmap bitmap = null;
                            Shfileinfo shinfo = new Shfileinfo();
                            IntPtr hImgSmall = SHGetFileInfo(item.FullName, 0, ref shinfo, 
                                (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
                            
                            if (shinfo.hIcon != IntPtr.Zero)
                            {
                                try
                                {
                                    using (Icon icon = Icon.FromHandle(shinfo.hIcon))
                                    {
                                        bitmap = new Bitmap(icon.ToBitmap());
                                    }
                                }
                                finally
                                {
                                    DestroyIcon(shinfo.hIcon);
                                }
                            }
                            
                            using (var ms = new MemoryStream())
                            {
                                if (bitmap == null)
                                {
                                    Debug.LogError(item.FullName);
                                    continue;
                                }
                                bitmap.Save(ms, ImageFormat.Png);
                                byte[] bytes = ms.ToArray();

                                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                                texture.LoadImage(bytes);

                                image.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(
                                    texture,
                                    new Rect(0, 0, texture.width, texture.height),
                                    new Vector2(0.5f, 0.5f)
                                );
                            }
                            
                            break;
                        }
                        else if (item is DirectoryInfo)
                        {
                            //tbd
                        }
                    }
                }
                _desktopIconObjects[iconIndex].SetActive(true);

                iconIndex++;
            }
            
            return "";
        }
    }
}
