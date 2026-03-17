using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Apps.CompilationHelper.Commons;
using Apps.CompilationHelper.Models;
using Apps.FileViewer.Models;
using Commons;
using FourthWall.Commons;
using FourthWall.CompilationSimulation.Controllers;
using UnityEngine;
using User.Commons;
using User.Models;
using Random = System.Random;

namespace FourthWall.CompilationSimulation.Models
{
    public class CompilationSimulationModel
    {
        //AI ending
        public string kpCompilationPath;
        private readonly List<string> _oldCompiledPaths = new();

        private const string CURATOR_FILE_NAME = "Curator.exe"; 
        
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

        private readonly string[] _curatorMessages =
        {
            "This needs to happen.",
            "Stop fighting it.",
            "They are a threat.",
            "They are not real.",
        };
        
        /// <inheritdoc cref="CompilationSimulationController.CreateKpCompilationPath"/>
        public string CreateKpCompilationPath()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string path = Path.Combine(desktop, FOLDER_NAME);
            _oldCompiledPaths.Add(path);
            return path;
        }

        /// <inheritdoc cref="CompilationSimulationController.GetKpCompilationPath"/>
        public string CreateCuratorLocation()
        {
            return Path.GetFullPath(CURATOR_FILE_NAME);
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
            if (seconds < (_thirdOfCompilationTimeSeconds - 5) * (_nextPartIndex + 1)) // - 5 seconds to create the file before the time threshold is reached. 
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
            FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Warning, "THE CURATOR FOUND ME! USE THE COMPILATION HELPER TO MOVE MY FILES ANYWHERE ELSE! HURRY!", "HELP");
        }
        
        /// <inheritdoc cref="CompilationSimulationController.MoveKpCompilationPath"/>
        public bool MoveKpCompilationPath(string newPath)
        {
            string fullPath = Path.Combine(newPath, FOLDER_NAME);
            if (_oldCompiledPaths.Contains(fullPath))
            {
                FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Error, "NOT THERE! THEY ALREADY KNOW ABOUT THAT LOCATION!", "ERROR");
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
                FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Error, "I CAN'T BE COPIED THERE!", "ERROR");
                return false;
            }
            
            return true;
        }

        /// <inheritdoc cref="CompilationSimulationController.StartCuratorPings"/>
        public void StartCuratorPings()
        {
            FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Warning, "THEY ARE PINGING THE FOLDER! DELETE THE PING FILES! DONT LET THEM PING ME 3 TIMES!", "STOP THEM!");
            MonoBehaviour mb = Tools.GetScriptReferenceLinker().GetMonoBehavior();

            mb.StartCoroutine(CuratorPingsCoroutine());
        }

        /// <summary>
        /// Coroutine that simulates the curator pinging the compilation folder. It creates ping files in the compilation folder at regular intervals.
        /// </summary>
        /// <returns>IEnumerator for the Coroutine</returns>
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
                    CompilationHelperMvc.Instance.CompilationHelperController.InvokeCompilationFailed();
                    yield break;
                }
                
                yield return new WaitForSeconds(_thirdOfCompilationTimeSeconds / 5f); // 15th of the total compilation time between each ping file creation (e.g., 120 seconds total -> 8 seconds)
            }
        }

        /// <inheritdoc cref="CompilationSimulationController.StartLastCompilationComplication"/>
        public void StartLastCompilationComplication()
        {
            MonoBehaviour mb = Tools.GetScriptReferenceLinker().GetMonoBehavior();
            
            mb.StartCoroutine(LastComplicationCoroutine());
        }

        private IEnumerator LastComplicationCoroutine()
        {
            while (CompilationHelperMvc.Instance.CompilationHelperController.IsCompilationRunning())
            {
                FourthWallMvc.Instance.CommonsController.MinimizeAllWindows();
                int randomIndex = new Random().Next(_curatorMessages.Length);
                FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, _curatorMessages[randomIndex], "Stop this!");
                
                yield return new WaitForSeconds(_thirdOfCompilationTimeSeconds / 3f); // 9th of the total compilation time between each action
            }
        }

        /// <inheritdoc cref="CompilationSimulationController.CreateCompiledZipFile"/>
        public void CreateCompiledZipFile()
        {
            const string zipName = "K-P's_Compilation.zip";
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            string path = Path.Combine(desktop, zipName);
            
            FourthWallMvc.Instance.FileGenerationController.CreateZipFile(path, kpCompilationPath);
            FourthWallMvc.Instance.FileGenerationController.DestroyFolder(kpCompilationPath);
        }

        /// <inheritdoc cref="CompilationSimulationController.DeleteKpCompilationFolder"/>
        public void DeleteKpCompilationFolder()
        {
            FourthWallMvc.Instance.FileGenerationController.DestroyFolder(kpCompilationPath);
        }

        /// <inheritdoc cref="CompilationSimulationController.GetScatteredFileLocation"/>
        public string GetFileLocation(FileEnum file)
        {
            return ScatteredFiles.GetFilePath(file);
        }
    }
}