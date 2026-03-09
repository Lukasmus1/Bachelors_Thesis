using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Commons
{
    public static class Tools
    {
        private static GameObject _scriptHolder;

        /// <summary>
        /// Gets the GameObject with the tag "ScriptHolder".
        /// </summary>
        /// <returns>ScriptHolder GameObject</returns>
        public static GameObject GetScriptHolder()
        {
            if (_scriptHolder == null)
            {
                _scriptHolder = GameObject.FindGameObjectWithTag("ScriptHolder");   
            }

            return _scriptHolder;
        }

        /// <summary>
        /// Gets the ScriptReferenceLinker component from the ScriptHolder GameObject.
        /// </summary>
        /// <returns>ScriptReferenceLinker instance</returns>
        public static ScriptReferenceLinker GetScriptReferenceLinker() => GetScriptHolder().GetComponent<ScriptReferenceLinker>();

        /// <summary>
        /// Gets the maximum value of sum of a list of integers.
        /// </summary>
        /// <param name="numbers">List of integers</param>
        /// <returns>Maximum value of its sum</returns>
        public static int GetMaxSumOfList(List<int> numbers)
        {
            return numbers.Sum(x => x > 0 ? x : 0);
        }

        /// <summary>
        /// Gets the minimum value of sum of a list of integers.
        /// </summary>
        /// <param name="numbers">List of integers</param>
        /// <returns>Minimum value of its sum</returns>
        public static int GetMinSumOfList(List<int> numbers)
        {
            return numbers.Sum(x => x < 0 ? x : 0);
        }

        /// <summary>
        /// Copies a folder and its contents source to destination and destroy the source folder.
        /// </summary>
        /// <param name="source">Source folder</param>
        /// <param name="destination">Destination folder</param>
        public static void CopyFolder(string source, string destination)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
            {
                throw new ArgumentNullException();
            }
            
            if (!Directory.Exists(source) || Directory.Exists(destination))
            {
                throw new DirectoryNotFoundException();
            }

            var dir = new DirectoryInfo(source);
            Directory.CreateDirectory(destination);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destination, file.Name);
                file.CopyTo(targetFilePath);
            }
            
            Directory.Delete(source, true);
        }
    }
}