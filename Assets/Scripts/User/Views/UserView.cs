using UnityEngine;
using User.Commons;

namespace User.Views
{
    public class UserView : MonoBehaviour
    {
        private void Awake()
        {
            UserMvc.Instance.UserController.InitUser();
        }
    }
}