using System;
using Apps.Commons;
using Desktop.Commons;

namespace Apps.FileUploader.Views
{
    public class FileUploaderView : AppsCommon
    {
        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }

        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }
    }
}