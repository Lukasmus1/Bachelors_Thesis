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

        //Offsetting the drag to match screen resolution
        //The implementation assumes a 16:9 aspect ratio with a height of 1440 pixels (set in canvas)
        private float _deltaOffset;
        
        public event Action OnEndDragged;
        
        private void Awake()
        {
            _deltaOffset = 1440f / Screen.height;
            
            print(_deltaOffset);
            
            if (objectToDrag == null)
            {
                throw new NullReferenceException("Parent to drag and hold is not assigned.");
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (objectToDrag != null)
            {
                objectToDrag.anchoredPosition += eventData.delta * _deltaOffset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragged?.Invoke();
        }
    }
}
