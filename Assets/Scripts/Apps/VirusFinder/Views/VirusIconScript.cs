using System;
using Desktop.Views;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Apps.VirusFinder.Views
{
    public class VirusIconScript : IconScript, IIconAction
    {
        [SerializeField] private GameObject deletePopup;
        private bool isOpened;

        private void Update()
        {
            if (!Keyboard.current.deleteKey.wasPressedThisFrame ||
                EventSystem.current.currentSelectedGameObject != gameObject)
            {
                return;
            }

            isOpened = false;
            
            //ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            //This will get called only when delete key is pressed
            PerformAction();
            
            isOpened = true;
        }

        //ReSharper disable Unity.PerformanceAnalysis
        public void PerformAction()
        {
            EventSystem.current.SetSelectedGameObject(null);
            
            GameObject popup = Instantiate(deletePopup, gameObject.transform.parent.parent);
            
            popup.GetComponent<DeleteVirusPopup>().SetVirusIntoContext(gameObject);

            popup.GetComponentInChildren<TMP_Text>().text = isOpened ? "This file is corrupted, do you want to <color=red>delete</color> it?" : "Are you sure you want to <color=red>delete</color> this file?";
        }
    }
}