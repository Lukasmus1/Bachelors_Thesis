using Apps.Autospectogram.Models;
using TMPro;
using UnityEngine;

namespace Apps.Autospectogram.Controllers
{
    public class AutospectogramController
    {
        private readonly AutospectogramModel _autospectogramModel = new();
        
        /// <summary>
        /// Creates autospectogram. See <see cref="AutospectogramModel.CreateAutospectogram"/> for details.
        /// </summary>
        public Texture2D GenerateAutospectogram(string text, TMP_FontAsset font)
        {
            return _autospectogramModel.CreateAutospectogram(text, font);
        }
    }
}