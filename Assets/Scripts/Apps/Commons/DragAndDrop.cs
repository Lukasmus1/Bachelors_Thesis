using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Apps.Commons
{
    /// <summary>
    /// Class to handle drag and drop functionality for UI elements.
    /// </summary>
    public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform objectToDrag;
        public event Action OnEndDragged;
        
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

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragged?.Invoke();
        }
    }
}
