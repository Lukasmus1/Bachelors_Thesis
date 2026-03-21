using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using User.Commons;

public class WelcomeBackScreen : MonoBehaviour
{
    [SerializeField] private string mainScene;
    
    [SerializeField] private TMP_Text welcomeBackText;

    private void Awake()
    {
        welcomeBackText.text = $"Welcome back, {UserMvc.Instance.UserController.Username}!";
    }

    public void Login()
    {
        SceneManager.LoadScene(mainScene);
    }
}