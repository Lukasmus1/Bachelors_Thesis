using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Apps.ChatTerminal.Views
{
    public class ChoiceView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        [SerializeField] private GameObject choiceBubble;
        
        private Transform bubbleParent;
        private GameObject instantiatedBubble;
        private string choiceText;

        private readonly Vector2 bubbleOffset = new Vector2(20, -10);
        
        /// <summary>
        /// Sets the parent of the choice bubble, which is used to position the bubble correctly in the UI.
        /// </summary>
        /// <param name="parent">Transform parent for the choice bubble</param>
        public void SetBubbleParent(Transform parent)
        {
            bubbleParent = parent;
        }

        /// <summary>
        /// Sets the text of the choice view, which is displayed to the user as a selectable option in the chat terminal.
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            GetComponentInChildren<TMP_Text>().text = text;
            choiceText = text;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            instantiatedBubble = Instantiate(choiceBubble, bubbleParent);
            instantiatedBubble.GetComponentInChildren<TMP_Text>().text = choiceText;

            //Set the pivot to upper left corner
            var bubbleRect = instantiatedBubble.GetComponent<RectTransform>();
            bubbleRect.pivot = new Vector2(0, 1); 

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                bubbleParent.GetComponent<RectTransform>(),
                eventData.position + bubbleOffset,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            bubbleRect.localPosition = localPoint;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (instantiatedBubble != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    bubbleParent.GetComponent<RectTransform>(),
                    eventData.position + bubbleOffset,
                    eventData.pressEventCamera,
                    out Vector2 localPoint
                );

                instantiatedBubble.GetComponent<RectTransform>().localPosition = localPoint;
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            Destroy(instantiatedBubble);
        }
    }
}