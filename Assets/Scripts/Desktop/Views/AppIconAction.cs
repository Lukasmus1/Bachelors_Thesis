using Desktop.Models;
using UnityEngine;

namespace Desktop.Views
{
    public class AppIconAction : MonoBehaviour, IIconAction
    {
        [SerializeField] private GameObject appToOpen;
        
        public void PerformAction()
        {
            if (appToOpen == null)
            {
                return;
            }
            
            //Check if the app is already open using the Flags dictionary and the gameObject tag
            if (!DesktopModel.Instance.Flags.ContainsKey(appToOpen.tag) || 
                (!DesktopModel.Instance.Flags[appToOpen.tag] &&
                DesktopModel.Instance.Flags.ContainsKey(appToOpen.tag)))
            {
                Instantiate(appToOpen, gameObject.transform.parent.parent);    
            }
        }
    }
}