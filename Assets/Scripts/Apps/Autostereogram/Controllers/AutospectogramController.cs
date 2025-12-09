using Apps.Autostereogram.Models;
using TMPro;
using UnityEngine;

namespace Apps.Autostereogram.Controllers
{
    public class AutospectogramController
    {
        private readonly AutostereogramModel autostereogramModel = new();
        
        /// <summary>
        /// Creates autospectogram. See <see cref="AutostereogramModel.CreateAutostereogram"/> for details.
        /// </summary>
        public Texture2D GenerateAutospectogram(string text, TMP_FontAsset font = null)
        {
            if (font == null)
            {
                font = TMP_Settings.defaultFontAsset;
            }
            return autostereogramModel.CreateAutostereogram(text, font);
        }
    }
}