using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using User.Commons;
using User.Models;

namespace Apps.Commons.FileScripts
{
    public class DetectiveMessagesInteractionHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text textWithName;
        
        private void Awake()
        {
            textWithName.text = textWithName.text.Replace("{name}", UserMvc.Instance.UserController.ProceduralData(UserDataType.ScammerName));
        }
    }
}
