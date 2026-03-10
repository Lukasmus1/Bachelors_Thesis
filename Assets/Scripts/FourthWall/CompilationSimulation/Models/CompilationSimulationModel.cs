using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Apps.CompilationHelper.Commons;
using Commons;
using Desktop.Commons;
using FourthWall.Commons;
using FourthWall.CompilationSimulation.Controllers;
using FourthWall.FileGeneration.Models;
using UnityEngine;
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
        
        private readonly string[] _curatorPingFiles =
        {
            "curator_ping_1.ping",
            "curator_ping_2.ping",
            "curator_ping_3.ping"
        };
        public Action onFolderPingedByCurator;
        
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
                FourthWallMvc.Instance.FileGenerationController.ThrowWindowsDialog(DialogType.Error, "NOT THERE! THEY ALREADY KNOW ABOUT THAT LOCATION!", "ERROR");
                return false;
            }
            
            try
            {
                Tools.CopyFolder(kpCompilationPath, fullPath);
                kpCompilationPath = fullPath;
                _oldCompiledPaths.Add(fullPath);
            }
            catch (Exception)
            {
                FourthWallMvc.Instance.FileGenerationController.ThrowWindowsDialog(DialogType.Error, "I CAN'T BE COPIED THERE!", "ERROR");
                return false;
            }
            
            return true;
        }

        /// <inheritdoc cref="CompilationSimulationController.StartCuratorPings"/>
        public void StartCuratorPings()
        {
            FourthWallMvc.Instance.FileGenerationController.ThrowWindowsDialog(DialogType.Warning, "THEY ARE PINGING THE FOLDER! DELETE THE PING FILES! DONT LET THEM PING ME 3 TIMES!", "STOP THEM!");
            MonoBehaviour mb = Tools.GetScriptReferenceLinker().GetMonoBehavior();

            mb.StartCoroutine(CuratorPingsCoroutine());
        }

        private IEnumerator CuratorPingsCoroutine()
        {
            while (CompilationHelperMvc.Instance.CompilationHelperController.IsCompilationRunning())
            {
                var di = new DirectoryInfo(kpCompilationPath);
                var isPinged = false;
                List<string> existingFiles = di.GetFiles().Select(f => f.Name).ToList();

                foreach (string pingFile in _curatorPingFiles)
                {
                    if (existingFiles.Contains(pingFile))
                    {
                        isPinged = true;
                        continue;
                    }
                    
                    string path = Path.Combine(kpCompilationPath, pingFile);
                    FourthWallMvc.Instance.FileGenerationController.CreateFile(path, $"PING: {kpCompilationPath}", false);
                    isPinged = _curatorPingFiles[^1] == pingFile; // If the current ping file is the last one in the list, it means we pinged 3 times and the curator found us. Game over.
                    break;
                }

                if (isPinged)
                {
                    onFolderPingedByCurator?.Invoke();
                    yield break;
                }
                
                yield return new WaitForSeconds(_thirdOfCompilationTimeSeconds / 5f); // 15th of the total compilation time between each ping file creation (e.g., 120 seconds total -> 8 seconds)
            }
        }
    }
}