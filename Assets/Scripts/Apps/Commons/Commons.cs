using UnityEngine;

namespace Apps.Commons
{
    public class Commons : MonoBehaviour
    {
        public void CloseApp(GameObject appToDestroy)
        {
            appToDestroy.SetActive(false);
        }
        
        public void OpenApp(GameObject appToOpen)
        {
            appToOpen.SetActive(true);
            AppCommonsModel.Instance.OnAppOpened(appToOpen.name);
        }
    }
}
