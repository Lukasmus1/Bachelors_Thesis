using System;
using TMPro;
using UnityEngine;

namespace Desktop.Views
{
    public class TimeScript : MonoBehaviour
    {
        private TMP_Text _timeText;

        private void Start()
        {
            //Getting the time text component
            _timeText = GetComponent<TMP_Text>();
        }

        private void FixedUpdate()
        {
            //Updating the time text every fixed update
            _timeText.text = $"{DateTime.Now.TimeOfDay.Hours:D2}:{DateTime.Now.TimeOfDay.Minutes:D2}\n{DateTime.Now:dd/MM/yyyy}";
        }
    }
}
