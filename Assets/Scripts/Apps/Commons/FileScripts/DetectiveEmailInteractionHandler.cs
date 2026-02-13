using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using User.Commons;
using User.Models;

namespace Apps.Commons.FileScripts
{
    public class DetectiveEmailInteractionHandler : MonoBehaviour
    {
        private TMP_Text _textComponent;
        
        private void Awake()
        {
            _textComponent = GetComponentInChildren<TMP_Text>();

            _textComponent.text = _textComponent.text.Replace("{birthYear}", UserMvc.Instance.UserController.ProceduralData(UserDataType.PictureCodeYear));
        }
    }
}
