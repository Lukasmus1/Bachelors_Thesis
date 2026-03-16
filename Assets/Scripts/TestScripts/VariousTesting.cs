using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TestScripts
{
    public class VariousTesting : MonoBehaviour
    {
        [DllImport("AudioPlugin.dll")]
        private static extern bool IsSystemMuted();
        
        private void Update()
        {
            
        }
    }
}