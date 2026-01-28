using System;
using System.Collections;
using System.Collections.Generic;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Desktop.Notification.Views
{
    public class NotificationView : MonoBehaviour
    {
        [SerializeField] private TMP_Text notificationText;
        [SerializeField] private CanvasGroup notificationCanvasGroup;
        
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite errorIcon;
        [SerializeField] private Sprite notificationIcon;
        
        private Coroutine coroutine;
        private readonly List<(string, Sprite)> _notificationQueue = new();
        private Action notificationDone; 
        
        private void Awake()
        {
            NotificationMvc.Instance.NotificationController.NotificationView = this;
            gameObject.SetActive(false);
            notificationDone += CheckForQueuedNotifications;
        }

        private void OnDestroy()
        {
            notificationDone -= CheckForQueuedNotifications;
        }

        //Activate the notification display
        public void ActivateNotification()
        {
            if (coroutine != null)
            {
                return;
            }
            
            gameObject.SetActive(true);
            coroutine = StartCoroutine(EnableNotification());
        }

        //Check if there are any queued notifications and display them
        private void CheckForQueuedNotifications()
        {
            //If the queue is empty, return
            if (_notificationQueue.Count == 0)
            {
                return;
            }
            
            //Activate the notification again
            ActivateNotification();
        }
        
        //Coroutine to enable the notification for a set amount of time
        private IEnumerator EnableNotification()
        {
            //Set the notification text to the first item in the queue and remove it from the queue
            notificationText.text = _notificationQueue[0].Item1;
            iconImage.sprite = _notificationQueue[0].Item2;
            _notificationQueue.RemoveAt(0);
            
            // Fade in
            var elapsed = 0f;
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                notificationCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / 0.5f);
                yield return null;
            }
            
            yield return new WaitForSeconds(3f);
            
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
            
            coroutine = null;
            
            notificationDone?.Invoke();
        }

        //Setter for notification text
        public void SetNotificationText(string text, NotificationType type)
        {
            _notificationQueue.Add((text, GetNotificationIcon(type)));
        }

        private Sprite GetNotificationIcon(NotificationType type)
        {
            return type switch
            {
                NotificationType.Error => errorIcon,
                _ => notificationIcon
            };
        }
        
        private void OnApplicationQuit()
        {
            StopAllCoroutines();
        }
    }
}