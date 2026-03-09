using System;
using System.Collections.Generic;
using System.IO;
using Apps.CompilationHelper.Commons;
using Commons;
using FourthWall.Commons;
using FourthWall.CompilationSimulation.Controllers;
using FourthWall.FileGeneration.Models;
using User.Commons;
using User.Models;

namespace FourthWall.CompilationSimulation.Models
{
    public class CompilationSimulationModel
    {
        public string kpCompilationPath;
        private readonly List<string> _oldCompiledPaths = new();
        
        private const string FOLDER_NAME = "K-P";
        private readonly string[] _compiledParts =
        {
            "compiled_core_logic.bin",
            "compiled_memory_archives.dat",
            "compiled_autonomy_override.dll"
        };
        private int _nextPartIndex = 0;
        private int _thirdOfCompilationTimeSeconds;
        
        /// <inheritdoc cref="CompilationSimulationController.CreateKpCompilationPath"/>
        public string CreateKpCompilationPath()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string path = Path.Combine(desktop, FOLDER_NAME);
            _oldCompiledPaths.Add(path);
            return path;
        }

        /// <inheritdoc cref="CompilationSimulationController.BeginCompilationSimulation"/>
        public void BeginCompilationSimulation()
        {
            kpCompilationPath = Directory
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
            
            string partPath = Path.Combine(kpCompilationPath, _compiledParts[_nextPartIndex]);
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
        
        /// <inheritdoc cref="CompilationSimulationController.MoveKpCompilationPath"/>
        public bool MoveKpCompilationPath(string newPath)
        {
            string fullPath = Path.Combine(newPath, FOLDER_NAME);
            if (_oldCompiledPaths.Contains(fullPath))
            {
                FourthWallMvc.Instance.FileGenerationController.ThrowWindowsDialog(DialogType.Error, "NOT THERE! HE ALREADY KNOWS ABOUT THAT LOCATION!", "ERROR");
                return false;
            }
            
            _oldCompiledPaths.Add(fullPath);
            Tools.CopyFolder(kpCompilationPath, fullPath);
            kpCompilationPath = fullPath;
            
            return true;
        }
    }
}