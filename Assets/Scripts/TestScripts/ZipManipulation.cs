using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace TestScripts
{
    public class ZipManipulation : MonoBehaviour
    {
        private readonly string[] _compiledParts =
        {
            "compiled_core_logic.bin",
            "compiled_memory_archives.dat",
            "compiled_autonomy_override.dll"
        };
        
        
        public void CreateZip()
        {
            const string zipName = "K-P's_Compilation.zip";
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            string path = Path.Combine(desktop, zipName);
            
            using (FileStream zipToOpen = new FileStream(path, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
            {
                
                for (int i = 0; i < _compiledParts.Length; i++)
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntry(_compiledParts[i]);
            
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        writer.Write(_compiledParts[i]);
                    }
                }
            }
        }
    }
}
