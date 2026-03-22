using Sounds.Controllers;
using UnityEngine;

namespace Sounds.Views
{
    public class SoundView : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSourcePrefab;
        
        /// <inheritdoc cref="SoundController.PlaySound"/>
        public void PlaySound(AudioClip clip, Transform spawnTransform)
        {
            AudioSource source = Instantiate(audioSourcePrefab, spawnTransform);
            
            source.clip = clip;
            
            float clipLength = source.clip.length + source.clip.length * 0.1f; //Slightly longer, 'cause sometimes it cuts off the end of the clip.
            
            source.Play();
            
            Destroy(source.gameObject, clipLength);
        }
    }
}