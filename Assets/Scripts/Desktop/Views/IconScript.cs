using System.Collections;
using Desktop.Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Desktop.Views
{
    public class IconScript : MonoBehaviour, ISubmitHandler, IPointerClickHandler
    {
        private bool clickedOnce = false;
        private Coroutine doubleClickCoroutine;

        public void OnSubmit(BaseEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                PerformIconAction();
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (clickedOnce)
            {
                PerformIconAction();
                clickedOnce = false;
                if (doubleClickCoroutine != null)
                {
                    StopCoroutine(doubleClickCoroutine);
                }
            }
            
            if (doubleClickCoroutine != null)
            {
                StopCoroutine(doubleClickCoroutine);
            }
            StartCoroutine(DoubleClick());
        }

        private IEnumerator DoubleClick()
        {
            clickedOnce = true;
            yield return new WaitForSeconds(0.5f);
            clickedOnce = false;
        }

        public void SetProperties(IconClass icon)
        {
            GetComponent<RectTransform>().localScale = icon.Size;
            GetComponentInChildren<TMP_Text>().text = icon.Name;
            GetComponent<RectTransform>().anchoredPosition = icon.Position;
            GetComponentInChildren<RawImage>().texture = icon.Image;
            GetComponentInChildren<TMP_Text>().font = icon.Font;
        }

        private void PerformIconAction()
        { 
            GetComponent<IIconAction>().PerformAction();
        }
    }
}
