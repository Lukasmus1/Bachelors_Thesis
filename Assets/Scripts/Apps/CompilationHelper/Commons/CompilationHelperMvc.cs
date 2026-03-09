using Apps.CompilationHelper.Controllers;

namespace Apps.CompilationHelper.Commons
{
    public class CompilationHelperMvc
    {
        //Singleton instance
        private static CompilationHelperMvc _instance;
        public static CompilationHelperMvc Instance
        {
            get
            {
                _instance ??= new CompilationHelperMvc();
                return _instance;
            }
        }

        public CompilationHelperController CompilationHelperController { get; set; }
        
        private CompilationHelperMvc()
        {
            CompilationHelperController = new CompilationHelperController();
        }
    }
}