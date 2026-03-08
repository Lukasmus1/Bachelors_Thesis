using System;
using System.Collections.Generic;
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
    }
}