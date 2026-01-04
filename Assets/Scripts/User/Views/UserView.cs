using UnityEngine;
using User.Commons;

namespace User.Views
{
    public class UserView : MonoBehaviour
    {
        private void Awake()
        {
            if (Bootstrapper.LoadedNewGame)
            {
                UserMvc.Instance.UserController.InitUser();   
            }
        }
    }
}