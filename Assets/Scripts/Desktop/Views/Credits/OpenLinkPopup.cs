using TMPro;
using UnityEngine;

namespace Desktop.Views.Credits
{
    public class OpenLinkPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text linkTextLabel;

        private string _link;
        public string Link
        {
            set
            {
                _link = value;
                linkTextLabel.text = _link;
                gameObject.SetActive(true);
            }
        }
        
        public void ClosePopup()
        {
            gameObject.SetActive(false);
        }

        public void OpenLinkInBrowser()
        {
            Application.OpenURL(_link);
            ClosePopup();
        }
    }
}