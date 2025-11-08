using System.Collections.Generic;
using Apps.FileManager.Commons;
using Apps.FileManager.Models;
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
                string fileName = file.GetComponent<FileModel>().FileName;
                if (FileLoaderMvc.Instance.FileLoaderController.InstantiatedFileNames.Contains(fileName))
                {
                    continue;
                }
                
                GameObject fileIcon = Instantiate(filePrefab, fileParent);
                var fileView = fileIcon.GetComponent<FileView>();
                
                fileView.SetProps(file, openFileButton);
                FileLoaderMvc.Instance.FileLoaderController.InstantiatedFileNames.Add(fileName);
                fileIcon.SetActive(true);
            }
        }
    }
}
