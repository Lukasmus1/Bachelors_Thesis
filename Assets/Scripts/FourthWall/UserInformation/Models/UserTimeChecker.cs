using System;
using UnityEngine;

namespace FourthWall.UserInformation.Models
{
    public class UserTimeChecker : MonoBehaviour
    {
        private Action _onCurrentTimeInRange;
        private Action _onTimeInRange;
        
        private TimeSpan _fromTime;
        private TimeSpan _toTime;
        private bool _start = false;
        
        public void StartTimeChecking(string from, string to, Action onCurrentTimeInRange)
        {
            _fromTime = TimeSpan.Parse(from);
            _toTime = TimeSpan.Parse(to);
            
            _onTimeInRange = onCurrentTimeInRange;
            _onCurrentTimeInRange += _onTimeInRange;
            
            _start = true;
        }

        private void Update()
        {
            if (!_start || !IsCurrentTimeInRange())
            {
                return;
            }

            _onCurrentTimeInRange?.Invoke();
            _start = false;
            _onCurrentTimeInRange -= _onTimeInRange;
            Destroy(this);
        }
        
        private bool IsCurrentTimeInRange()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            
            if (_toTime < _fromTime)
            {
                return currentTime >= _fromTime || currentTime <= _toTime;
            }
            else
            {
                return currentTime <= _toTime && currentTime >= _fromTime;
            }
            
        }
        
    }
}