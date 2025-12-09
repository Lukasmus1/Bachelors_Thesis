using Apps.Autostereogram.Controllers;

namespace Apps.Autostereogram.Commons
{
    public class AutostereogramMvc
    {
        //Singleton instance
        private static AutostereogramMvc _instance;
        public static AutostereogramMvc Instance
        {
            get
            {
                _instance ??= new AutostereogramMvc();
                return _instance;
            }
        }

        public AutospectogramController AutostereogramController { get; set; }
        
        private AutostereogramMvc()
        {
            AutostereogramController = new AutospectogramController();
        }
    }
}