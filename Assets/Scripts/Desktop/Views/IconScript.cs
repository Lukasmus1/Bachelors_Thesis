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
        private bool _clickedOnce = false;
        private Coroutine _doubleClickCoroutine;
        
        
        public void OnSubmit(BaseEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                PerformIconAction();
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_clickedOnce)
            {
                PerformIconAction();
                _clickedOnce = false;
                if (_doubleClickCoroutine != null)
                {
                    StopCoroutine(_doubleClickCoroutine);
                }
            }
            
            if (_doubleClickCoroutine != null)
            {
                StopCoroutine(_doubleClickCoroutine);
            }
            StartCoroutine(DoubleClick());
        }

        private IEnumerator DoubleClick()
        {
            _clickedOnce = true;
            yield return new WaitForSeconds(0.5f);
            _clickedOnce = false;
        }

        public void SetProperties(IconClass icon, TMP_FontAsset font)
        {
            var size = new Vector2(icon.SizeX, icon.SizeY);
            var position = new Vector2(icon.PositionX, icon.PositionY);
            
            GetComponent<RectTransform>().localScale = size;
            GetComponentInChildren<TMP_Text>().text = icon.Name;
            GetComponent<RectTransform>().anchoredPosition = position;
            
            var tex = new Texture2D(2, 2);
            tex.LoadImage(icon.Image);
            GetComponentInChildren<RawImage>().texture = tex;
            
            GetComponentInChildren<TMP_Text>().font = font;
        }

        public void SetProperties(Vector2 pos, string iconName)
        {
            GetComponentInChildren<TMP_Text>().text = iconName;
            GetComponent<RectTransform>().anchoredPosition = pos;
        }

        private void PerformIconAction()
        { 
            GetComponent<IIconAction>().PerformAction();
        }
    }
}
