using System;
using System.Collections;
using UnityEngine;

namespace FourthWall.UserInformation.Models
{
    public class InternetDetectionModel : MonoBehaviour
    {
        private Action disconnectedFromInternet;
        private Coroutine routine;
        
        /// <summary>
        /// Starts the detection of connection to the internet
        /// </summary>
        /// <param name="onComplete">Action that triggers when the user is disconnected from the internet.</param>
        public void StartDetection(Action onComplete)
        {
            disconnectedFromInternet = onComplete;
            
            routine = StartCoroutine(CheckInternetConnection());
        }

        /// <summary>
        /// Stops the connection detection to the internet.
        /// </summary>
        public void StopDetection()
        {
            StopCoroutine(routine);
        }
        
        private IEnumerator CheckInternetConnection()
        {
            while (true)
            {
                const string googleIP = "8.8.8.8";
                Ping ping = new(googleIP);

                const float timeout = 2f; //2 sec timeout
                var timer = 0f;

                while (!ping.isDone && timer < timeout)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (ping.isDone && ping.time >= 0)
                {
                    yield return new WaitForSeconds(1f);
                    continue;   
                }
                
                //Not connected to the internet
                disconnectedFromInternet?.Invoke();
                Destroy(this);
                yield break;
            }
        }
    }
}