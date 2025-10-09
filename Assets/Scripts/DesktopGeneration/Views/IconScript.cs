using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DesktopGeneration.Views
{
    public class IconScript : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image iconImage;
        
        //Even tho this script is on a button, I need to handle right clicks
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                // Check if the left mouse button was clicked
                case PointerEventData.InputButton.Left:
                    OnLeftClick();
                    break;
                // Check if the right mouse button was clicked
                case PointerEventData.InputButton.Right:
                    print("RIGHT MOUSE CLICKED");
                    break;
            }
        }

        public void SetProperties(Sprite iconSprite)
        {
            iconImage.sprite = iconSprite;
        }

        public void OnLeftClick()
        {
            print("LEFT MOUSE CLICKED");
        }
    }
}
