using UnityEngine;

namespace FourthWall.Commons
{
    public class ClipboardHelper
    {
        public void CopyToClipboard(string text)
        {
            var textEditor = new TextEditor { text = text };
            textEditor.SelectAll();
            textEditor.Copy();
        }
    }
}