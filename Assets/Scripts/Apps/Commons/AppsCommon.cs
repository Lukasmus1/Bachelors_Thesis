using Desktop.BottomBar.Commons;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Apps.Commons
{
    public class AppsCommon : MonoBehaviour, IPointerClickHandler
    {
        public GameObject BottomBarIcon {get; set;}
        
        protected void DeleteBottomBarIcon()
        {
            if (BottomBarIcon != null)
            {
                Destroy(BottomBarIcon);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
            
            BottomBarMvc.Instance.BottomBarController.HighlightIcon(BottomBarIcon);
        }
    }
}