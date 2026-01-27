using Desktop.BottomBar.Commons;
using Desktop.BottomBar.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Desktop.BottomBar.Views
{
    public class BottomBarItemView : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        public GameObject iconHighlight;
        
        private readonly BottomBarItemModel _model = new();

        public void SetProps(GameObject openedApp)
        {
            Sprite icon = IconImageLinker.AppIconDictionary[openedApp.tag];
            
            if (icon == null)
            {
                Debug.LogError("Icon image not found");
                return;
            }
            
            iconImage.sprite = icon;
            BottomBarMvc.Instance.BottomBarController.SetPropsToModel(_model, openedApp);
        }

        public void OnClick()
        {
            GameObject app = BottomBarMvc.Instance.BottomBarController.GetOpenedApp(_model);
            app.transform.SetAsLastSibling();
            
            BottomBarMvc.Instance.BottomBarController.HighlightIcon(gameObject);
        }
    }
}