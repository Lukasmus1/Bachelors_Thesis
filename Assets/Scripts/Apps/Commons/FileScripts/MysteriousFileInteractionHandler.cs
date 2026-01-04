using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Apps.Commons.FileScripts
{
    public class MysteriousFileInteractionHandler : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text _textComponent;
        
        public static Action ClickedOnLink;
        
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
                
                if (linkId == "OnClick")
                {
                    Debug.Log("OnClick");
                    
                    ClickedOnLink?.Invoke();
                }
            }
        }
    }
}
