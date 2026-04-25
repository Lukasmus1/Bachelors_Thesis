using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Commons
{
    public class AsyncTimer : IDisposable
    {
        private Action onTimerComplete;
        
        private bool _isRunning = false;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _timerTask;
        
        /// <summary>
        /// Starts the timer for the specified number of seconds and invokes the onComplete action when done.
        /// </summary>
        /// <param name="seconds">Time for the timer to run</param>
        /// <param name="onComplete">Action to be invoked when timer reaches the interval</param>
        public async Task StartTimer(float seconds, Action onComplete)
        {
            //If the timer is already running on this thread, do not start another
            if (_isRunning)
            {
                return;
            }
            
            _isRunning = true;
            onTimerComplete += onComplete;
            
            _cancellationTokenSource = new CancellationTokenSource();
            
            _timerTask = RunTimerAsync(seconds, onComplete, _cancellationTokenSource.Token);
            await _timerTask;
        }
        
        /// <summary>
        /// Runs the timer asynchronously, invoking the onComplete action after the specified seconds.
        /// </summary>
        /// <param name="seconds">Time for the timer to run</param>
        /// <param name="onComplete">Action to invoke when timer reaches the interval</param>
        /// <param name="cancellationToken">Token used for cancelling running timer</param>
        private async Task RunTimerAsync(float seconds, Action onComplete, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(seconds), cancellationToken);
        
                if (!cancellationToken.IsCancellationRequested)
                {
                    onComplete?.Invoke();
                }
            }
            catch (TaskCanceledException)
            {
                //Nothing
            }
            finally
            {
                _isRunning = false;
            }
        }

        
        /// <summary>
        /// Stops the timer if it is running.
        /// </summary>
        private void StopTimer()
        {
            _cancellationTokenSource?.Cancel();
            _isRunning = false;
        }

        /// <summary>
        /// Disposes the AsyncTimer, stopping any running timer and releasing resources.
        /// </summary>
        public void Dispose()
        {
            StopTimer();
            onTimerComplete = null;
            _cancellationTokenSource?.Dispose();
        }
    }
}