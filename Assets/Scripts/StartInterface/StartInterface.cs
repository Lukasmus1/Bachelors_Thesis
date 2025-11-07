using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User.Commons;

namespace StartInterface
{
    public class StartInterface : MonoBehaviour
    {
        [SerializeField] private Image backgroundBlur;
        
        [SerializeField] private TMP_Text welcomeText;

        private void Awake()
        {
            welcomeText.text = $"Welcome back {UserMvc.Instance.UserController.Username}.";
        }

        public void StartGame(Button clickedButton)
        {
            const float fadeDuration = 1.5f;
            clickedButton.interactable = false;
            
            clickedButton.GetComponent<Image>().CrossFadeAlpha(0, fadeDuration, false);
            backgroundBlur.CrossFadeAlpha(0, fadeDuration, false);
            
            StartCoroutine(OnBlurFadeComplete(fadeDuration));
        }
        
        private IEnumerator OnBlurFadeComplete(float duration)
        {
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
        }
    }
}
