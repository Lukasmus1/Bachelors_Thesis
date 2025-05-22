using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DesktopGeneration
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
            _timeText.text = $"{DateTime.Now.TimeOfDay.Hours}:{DateTime.Now.TimeOfDay.Minutes}\n{DateTime.Now:dd/MM/yyyy}";
        }
    }
}
