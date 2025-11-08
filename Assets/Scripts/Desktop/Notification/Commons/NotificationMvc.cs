using Desktop.Notification.Controllers;

namespace Desktop.Notification.Commons
{
    public class NotificationMvc
    {
        //Singleton instance
        private static NotificationMvc _instance;
        public static NotificationMvc Instance
        {
            get
            {
                _instance ??= new NotificationMvc();
                return _instance;
            }
        }

        public NotificationController NotificationController { get; set; }
        
        private NotificationMvc()
        {
            NotificationController = new NotificationController();
        }
    }
}