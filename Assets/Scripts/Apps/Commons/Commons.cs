using UnityEngine;
using UnityEngine.EventSystems;

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
            appToOpen.transform.SetAsLastSibling();
            AppCommonsModel.Instance.OnAppOpened(appToOpen.name);
        }

        public void DestroyApp(GameObject appToDestroy)
        {
            Destroy(appToDestroy);
        }
    }
}
