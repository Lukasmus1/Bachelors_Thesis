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
        public Sprite GenerateAutostereogram(string text, TMP_FontAsset font = null)
        {
            if (font == null)
            {
                font = TMP_Settings.defaultFontAsset;
            }

            Texture2D autostereogramTexture = autostereogramModel.CreateAutostereogram(text, font);
            return Sprite.Create(
                autostereogramTexture,
                new Rect(0, 0, autostereogramTexture.width, autostereogramTexture.height),
                new Vector2(0.5f, 0.5f)
            );
        }
    }
}