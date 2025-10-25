using System.Collections.Generic;
using Apps.FileManager.Commons;
using Desktop.Commons;
using UnityEngine;

namespace Apps.FileManager.Views
{
    public class FileLoaderView : MonoBehaviour
    {
        [Header("Loaded Files")]
        [SerializeField] private GameObject filePrefab;
        [SerializeField] private Transform fileParent;
        
        [SerializeField] private GameObject openFileButton;

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
            LoadLoadedFiles();
        }

        private void OnDisable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
            openFileButton.SetActive(false);
        }

        private void LoadLoadedFiles()
        {
            List<GameObject> files = FileLoaderMvc.Instance.FileLoaderController.GetLoadedFile();
            foreach (GameObject file in files)
            {
                GameObject fileIcon = Instantiate(filePrefab, fileParent);
                var fileView = fileIcon.GetComponent<FileView>();
                
                fileView.SetProps(file, openFileButton);
                fileIcon.SetActive(true);
            }
        }
    }
}
