using Desktop.Commons;
using UnityEngine;

namespace Apps.Commons
{
    public class AppsCommon : MonoBehaviour
    {
        public GameObject BottomBarIcon {get; set;}
        
        protected void DeleteBottomBarIcon()
        {
            if (BottomBarIcon != null)
            {
                Destroy(BottomBarIcon);
            }
        }
    }
}