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
    }
}