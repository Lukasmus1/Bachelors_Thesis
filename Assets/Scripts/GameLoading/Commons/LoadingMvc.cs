using GameLoading.Controllers;
using Saving.Controllers;

namespace GameLoading.Commons
{
    public class LoadingMvc
    {
        //Singleton instance
        private static LoadingMvc _instance;
        public static LoadingMvc Instance
        {
            get
            {
                _instance ??= new LoadingMvc();
                return _instance;
            }
        }

        public GameLoadingController GameLoadingController { get; set; }
        
        private LoadingMvc()
        {
            GameLoadingController = new GameLoadingController();
        }
    }
}