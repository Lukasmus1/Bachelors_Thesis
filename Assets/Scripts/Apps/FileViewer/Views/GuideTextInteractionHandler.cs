using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Apps.FileViewer.Views
{
    public class GuideTextInteractionHandler : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text _textComponent;
        private bool _isRed = false;
        
        private void Awake()
        {
            _textComponent = GetComponentInChildren<TMP_Text>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textComponent, eventData.position, null);
            
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = _textComponent.textInfo.linkInfo[linkIndex];
                string linkId = linkInfo.GetLinkID();
                
                if (linkId == "ChangeColor")
                {
                    string text = _textComponent.text;
                    
                    int start = text.IndexOf("<color=", StringComparison.Ordinal);
                    int endIndex = text.IndexOf("</color>", start, StringComparison.Ordinal);
                    
                    string newLinkText = _isRed ? $"<color=Blue>{linkInfo.GetLinkText()}</color>" : $"<color=Red>{linkInfo.GetLinkText()}</color>";
                    _isRed = !_isRed;
                    
                    _textComponent.text = text.Remove(start, endIndex - start)
                        .Insert(start, newLinkText);
                }
            }
        }
    }
}
