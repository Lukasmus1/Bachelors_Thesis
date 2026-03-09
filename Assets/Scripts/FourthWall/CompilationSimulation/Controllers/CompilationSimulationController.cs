using FourthWall.CompilationSimulation.Models;

namespace FourthWall.CompilationSimulation.Controllers
{
    public class CompilationSimulationController
    {
        private readonly CompilationSimulationModel _model = new();
        
        /// <summary>
        /// Gets the first path of K-P's compilation.
        /// </summary>
        /// <returns>K-P's compilation path</returns>
        public string GetKpCompilationPath()
        {
            return _model.GetKpCompilationPath();
        }
        
        /// <summary>
        /// Begins the compilation simulation by creating files in the K-P compilation path in sequence with a delay.
        /// </summary>
        public void BeginCompilationSimulation()
        {
            _model.BeginCompilationSimulation();
        }

        /// <summary>
        /// The first event from Curator that prompts the user to move K-P's compilation files.
        /// </summary>
        public void FirstMovePrompt()
        {
            _model.FirstMovePrompt();
        }
        
        public void ChangeKpCompilationPath(string newPath)
        {
            _model.ChangeKpCompilationPath(newPath);
        }
    }
}