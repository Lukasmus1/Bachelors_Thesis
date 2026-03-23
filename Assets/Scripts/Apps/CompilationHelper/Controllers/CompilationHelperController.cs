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

        public Action OnAllFilesDeleted
        {
            get => _view.onAllFilesDeleted;
            set => _view.onAllFilesDeleted = value;
        }

        public Action onCompilationFinished;
        public void InvokeCompilationFinished()
        {
            onCompilationFinished?.Invoke();
        }

        public Action onCompilationFailed;
        public void InvokeCompilationFailed()
        {
            onCompilationFailed?.Invoke();
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
        /// Enables the simulated compilation process for AI path.
        /// </summary>
        /// <param name="compilationTimeSeconds">How long should the compilation be in seconds</param>
        public void EnableForAICompilationProcess(int compilationTimeSeconds)
        {
            DesktopMvc.Instance.DesktopGeneratorController.ToggleIcon("Compilation Helper", true);
            _view.SetupProgressBar(compilationTimeSeconds, true);
            _view.SetupDeletionProgressBar(compilationTimeSeconds / 3);
            _view.EnableLayout(true);
            _model.StartCompilation(compilationTimeSeconds);
            
            FourthWallMvc.Instance.CompilationSimulationController.BeginCompilationSimulation(); // must be called after _model.StartCompilation -> CompilationTimeSeconds not set
        }

        /// <summary>
        /// Enables the simulated compilation process for curator path.
        /// </summary>
        /// <param name="compilationTimeSeconds">How long should the compilation be in seconds</param>
        public void EnableForCuratorCompilationProcess(int compilationTimeSeconds)
        {
            DesktopMvc.Instance.DesktopGeneratorController.ToggleIcon("Compilation Helper", true);
            _view.SetupProgressBar(compilationTimeSeconds, false);
            _view.EnableLayout(false);
            _model.StartCompilation(compilationTimeSeconds);
            
            FourthWallMvc.Instance.CompilationSimulationController.BeginCompilationSimulation();
        }

        /// <summary>
        /// Stops the compilation process.
        /// </summary>
        public void StopCompilation()
        {
            _model.StopCompilation();
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