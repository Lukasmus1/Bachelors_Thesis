using System;
using Sounds.Controllers;
using UnityEngine;
using UnityEngine.Audio;
using AudioType = Sounds.Models.AudioType;

namespace Sounds.Views
{
    public class SoundView : MonoBehaviour
    {
        [Header("General Audio Settings")]
        [SerializeField] private AudioSource audioSourcePrefab;
        public AudioMixerGroup audioMixerEffects;
        public AudioMixerGroup audioMixerMusic;
        
        [Header("Background Music Settings")]
        [SerializeField] private AudioClip backgroundMusicClip;
        [NonSerialized] public AudioSource bgmAudioSource;
        
        private void Awake()
        {
            bgmAudioSource = PlaySound(backgroundMusicClip, transform, AudioType.Music, true);
        }

        /// <inheritdoc cref="SoundController.PlaySound"/>
        public AudioSource PlaySound(AudioClip clip, Transform spawnTransform, AudioType audioType, bool loop = false)
        {
            AudioSource source = Instantiate(audioSourcePrefab, spawnTransform);
            
            source.clip = clip;

            source.outputAudioMixerGroup = audioType switch
            {
                AudioType.Effects => audioMixerEffects,
                AudioType.Music => audioMixerMusic,
                _ => throw new Exception("Invalid audio type")
            };

            source.loop = loop;
            
            source.Play();
            
            if (!loop)
            {
                float clipLength = source.clip.length + source.clip.length * 0.1f; //Slightly longer, 'cause sometimes it cuts off the end of the clip.
                Destroy(source.gameObject, clipLength);
            }
            
            return source;
        }
    }
}