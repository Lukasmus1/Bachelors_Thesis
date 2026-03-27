using System;
using Sounds.Controllers;
using UnityEngine;
using UnityEngine.Audio;
using AudioType = Sounds.Models.AudioType;

namespace Sounds.Views
{
    public class SoundView : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSourcePrefab;
        public AudioMixerGroup audioMixerEffects;
        public AudioMixerGroup audioMixerMusic;
        
        /// <inheritdoc cref="SoundController.PlaySound"/>
        public void PlaySound(AudioClip clip, Transform spawnTransform, AudioType audioType)
        {
            AudioSource source = Instantiate(audioSourcePrefab, spawnTransform);
            
            source.clip = clip;

            source.outputAudioMixerGroup = audioType switch
            {
                AudioType.Effects => audioMixerEffects,
                AudioType.Music => audioMixerMusic,
                _ => throw new Exception("Invalid audio type")
            };

            float clipLength = source.clip.length + source.clip.length * 0.1f; //Slightly longer, 'cause sometimes it cuts off the end of the clip.
            
            source.Play();
            
            Destroy(source.gameObject, clipLength);
        }
    }
}