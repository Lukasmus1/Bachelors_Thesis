using System;

namespace FourthWall.Commons
{
    public class CommonsController
    {
        private readonly WindowsErrorHandling _winErrorHandling = new();
        private readonly DesktopManipulation _desktopManipulation = new();
        private readonly ClipboardHelper _clipboardHelper = new();
        
        /// <summary>
        /// Created a new windows dialog based on the specified type, message and title.
        /// </summary>
        /// <param name="dialogType">Type of the dialog</param>
        /// <param name="message">Message of the dialog window</param>
        /// <param name="title">Title of the window</param>
        /// <exception cref="ArgumentOutOfRangeException">Gets thrown if unknown dialogTypeis used</exception>
        public void ThrowWindowsDialog(DialogType dialogType, string message, string title)
        {
            switch (dialogType)
            {
                case DialogType.Error:
                    _winErrorHandling.ThrowError(message, title);
                    break;
                case DialogType.Info:
                    _winErrorHandling.ThrowInfo(message, title);
                    break;
                case DialogType.Warning:
                    _winErrorHandling.ThrowWarning(message, title);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null);
            }
        }

        /// <summary>
        /// Minimizes all windows by simulating the "Win + D" keyboard shortcut.
        /// </summary>
        public void MinimizeAllWindows()
        {
            _desktopManipulation.MinimizeAllWindows();
        }
        
        /// <summary>
        /// Copies the given argument string to the clipboard.
        /// </summary>
        /// <param name="text">Text to copy to the clipboard</param>
        public void CopyToClipboard(string text)
        {
            _clipboardHelper.CopyToClipboard(text);
        }
    }
}