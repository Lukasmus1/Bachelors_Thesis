using Saving.Controllers;

namespace Saving.Commons
{
    public class SavingMvc
    {
        //Singleton instance
        private static SavingMvc _instance;
        public static SavingMvc Instance
        {
            get
            {
                _instance ??= new SavingMvc();
                return _instance;
            }
        }

        public SaveController SavingController { get; set; }
        
        private SavingMvc()
        {
            SavingController = new SaveController();
        }
    }
}