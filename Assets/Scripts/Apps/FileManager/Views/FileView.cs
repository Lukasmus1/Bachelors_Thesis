using TMPro;
using UnityEngine;

namespace Apps.FileManager.Views
{
    public class FileView : MonoBehaviour
    {
        [SerializeField] private TMP_Text fileName;
        [SerializeField] private GameObject loadFileButton;
        
        public void SetProps(string fName)
        {
            fileName.text = fName;
        }

        public void OnClick()
        {
            loadFileButton.SetActive(true);
        }
    }
}