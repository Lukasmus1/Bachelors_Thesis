using System;
using System.Runtime.InteropServices;

namespace FourthWall.Commons
{
    /// <summary>
    /// This class provides methods to manipulate the Windows desktop.
    /// </summary>
    public class DesktopManipulation
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const byte VK_LWIN = 0x5B;
        private const byte VK_D = 0x44;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        /// <inheritdoc cref="CommonsController.MinimizeAllWindows"/>
        public void MinimizeAllWindows()
        {
            keybd_event(VK_LWIN, 0, 0, UIntPtr.Zero);
            keybd_event(VK_D, 0, 0, UIntPtr.Zero);
            keybd_event(VK_D, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
    }
}