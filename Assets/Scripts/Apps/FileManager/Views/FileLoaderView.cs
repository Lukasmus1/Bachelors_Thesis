using System;
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
        
        
        private void Awake()
        {
            FileLoaderMvc.Instance.FileLoaderController.onFilesUpdated += UpdateLoadedFiles;
        }

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
            UpdateLoadedFiles();
        }

        private void OnDisable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }

        private void OnDestroy()
        {
            FileLoaderMvc.Instance.FileLoaderController.onFilesUpdated += UpdateLoadedFiles;
        }

        /// <summary>
        /// Shows all files that are loaded in the file loader.
        /// </summary>
        private void UpdateLoadedFiles()
        {
            var files = FileLoaderMvc.Instance.FileLoaderController.GetLoadedFiles();
            foreach (GameObject file in files)
            {
                var fileModel = file.GetComponent<FileModel>();
                if (FileLoaderMvc.Instance.FileLoaderController.InstantiatedFileNames.Contains(fileModel.FileName) || !fileModel.IsLoaded)
                {
                    continue;
                }
                
                GameObject fileIcon = Instantiate(filePrefab, fileParent);
                var fileView = fileIcon.GetComponent<FileView>();
                
                fileView.SetProps(file);
                FileLoaderMvc.Instance.FileLoaderController.InstantiatedFileNames.Add(fileModel.FileName);
                fileIcon.SetActive(true);
            }
        }
    }
}
