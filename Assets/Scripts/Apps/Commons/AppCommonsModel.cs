using System;

namespace Apps.Commons
{
    public class AppCommonsModel
    {
        //Singleton instance
        private static AppCommonsModel _instance;
        public static AppCommonsModel Instance
        {
            get
            {
                _instance ??= new AppCommonsModel();
                return _instance;
            }
        }

        public event Action<string> AppOpened;
        public void OnAppOpened(string appName)
        {
            AppOpened?.Invoke(appName);
        }
        
        //Private constructor to prevent instantiation
        private AppCommonsModel() {}
    }
}