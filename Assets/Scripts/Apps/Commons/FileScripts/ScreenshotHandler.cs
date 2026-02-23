using UnityEngine;
using UnityEngine.UI;
using User.Commons;

namespace Apps.Commons.FileScripts
{
    public class ScreenshotHandler : MonoBehaviour
    {
        private Sprite _desktopScreenshot;
        
        public void SetScreenshot()
        {
            GetComponentInChildren<Image>().sprite = _desktopScreenshot == null ? UserMvc.Instance.UserController.GetScreenshot() : _desktopScreenshot;
        }
    }
}