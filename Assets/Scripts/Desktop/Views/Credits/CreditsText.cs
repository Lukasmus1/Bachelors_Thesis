using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Desktop.Views.Credits
{
    public class CreditsText : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            string link = gameObject.GetComponent<TMP_Text>().text;

            var popup = CreditsScript.OpenLinkPopup.GetComponent<OpenLinkPopup>();
            
            popup.Link = link;
        }
    }
}