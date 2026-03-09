using System;
using System.IO;
using Apps.CompilationHelper.Commons;
using Desktop.Commons;
using FourthWall.Commons;
using FourthWall.CompilationSimulation.Controllers;
using FourthWall.FileGeneration.Models;
using User.Commons;
using User.Models;

namespace FourthWall.CompilationSimulation.Models
{
    public class CompilationSimulationModel
    {
        private string _kpCompilationPath;

        private readonly string[] _compiledParts =
        {
            "compiled_core_logic.bin",
            "compiled_memory_archives.dat",
            "compiled_autonomy_override.dll"
        };
        private int _nextPartIndex = 0;
        private int _thirdOfCompilationTimeSeconds;
        
        /// <inheritdoc cref="CompilationSimulationController.GetKpCompilationPath"/>
        public string GetKpCompilationPath()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            const string folderName = "K-P";

            return Path.Combine(desktop, folderName);
        }

        /// <inheritdoc cref="CompilationSimulationController.BeginCompilationSimulation"/>
        public void BeginCompilationSimulation()
        {
            _kpCompilationPath = Directory
                .CreateDirectory(UserMvc.Instance.UserController.ProceduralData(UserDataType.CompilationLocation))
                .FullName;
            
            _thirdOfCompilationTimeSeconds = CompilationHelperMvc.Instance.CompilationHelperController.GetMaxCompilationTimeSeconds() / 3;
            
            CompilationHelperMvc.Instance.CompilationHelperController.OnCompilationProgressUpdateSeconds += CreateCompiledPart;
        }

        /// <summary>
        /// To simulate continuous compilation, we create the compiled parts in sequence with a delay of a third of the total compilation time.
        /// </summary>
        /// <param name="seconds">Current compilation time</param>
        private void CreateCompiledPart(int seconds)
        {
            if (seconds < _thirdOfCompilationTimeSeconds * (_nextPartIndex + 1))
            {
                return;
            }
            
            string partPath = Path.Combine(_kpCompilationPath, _compiledParts[_nextPartIndex]);
            string fileData = FourthWallMvc.Instance.FileGenerationController.GenerateFileData();

            FourthWallMvc.Instance.FileGenerationController.CreateFile(partPath, fileData, false);
            _nextPartIndex++;
        }

        /// <inheritdoc cref="CompilationSimulationController.FirstMovePrompt"/>
        public void FirstMovePrompt()
        {
            CompilationHelperMvc.Instance.CompilationHelperController.EnableKpCompilationFileMoving();
            FourthWallMvc.Instance.FileGenerationController.ThrowWindowsDialog(DialogType.Warning, "THE CURATOR FOUND ME! USE THE COMPILATION HELPER TO MOVE MY FILES ANYWHERE ELSE! HURRY!", "HELP");
        }
        
        public void ChangeKpCompilationPath(string newPath)
        {
            _kpCompilationPath = newPath;
        }
    }
}