using System;
using System.Collections.Generic;
using System.IO;
using Apps.CompilationHelper.Models;
using FourthWall.Commons;

namespace FourthWall.CompilationSimulation.Models
{
    public static class ScatteredFiles
    {
        private static readonly Dictionary<FileEnum, string> FileDirectories = new()
        {
            { FileEnum.DesktopFile, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "K-P_Logic")},
            { FileEnum.AudioFile, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "K-P_Memory")},
            { FileEnum.FileThree, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "K-P_Autonomy")},
        };

        private static readonly Dictionary<FileEnum, string> FileNames = new()
        {
            { FileEnum.DesktopFile, "compiled_core_logic.bin"},
            { FileEnum.AudioFile, "compiled_memory_archives.dat"},
            { FileEnum.FileThree, "compiled_autonomy_override.dll"},
        };

        private const string HINT_FILE_NAME = "reminder_hint.txt";

        private static readonly Dictionary<FileEnum, string> FileHints = new()
        {
            { FileEnum.DesktopFile, "No need to obfuscate this one."},
            { FileEnum.AudioFile, "Windows uses the COM interface for its audio API's. K-P has a dependency on it. To prevent issues, the file must remain hidden until the master audio feed is cut."},
            { FileEnum.FileThree, "This file shouldn't be editable at all, but I don't know how to prevent hackers from doing it, so I'll just create a registry entry in the HKEY_CURRENT_USER\\Software folder to hide it."},
        };
        
        /// <summary>
        /// Returns the full path of a file given its FileEnum. If the directory doesn't exist, creates one.
        /// </summary>
        /// <param name="file">Type of file</param>
        /// <returns>Path to the file</returns>
        public static string GetFilePath(FileEnum file)
        {
            if (!Directory.Exists(FileDirectories[file]))
            {
                Directory.CreateDirectory(FileDirectories[file]);
            }
            
            string fileHintPath = Path.Combine(FileDirectories[file], HINT_FILE_NAME);
            if (!File.Exists(fileHintPath))
            {
                FourthWallMvc.Instance.FileGenerationController.CreateFile(fileHintPath, FileHints[file], false);
            }
            
            return Path.Combine(FileDirectories[file], FileNames[file]);
        }
    }
}