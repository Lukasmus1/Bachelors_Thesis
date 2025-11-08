using System;
using System.Collections;
using Desktop.Notification.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Desktop.Notification.Views
{
    public class NotificationView : MonoBehaviour
    {
        [SerializeField] private TMP_Text notificationText;
        [SerializeField] private CanvasGroup notificationCanvasGroup;
        
        private void Awake()
        {
            NotificationMvc.Instance.NotificationController.NotificationView = this;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            StartCoroutine(EnableNotification());
        }
        
        private IEnumerator EnableNotification()
        {
            // Fade in
            var elapsed = 0f;
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                notificationCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / 0.5f);
                yield return null;
            }
            
            yield return new WaitForSeconds(4f);
            
            // Fade out
            elapsed = 0f;
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                notificationCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / 0.5f);
                yield return null;
            }
            
            yield return new WaitForSeconds(0.5f);

            gameObject.SetActive(false);
        }

        public void SetNotificationText(string text)
        {
            notificationText.text = text;
        }
    }
}