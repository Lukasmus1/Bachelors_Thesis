using System.Collections.Generic;
using Desktop.BottomBar.Commons;
using UnityEngine;

namespace Desktop.BottomBar.Views
{
    public class BottomBarView : MonoBehaviour
    {
        [SerializeField] private GameObject bottomBarItemPrefab;
        
        [SerializeField] private Transform bottomBarItemsParent;
        
        public GameObject CreateNewIcon(GameObject openedApp)
        {
            GameObject icon = Instantiate(bottomBarItemPrefab, bottomBarItemsParent);
            
            icon.GetComponent<BottomBarItemView>().SetProps(openedApp);
            
            icon.SetActive(true);
            
            BottomBarMvc.Instance.BottomBarController.AddIcon(icon);
            
            return icon;
        }

        public void HighlightIcon(GameObject icon)
        {
            Dictionary<GameObject, BottomBarItemView> newIcons = new();
            
            foreach (KeyValuePair<GameObject, BottomBarItemView> item in BottomBarMvc.Instance.BottomBarController.GetIcons())
            {
                if (item.Key == null)
                {
                    continue;
                }
                
                item.Value.iconHighlight.SetActive(false);
                newIcons.Add(item.Key, item.Value);
            }
            
            icon.GetComponent<BottomBarItemView>().iconHighlight.SetActive(true);
            
            BottomBarMvc.Instance.BottomBarController.SetIcons(newIcons);
        }
    }
}