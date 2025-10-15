using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Apps.Commons
{
    /// <summary>
    /// Class to handle drag and drop functionality for UI elements.
    /// </summary>
    public class DragAndDrop : MonoBehaviour, IDragHandler
    {
        [SerializeField] private RectTransform objectToDrag;

        private void Awake()
        {
            if (objectToDrag == null)
            {
                throw new NullReferenceException("Parent to drag and hold is not assigned.");
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (objectToDrag != null)
            {
                objectToDrag.anchoredPosition += eventData.delta;
            }
        }
    }
}
