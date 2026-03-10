using System;
using FourthWall.CompilationSimulation.Models;

namespace FourthWall.CompilationSimulation.Controllers
{
    public class CompilationSimulationController
    {
        private readonly CompilationSimulationModel _model = new();
        
        public Action OnPingedByCurator 
        {
            get => _model.onFolderPingedByCurator;
            set => _model.onFolderPingedByCurator = value;
        }
        
        /// <summary>
        /// Gets the first path of K-P's compilation.
        /// </summary>
        /// <returns>K-P's compilation path</returns>
        public string CreateKpCompilationPath()
        {
            return _model.CreateKpCompilationPath();
        }

        /// <summary>
        /// Gets the current path of K-P's compilation, which may have been changed by the user.
        /// </summary>
        /// <returns>Current K-P's compilation path</returns>
        public string GetKpCompilationPath() => _model.kpCompilationPath;
        
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
        
        /// <summary>
        /// Changes the path of K-P's compilation to the given new path, which simulates the user moving K-P's compilation files to a different folder.
        /// </summary>
        /// <param name="newPath">New path to copy to</param>
        /// <returns>Was the moving successful?</returns>>
        public bool MoveKpCompilationPath(string newPath)
        {
            return _model.MoveKpCompilationPath(newPath);
        }

        public void StartCuratorPings()
        {
            _model.StartCuratorPings();
        }
    }
}