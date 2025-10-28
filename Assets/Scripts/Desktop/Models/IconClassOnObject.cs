using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Desktop.Models
{
    public class IconClassOnObject : MonoBehaviour
    {
        public string IconName { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Position { get; set; }
        public byte[] Image { get; set; }
        public TMP_FontAsset Font { get; set; }

        private void OnEnable()
        {
            var transformComponent = GetComponent<RectTransform>();
            var textComponent = GetComponentInChildren<TMP_Text>();
            IconName = textComponent.text;
            Scale = new Vector2(transformComponent.localScale.x, transformComponent.localScale.y);
            Position = transformComponent.anchoredPosition;
            Image = (GetComponentInChildren<RawImage>().texture as Texture2D).EncodeToPNG();
            Font = textComponent.font;
        }
    }
}