using UnityEngine;

namespace Desktop.Views.Credits
{
    public class CreditsScript : MonoBehaviour
    {
        [SerializeField] private GameObject openLinkPopup;
        public static GameObject OpenLinkPopup;

        private void OnEnable()
        {
            OpenLinkPopup = openLinkPopup;
        }

        public void CloseCreditsPanel()
        {
            Destroy(gameObject);
        }
    }
}