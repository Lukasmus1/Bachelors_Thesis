using System;
using System.Collections;
using UnityEngine;

namespace FourthWall.UserInformation.Models
{
    public class Screenshot
    {
        /// <summary>
        /// Gets a screenshot of the current screen and returns it as a Texture2D via a callback.
        /// </summary>
        /// <param name="monoBehavior">MonoBehaviour for coroutine usage</param>
        /// <param name="callback">Callback function to get the texture</param>
        public void GetScreenshot(MonoBehaviour monoBehavior, Action<Texture2D> callback)
        {
            monoBehavior.StartCoroutine(ScreenshotCoroutine(callback));
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Coroutine to capture the screenshot after the frame has ended and invoke the callback with the captured texture.
        /// </summary>
        /// <param name="callback">Callback function to get the texture</param>
        /// <returns>Coroutine IEnumerator</returns>
        private IEnumerator ScreenshotCoroutine(Action<Texture2D> callback)
        {
            yield return new WaitForEndOfFrame();
            
            Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
            
            callback?.Invoke(tex);
        }
    }
}