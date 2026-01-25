using Desktop.BottomBar.Commons;
using Desktop.BottomBar.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Desktop.BottomBar.Views
{
    public class BottomBarItemView : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        
        private readonly BottomBarItemModel _model = new();

        public void SetProps(GameObject openedApp)
        {
            Sprite icon = IconImageLinker.appIconDictionary[openedApp.tag];
            
            if (icon == null)
            {
                Debug.LogError("Icon image not found");
                return;
            }
            
            iconImage.sprite = icon;
            BottomBarMvc.Instance.BottomBarController.SetPropsToModel(_model, openedApp);
        }
        
    }
}