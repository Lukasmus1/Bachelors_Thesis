using Saving.Commons;
using UnityEngine;

namespace Desktop.Views
{
    public class QuitGamePopupScript : MonoBehaviour
    {
        [SerializeField] private GameObject creditsPanel;
        
        public void OnConfirm()
        {
            SavingMvc.Instance.SavingController.QuitAndSaveGame();
        }
        
        public void OnCancel()
        {
            Destroy(gameObject);
        }

        public void OnCredits()
        {
            Instantiate(creditsPanel, transform.parent);
            Destroy(gameObject);
        }
    }
}
