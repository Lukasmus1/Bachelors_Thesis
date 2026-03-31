using System.Collections;
using Apps.Commons;
using Apps.VirusFinder.Commons;
using Desktop.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.VirusFinder.Views
{
    public class VirusFinderView : AppsCommon
    {
        [SerializeField] private Slider scanProgressBar;
        [SerializeField] private TMP_Text percentageText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private GameObject virusFinderIcon;

        private bool killScanCoroutine = false;
        private bool killStatusTextCoroutine = false;

        private void OnEnable()
        {
            resultText.text = "";
            scanProgressBar.value = 0;
            percentageText.text = "0%";
            resultText.gameObject.SetActive(false);

            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }

        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);

            killStatusTextCoroutine = true;
            killScanCoroutine = true;
        }

        /// <summary>
        /// Sets the active state of the Virus Finder game object.
        /// </summary>
        /// <param name="active">Should the object be active.</param>
        public void SetActive(bool active)
        {
            virusFinderIcon.SetActive(active);
        }

        /// <summary>
        /// Starts the virus scanning process.
        /// </summary>
        public void StartScanning()
        {
            killScanCoroutine = false;
            killStatusTextCoroutine = false;
            StartCoroutine(Scan());
        }

        /// <summary>
        /// Coroutine to simulate scanning for viruses.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator Scan()
        {
            float duration = 2 + VirusFinderMvc.Instance.VirusFinderController.GetVirusesCount();
            var elapsed = 0f;

            StartCoroutine(FindingVirusTextProgress());

            scanProgressBar.value = 0;
            percentageText.text = "0%";
            resultText.gameObject.SetActive(true);

            while (elapsed < duration && !killScanCoroutine)
            {
                elapsed += Time.deltaTime;

                float progress = (elapsed / duration) * 100;

                percentageText.text = $"{(int)progress}%";
                scanProgressBar.value = progress;

                yield return null;
            }

            if (killScanCoroutine)
            {
                killStatusTextCoroutine = true;
                yield break;
            }

            int virusCount = VirusFinderMvc.Instance.VirusFinderController.FindViruses();

            killStatusTextCoroutine = true;
            scanProgressBar.value = 100;
            percentageText.text = "100%";
            resultText.text = 
                virusCount switch
                {
                    0 => "No viruses found.",
                    1 => $"Found {virusCount} virus.",
                    _ => $"Found {virusCount} viruses."
                };
            resultText.color = virusCount == 0 ? Color.darkGreen : Color.darkRed;
        }

        /// <summary>
        /// Coroutine to update the "Searching for viruses..." text with animated dots.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator FindingVirusTextProgress()
        {
            resultText.color = Color.black;
            resultText.text = "Scanning for viruses";
            float elapsed = 0f;
            int dotCount = 0;
            while (!killStatusTextCoroutine)
            {
                if (elapsed >= 1f)
                {
                    resultText.text = "Scanning for viruses";
                    for (int i = 0; i < dotCount; i++)
                    {
                        resultText.text += ".";
                    }

                    dotCount = (dotCount + 1) % 4;

                    elapsed = 0f;
                }
                else
                {
                    elapsed += Time.deltaTime;
                }

                yield return null;
            }
        }
    }
}