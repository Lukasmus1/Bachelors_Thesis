using System.Collections;
using UnityEngine;

namespace Apps.Commons
{
    public class Commons : MonoBehaviour
    {
        public void CloseApp(GameObject appToDestroy)
        {
            StartCoroutine(FadeOutCoroutine());
            Destroy(appToDestroy);
        }

        private IEnumerator FadeOutCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            
        }
        
        public void OpenApp()
        {
            
        }
    }
}
