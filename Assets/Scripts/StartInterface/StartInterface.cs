using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace StartInterface
{
    public class StartInterface : MonoBehaviour
    {
        [SerializeField] private Image backgroundBlur;

        public void StartGame(Button clickedButton)
        {
            const float fadeDuration = 2f;
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
