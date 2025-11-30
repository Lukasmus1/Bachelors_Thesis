using Desktop.Models;
using UnityEngine;

namespace Desktop.Views
{
    public class AppIconAction : MonoBehaviour, IIconAction
    {
        public GameObject appToOpen;
        
        public void PerformAction()
        {
            if (appToOpen == null)
            {
                return;
            }
            
            //Check if the app is already open using the Flags dictionary and the gameObject tag
            if (!DesktopModel.Instance.flags.ContainsKey(appToOpen.tag) || 
                (!DesktopModel.Instance.flags[appToOpen.tag] &&
                DesktopModel.Instance.flags.ContainsKey(appToOpen.tag)))
            {
                appToOpen.SetActive(true);
            }
        }
    }
}