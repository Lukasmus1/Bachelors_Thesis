using Apps.CompilationHelper.Controllers;
using Desktop.Controllers;

namespace Desktop.Commons
{
    public class DesktopMvc
    {
        //Singleton instance
        private static DesktopMvc _instance;
        public static DesktopMvc Instance
        {
            get
            {
                _instance ??= new DesktopMvc();
                return _instance;
            }
        }

        public DesktopGeneratorController DesktopGeneratorController { get; set; }
        public CompilationHelperController CompilationHelperController { get; set; }
        
        private DesktopMvc()
        {
            DesktopGeneratorController = new DesktopGeneratorController();
            CompilationHelperController = new CompilationHelperController();
        }
    }
}