using System;

namespace Sounds.Models
{
    [Serializable]
    public class SoundModel
    {
        public float EffectsVolume { get; set; } = 1f;
        public float MusicVolume { get; set; } = 1f;
    }
}