using FourthWall.CompilationSimulation.Controllers;
using FourthWall.ExternalWeb.Controllers;
using FourthWall.FileGeneration.Controllers;
using FourthWall.NumberPattern.Controllers;
using FourthWall.SavingActions.Controllers;
using FourthWall.UserInformation.Controllers;
using Story.Controllers;

namespace FourthWall.Commons
{
    public class FourthWallMvc
    {
        //Singleton instance
        private static FourthWallMvc _instance;
        public static FourthWallMvc Instance
        {
            get
            {
                _instance ??= new FourthWallMvc();
                return _instance;
            }
        }

        public FileGenerationController FileGenerationController { get; set; }
        public UserInformationController UserInformationController { get; set; }
        public NumberPatternController NumberPatternController { get; set; }
        public ExternalWebController ExternalWebController { get; set; }
        public SavingActionsController SavingActionsController { get; set; }
        public CompilationSimulationController CompilationSimulationController { get; set; }
        
        private FourthWallMvc()
        {
            FileGenerationController = new FileGenerationController();
            UserInformationController = new UserInformationController();
            NumberPatternController = new NumberPatternController();
            ExternalWebController = new ExternalWebController();
            SavingActionsController = new SavingActionsController();
            CompilationSimulationController = new CompilationSimulationController();
        }
    }
}