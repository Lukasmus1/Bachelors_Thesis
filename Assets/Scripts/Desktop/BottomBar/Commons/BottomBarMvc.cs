using Apps.FileManager.Commons;
using Apps.FileManager.Controllers;
using Desktop.BottomBar.Controllers;

namespace Desktop.BottomBar.Commons
{
    public class BottomBarMvc
    {
        //Singleton instance
        private static BottomBarMvc _instance;
        public static BottomBarMvc Instance
        {
            get
            {
                _instance ??= new BottomBarMvc();
                return _instance;
            }
        }

        public BottomBarController BottomBarController { get; set; }
        
        private BottomBarMvc()
        {
            BottomBarController = new BottomBarController();
        }
    }
}