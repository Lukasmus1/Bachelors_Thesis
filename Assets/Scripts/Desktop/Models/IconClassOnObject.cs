using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Desktop.Models
{
    public class IconClassOnObject : MonoBehaviour
    {
        public string IconName { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public byte[] Image { get; set; }
        public TMP_FontAsset Font { get; set; }

        private RectTransform _transformComponent;

        private void OnEnable()
        {
            _transformComponent = GetComponent<RectTransform>();
            var textComponent = GetComponentInChildren<TMP_Text>();
            IconName = textComponent.text;
            Size = new Vector2(_transformComponent.localScale.x, _transformComponent.localScale.y);
            Position = _transformComponent.anchoredPosition;
            Image = (GetComponentInChildren<RawImage>().texture as Texture2D).EncodeToPNG();
            Font = textComponent.font;
            
            GetComponent<Apps.Commons.DragAndDrop>().OnEndDragged += UpdatePosition;
        }

        private void OnDisable()
        {
            GetComponent<Apps.Commons.DragAndDrop>().OnEndDragged -= UpdatePosition;
        }

        private void UpdatePosition()
        {
            Position = _transformComponent.anchoredPosition;
        }
    }
}