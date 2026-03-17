using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace FourthWall.UserInformation.Models
{
    public class WindowsMuteDetection : MonoBehaviour
    {
        [DllImport("AudioPlugin.dll")]
        private static extern bool IsSystemMuted();

        private bool startMuteDetection = false;
        private Action onSystemMuted;
        
        public void StartWindowsMuteDetection(Action callback)
        {
            startMuteDetection = true;
            
            onSystemMuted += callback;
        }
        
        private void Update()
        {
            if (!startMuteDetection || !IsSystemMuted()) return;

            startMuteDetection = false;
            onSystemMuted?.Invoke();
            Destroy(this);
        }
    }
}