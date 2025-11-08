using System;
using Desktop.Notification.Models;
using Desktop.Notification.Views;

namespace Desktop.Notification.Controllers
{
    public class NotificationController
    {
        private readonly NotificationModel _notificationModel = new();
        public NotificationView NotificationView { private get; set; }
        
        public void InstantiateNotification(NotificationType notificationType, string message = null)
        {
            switch (notificationType)
            {
                case NotificationType.Generic:
                    if (message == null)
                    {
                        throw new ArgumentNullException(nameof(message), "Message cannot be null for Generic notification type.");
                    }
                    NotificationView.SetNotificationText(message);
                    break;
                
                case NotificationType.NewMessage:
                    NotificationView.SetNotificationText("New Message Received");
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null);
            }
            
            NotificationView.gameObject.SetActive(true);
        }
    }
}