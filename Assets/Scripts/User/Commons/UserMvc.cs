using User.Controllers;

namespace User.Commons
{
    public class UserMvc
    {
        //Singleton instance
        private static UserMvc _instance;
        public static UserMvc Instance
        {
            get
            {
                _instance ??= new UserMvc();
                return _instance;
            }
        }

        public UserController DesktopGeneratorController { get; set; }
        
        private UserMvc()
        {
            DesktopGeneratorController = new UserController();
        }
    }
}