using System.Collections;
using Desktop.Models;
using UnityEngine;

namespace Apps.Commons
{
    public class Commons : MonoBehaviour
    {
        public void CloseApp(GameObject appToDestroy)
        {
            appToDestroy.SetActive(false);
        }
        
        public void OpenApp()
        {
            
        }
    }
}
