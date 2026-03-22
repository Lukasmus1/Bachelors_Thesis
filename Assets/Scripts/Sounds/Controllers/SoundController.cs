using Sounds.Models;
using Sounds.Views;
using UnityEngine;

namespace Sounds.Controllers
{
    public class SoundController
    {
        private readonly SoundModel _soundModel = new();
        private SoundView _soundView;

        public void SetView(SoundView soundView)
        {
            _soundView = soundView;
        }
        
        /// <summary>
        /// Plays a sound at the given transform position. https://www.youtube.com/watch?v=DU7cgVsU2rM
        /// </summary>
        /// <param name="clip">Clip to play</param>
        /// <param name="spawnTransform">Position to spawn the clip in</param>
        public void PlaySound(AudioClip clip, Transform spawnTransform)
        {
            _soundView.PlaySound(clip, spawnTransform);
        }
    }
}