using Apps.Autospectogram.Controllers;

namespace Apps.Autospectogram.Commons
{
    public class AutospectogramMvc
    {
        //Singleton instance
        private static AutospectogramMvc _instance;
        public static AutospectogramMvc Instance
        {
            get
            {
                _instance ??= new AutospectogramMvc();
                return _instance;
            }
        }

        public AutospectogramController AutospectogramController { get; set; }
        
        private AutospectogramMvc()
        {
            AutospectogramController = new AutospectogramController();
        }
    }
}