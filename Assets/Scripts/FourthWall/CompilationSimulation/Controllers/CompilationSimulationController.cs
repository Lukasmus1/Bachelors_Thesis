using System;
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

        /// <summary>
        /// Starts the pinging from the curator by creating files in the K-P compilation path in sequence with a delay, which simulates the curator trying to find K-P's compilation by pinging the folder it's in.
        /// </summary>
        public void StartCuratorPings()
        {
            _model.StartCuratorPings();
        }
        
        /// <summary>
        /// Begins the last set of complications while compilation simulation is running.
        /// </summary>
        public void StartLastCompilationComplication()
        {
            _model.StartLastCompilationComplication();
        }

        /// <summary>
        /// Creates the location of the curator file.
        /// </summary>
        /// <returns>Location of the curator file</returns>
        public string CreateCuratorLocation()
        {
            return _model.CreateCuratorLocation();
        }

        /// <summary>
        /// Creates the compiled zip file in the K-P compilation path on the user's desktop.
        /// </summary>
        public void CreateCompiledZipFile()
        {
            _model.CreateCompiledZipFile();
        }
        
        /// <summary>
        /// Deletes the KP compilation folder
        /// </summary>
        public void DeleteKpCompilationFolder()
        {
            _model.DeleteKpCompilationFolder();
        }
    }
}