using System;
using Desktop.Notification.Models;
using Desktop.Notification.Views;

namespace Desktop.Notification.Controllers
{
    public class NotificationController
    {
        private readonly NotificationModel _notificationModel = new();
        public NotificationView NotificationView { private get; set; }
        
        /// <summary>
        /// Instantiates a notification of the specified type with an optional message.
        /// </summary>
        /// <param name="notificationType">Type of the notification</param>
        /// <param name="message">Additional message that some types use</param>
        /// <exception cref="ArgumentNullException">Gets thrown when message is null with certain NotificationTypes</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if we use unimplemented NotificationType</exception>
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
                
                case NotificationType.NewFile:
                    if (message == null)
                    {
                        throw new ArgumentNullException(nameof(message), "Message cannot be null for Generic notification type.");
                    }
                    NotificationView.SetNotificationText($"New File Received: \"{message}\"");
                    break;
                
                case NotificationType.NewMessage:
                    NotificationView.SetNotificationText("New Message Received");
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null);
            }
            
            NotificationView.ActivateNotification();
        }
    }
}