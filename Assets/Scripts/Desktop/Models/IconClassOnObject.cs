using System;
using Desktop.Commons;
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
        public bool IsActive { get; set; }
        
        private RectTransform _transformComponent;

        public void InitProps()
        {
            _transformComponent = GetComponent<RectTransform>();
            var textComponent = GetComponentInChildren<TMP_Text>();
            IconName = textComponent.text;
            Size = new Vector2(_transformComponent.localScale.x, _transformComponent.localScale.y);
            Position = _transformComponent.anchoredPosition;
            Image = (GetComponentInChildren<RawImage>().texture as Texture2D).EncodeToPNG();
            Font = textComponent.font;
            IsActive = gameObject.activeSelf;
            
            GetComponent<Apps.Commons.DragAndDrop>().OnEndDragged += UpdatePosition;
        }

        public void SetProps(IconClass icon)
        {
            IconName = icon.Name;
            Size = new Vector2(icon.SizeX, icon.SizeY);
            Position = new Vector2(icon.PositionX, icon.PositionY);
            Image = icon.Image;
            Font = GetComponentInChildren<TMP_Text>().font;
            IsActive = icon.IsActive;
            
            DesktopMvc.Instance.DesktopGeneratorController.Icons.Add(this);
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