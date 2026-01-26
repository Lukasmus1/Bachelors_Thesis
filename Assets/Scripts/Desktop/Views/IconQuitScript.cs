using UnityEngine;

namespace Desktop.Views
{
    public class IconQuitScript : MonoBehaviour
    {
        [SerializeField] private GameObject quitPopup;
        [SerializeField] private Transform canvasTransform;
        
        public void QuitDesktop()
        {
            Instantiate(quitPopup, canvasTransform);
        }
    }
}
