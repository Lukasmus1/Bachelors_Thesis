using System;
using System.Runtime.InteropServices;

namespace FourthWall.FileGeneration.Models
{
    /// <summary>
    /// This class provides methods to display Windows message boxes for errors, warnings, and informational messages.
    /// </summary>
    public static class WindowsErrorHandling
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
        
        private const uint ERRICON = 0x10;
        private const uint WARNICON = 0x30;
        private const uint INFOICON = 0x40;

        public static void ThrowError(string message, string title)
        {
            MessageBox(IntPtr.Zero, message, title, ERRICON);
        }

        public static void ThrowWarning(string message, string title)
        {
            MessageBox(IntPtr.Zero, message, title, WARNICON);
        }

        public static void ThrowInfo(string message, string title)
        {
            MessageBox(IntPtr.Zero, message, title, INFOICON);
        }
    }
}