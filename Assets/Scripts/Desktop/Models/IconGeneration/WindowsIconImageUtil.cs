using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Desktop.Models.IconGeneration
{
    public static class WindowsIconImageUtil
    {
        //Import the necessary Windows API functions
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            IntPtr pbc,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            out IShellItem ppv);

        [DllImport("gdi32.dll")]
        private static extern int GetObject(IntPtr hObject, int nCount, out BITMAP lpObject);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
    
        [ComImport]
        [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItemImageFactory
        {
            void GetImage(
                SIZE size,
                SIIGBF flags,
                out IntPtr phbm);
        }

        [ComImport]
        [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem { }

        [StructLayout(LayoutKind.Sequential)]
        private struct SIZE
        {
            public int cx;
            public int cy;
        }
    
        [Flags]
        private enum SIIGBF
        {
            SIIGBF_RESIZETOFIT = 0x00,
            SIIGBF_BIGGERSIZEOK = 0x01,
            SIIGBF_MEMORYONLY = 0x02,
            SIIGBF_ICONONLY = 0x04,
            SIIGBF_THUMBNAILONLY = 0x08,
            SIIGBF_INCACHEONLY = 0x10,
        }
    
        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAP
        {
            public int bmType;
            public int bmWidth;
            public int bmHeight;
            public int bmWidthBytes;
            public ushort bmPlanes;
            public ushort bmBitsPixel;
            public IntPtr bmBits;
        }

        public static Texture2D GetFileIcon(string path, int size = 48)
        {
            Guid iidShellItem = new("43826d1e-e718-42ee-bc55-a1e261c37bfe");
            try
            {
                //Shell item image factory
                SHCreateItemFromParsingName(path, IntPtr.Zero, iidShellItem, out IShellItem shellItem);
                IShellItemImageFactory factory = (IShellItemImageFactory)shellItem;
            
                //Getting the icon and saving the pointer to a IntPtr
                factory.GetImage(new SIZE { cx = size, cy = size }, SIIGBF.SIIGBF_ICONONLY, out IntPtr hBitmap);

                try
                {
                    //Getting the bitmap of the icon
                    GetObject(hBitmap, Marshal.SizeOf<BITMAP>(), out BITMAP bmp);
                
                    //This could be copied easily using ToBitmap() method, but that doesn't save the bitmap with alpha channel
                    //Getting the number of bytes needed for one row of pixels
                    int row = bmp.bmWidth * 4;
                
                    //Allocating a byte array to hold the pixel data
                    byte[] pixels = new byte[row * Math.Abs(bmp.bmHeight)];
                
                    //Copy the pixel data from the bitmap to the byte array
                    Marshal.Copy(bmp.bmBits, pixels, 0, pixels.Length);

                
                    //Swap BGRA to RGBA
                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        (pixels[i], pixels[i + 2]) = (pixels[i + 2], pixels[i]);
                    }

                    //Creating a Texture2D from the pixel data
                    var tex = new Texture2D(bmp.bmWidth, Math.Abs(bmp.bmHeight), TextureFormat.RGBA32, false);
                    tex.LoadRawTextureData(pixels);
                    tex.Apply();
                    return tex;
                }
                finally
                {
                    //This method requires the caller to delete the bitmap object
                    DeleteObject(hBitmap);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("IShellItemImageFactory failed: " + ex.Message);
                return null;
            }
        }
    }
}