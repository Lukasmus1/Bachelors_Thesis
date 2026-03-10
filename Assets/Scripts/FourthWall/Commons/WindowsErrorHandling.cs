using System;
using System.Runtime.InteropServices;

namespace FourthWall.Commons
{
    /// <summary>
    /// This class provides methods to display Windows message boxes for errors, warnings, and informational messages.
    /// </summary>
    public class WindowsErrorHandling
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
        
        private const uint ERRICON = 0x10;
        private const uint WARNICON = 0x30;
        private const uint INFOICON = 0x40;

        public void ThrowError(string message, string title)
        {
            MessageBox(IntPtr.Zero, message, title, ERRICON);
        }

        public void ThrowWarning(string message, string title)
        {
            MessageBox(IntPtr.Zero, message, title, WARNICON);
        }

        public void ThrowInfo(string message, string title)
        {
            MessageBox(IntPtr.Zero, message, title, INFOICON);
        }
    }
    
    public enum DialogType
    {
        Error,
        Warning,
        Info
    }
}