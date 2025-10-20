using System;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.ChatTerminal.Views
{
    public class ContactView : MonoBehaviour
    {
        [SerializeField] private GameObject messagesWindow;
        
        //Contact properties
        private Image profileImage;
        private TMP_Text usernameText;
        private TMP_Text statusText;
        
        private ChatProfile profileModel;

        public void SetProperties(GameObject messagesWin, TMP_Text usernameTxt, TMP_Text statusTxt, Image profileImg)
        {
            messagesWindow = messagesWin;
            usernameText = usernameTxt;
            statusText = statusTxt;
            profileImage = profileImg;
            
            profileModel = GetComponent<ChatProfile>();
            
            var props = GetComponent<ContactProperties>();
            
            props.usernameText.text = profileModel.Username;
            props.statusText.text = profileModel.Status;
            
            gameObject.SetActive(true);
        }

        public void OnClick()
        {
            //profileImage.sprite = _profile.ProfilePicture;
            usernameText.text = profileModel.Username;
            statusText.text = profileModel.Status;
            
            messagesWindow.SetActive(true);
        }
    }
}