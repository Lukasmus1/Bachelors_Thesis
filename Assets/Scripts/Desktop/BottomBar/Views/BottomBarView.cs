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

            return icon;
        }
    }
}