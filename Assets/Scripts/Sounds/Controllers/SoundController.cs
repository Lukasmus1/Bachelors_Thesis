using Sounds.Models;
using Sounds.Views;
using UnityEngine;
using AudioType = Sounds.Models.AudioType;

namespace Sounds.Controllers
{
    public class SoundController
    {
        public SoundModel soundModel = new();
        private SoundView _soundView;

        public void SetView(SoundView soundView)
        {
            _soundView = soundView;
        }

        public float EffectsVolume
        {
            get =>  soundModel.EffectsVolume;
            set =>  soundModel.EffectsVolume = value;
        }

        public float MusicVolume
        {
            get =>  soundModel.MusicVolume;
            set =>  soundModel.MusicVolume = value;
        }

        public AudioSource BackgroundMusic
        {
            get => _soundView.bgmAudioSource;
            set => _soundView.bgmAudioSource = value;
        }
        
        /// <summary>
        /// Plays a sound at the given transform position. https://www.youtube.com/watch?v=DU7cgVsU2rM
        /// </summary>
        /// <param name="clip">Clip to play</param>
        /// <param name="spawnTransform">Position to spawn the clip in</param>
        /// <param name="audioType">Type of the audio played</param>
        public void PlaySound(AudioClip clip, Transform spawnTransform, AudioType audioType)
        {
            _soundView.PlaySound(clip, spawnTransform, audioType);
        }

        /// <summary>
        /// Updates the real volume from model (used when loading a save)
        /// </summary>
        public void UpdateVolumeFromModel()
        {
            UpdateSoundVolume(EffectsVolume, AudioType.Effects);
            UpdateSoundVolume(MusicVolume, AudioType.Music);
        }
        
        /// <summary>
        /// Updates a volume of specified sound type
        /// </summary>
        /// <param name="volume">Volume value</param>
        /// <param name="audioType">Type of audio</param>
        public void UpdateSoundVolume(float volume, AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.Effects:
                    _soundView.audioMixerEffects.audioMixer.SetFloat("EffectsVolume", volume);
                    EffectsVolume = volume;
                    break;
                case AudioType.Music:
                    _soundView.audioMixerMusic.audioMixer.SetFloat("MusicVolume", volume);
                    MusicVolume = volume;
                    break;
            }
        }

        /// <summary>
        /// Gets the music volume value
        /// </summary>
        /// <returns>Music volume value</returns>
        public float GetMusicVolumeValue()
        {
            _soundView.audioMixerMusic.audioMixer.GetFloat("MusicVolume", out float musicVolume);
            return musicVolume;
        }

        /// <summary>
        /// Gets the sound effects volume value
        /// </summary>
        /// <returns>Sound effects volume value</returns>
        public float GetEffectsVolumeValue()
        {
            _soundView.audioMixerEffects.audioMixer.GetFloat("EffectsVolume", out float effectsVolume);
            return effectsVolume;
        }
    }
}