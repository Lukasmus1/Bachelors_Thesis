using System.Collections;
using System.Collections.Generic;
using Commons;
using Saving.Commons;
using Sounds.Commons;
using Sounds.Views;
using Story.Commons;
using Story.Models;
using UnityEngine;
using AudioType = Sounds.Models.AudioType;

namespace Jumpscare
{
    public class JumpscareView : MonoBehaviour
    {    
        [SerializeField] private SoundView soundView;
        
        [SerializeField] private AudioClip audioClip;
        private float _soundLength;

        [SerializeField] private GameObject jumpscareImage;
        private readonly Vector2 _targetScale = new(4000f, 4000f);
        private Vector2 _initialScale;
        private RectTransform _rectTransform;
        
        private void OnEnable()
        {
            SoundMvc.Instance.SoundController.SetView(soundView);
            _rectTransform = GetComponent<RectTransform>();
            _initialScale = _rectTransform.sizeDelta;
            _soundLength = audioClip.length;
            
            
            AsyncTimer t = new();
            _ = t.StartTimer(3f, () =>
            {
                SoundMvc.Instance.SoundController.PlaySound(audioClip, gameObject.transform, AudioType.Effects);
            
                jumpscareImage.SetActive(true);
            
                StartCoroutine(ScaleUpOverTime());
            });
        }
        
        private IEnumerator ScaleUpOverTime()
        {
            float elapsedTime = 0f;
            
            while (elapsedTime < _soundLength)
            {
                float t = elapsedTime / _soundLength;
                
                _rectTransform.sizeDelta = Vector3.Lerp(_initialScale, _targetScale, t);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            transform.localScale = _targetScale;
            
            StoryMvc.Instance.StoryController.SetEnding(Endings.FightForCuratorFail);
            SavingMvc.Instance.SavingController.QuitAndSaveGame();
        }
    }
}