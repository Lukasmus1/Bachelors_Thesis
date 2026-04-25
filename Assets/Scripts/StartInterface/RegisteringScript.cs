using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using User.Commons;

namespace StartInterface
{
    public class RegisteringScript : MonoBehaviour
    {
        [SerializeField] private InputField usernameInputField;
        [SerializeField] private string sceneToLoad; 
        
        public void RegisterOnSubmit(string username)
        {
            if (RegisterUsername(username))
            {
                StartGame();
            }
        }

        public void RegisterOnButton()
        {
            if (RegisterUsername(usernameInputField.text))
            {
                StartGame();
            }
        }

        private static bool RegisterUsername(string username)
        {
            if (username.Length is 0 or > 40)
            {
                return false;
            }
            
            UserMvc.Instance.UserController.Username = username;
            return true;
        }

        private void StartGame()
        {
            SceneManager.LoadScene(sceneToLoad);
        }

    }
}
