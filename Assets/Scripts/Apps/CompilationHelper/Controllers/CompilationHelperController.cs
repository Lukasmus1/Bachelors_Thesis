using System;
using Apps.CompilationHelper.Models;
using Apps.CompilationHelper.Views;
using Desktop.Commons;
using FourthWall.Commons;

namespace Apps.CompilationHelper.Controllers
{
    public class CompilationHelperController
    {
        private readonly CompilationHelperModel _model = new();
        private CompilationHelperView _view;

        public Action<int> OnCompilationProgressUpdateSeconds
        {
            get => _model.onCompilationProgressUpdateSeconds;
            set => _model.onCompilationProgressUpdateSeconds = value;
        }
        
        public void SetView(CompilationHelperView view)
        {
            _view = view;
        }

        /// <summary>
        /// Gets the max compilation time in seconds that is set for the simulated compilation process.
        /// </summary>
        /// <returns></returns>
        public int GetMaxCompilationTimeSeconds() => _model.CompilationTimeSeconds;
        
        /// <summary>
        /// Enables the simulated compilation process. 
        /// </summary>
        /// <param name="compilationTimeSeconds">How long should the compilation be in seconds</param>
        public void EnableCompilationProcess(int compilationTimeSeconds)
        {
            DesktopMvc.Instance.DesktopGeneratorController.ToggleIcon("Compilation Helper", true);
            _view.SetupProgressBar(compilationTimeSeconds);
            _view.SetupDeletionProgressBar(compilationTimeSeconds / 3);
            _model.StartCompilation(compilationTimeSeconds);
            
            FourthWallMvc.Instance.CompilationSimulationController.BeginCompilationSimulation(); // must be called after _model.StartCompilation -> CompilationTimeSeconds not set
        }

        /// <summary>
        /// Enables the UI for moving the compiled files to the K-P compilation folder.
        /// </summary>
        public void EnableKpCompilationFileMoving()
        {
            _view.EnableKpCompilationFileMoving();
        }
        
        /// <summary>
        /// Gets the current compilation time.
        /// </summary>
        /// <returns>Current compilation time</returns>
        public float GetCurrentCompilationTime()
        {
            return _model.GetCurrentCompilationTime();
        }

        /// <summary>
        /// Gets whether the compilation process is still running.
        /// </summary>
        /// <returns>Is the compilation simulation running?</returns>
        public bool IsCompilationRunning()
        {
            return _model.IsCompilationRunning();
        }
    }
}