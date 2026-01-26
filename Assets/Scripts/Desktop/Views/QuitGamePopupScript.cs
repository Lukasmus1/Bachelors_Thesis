using Saving.Commons;
using UnityEngine;

namespace Desktop.Views
{
    public class QuitGamePopupScript : MonoBehaviour
    {
        public void OnConfirm()
        {
            SavingMvc.Instance.SavingController.QuitAndSaveGame();
        }
        
        public void OnCancel()
        {
            Destroy(gameObject);
        }
    }
}
