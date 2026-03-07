using System;
using UnityEngine;

namespace FourthWall.UserInformation.Models
{
    public class UserTimeChecker : MonoBehaviour
    {
        private Action onCurrentTimeInRange;
        private Action _onTimeInRange;
        
        private TimeSpan fromTime;
        private TimeSpan toTime;
        private bool start = false;
        
        public void StartTimeChecking(string from, string to, Action onCurrentTimeInRange)
        {
            fromTime = TimeSpan.Parse(from);
            toTime = TimeSpan.Parse(to);
            
            _onTimeInRange = onCurrentTimeInRange;
            this.onCurrentTimeInRange += _onTimeInRange;
            
            start = true;
        }

        private void Update()
        {
            if (!start || !IsCurrentTimeInRange())
            {
                return;
            }

            onCurrentTimeInRange?.Invoke();
            start = false;
            onCurrentTimeInRange -= _onTimeInRange;
            Destroy(this);
        }
        
        private bool IsCurrentTimeInRange()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            
            if (toTime < fromTime)
            {
                return currentTime >= fromTime || currentTime <= toTime;
            }
            else
            {
                return currentTime >= toTime || currentTime <= fromTime;
            }
            
        }
        
    }
}