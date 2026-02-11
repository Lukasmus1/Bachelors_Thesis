using System.Collections.Generic;
using Apps.Commons;
using Apps.FileManager.Commons;
using Apps.FileManager.Controllers;
using Apps.FileManager.Models;
using Desktop.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.FileManager.Views
{
    public class FileLoaderView : AppsCommon
    {
        [Header("Loaded Files")]
        [SerializeField] private GameObject filePrefab;
        [SerializeField] private Transform fileParent;

        [SerializeField] private Toggle showHiddenFilesToggle;  
        private bool _showHiddenFilesBool;

        private List<GameObject> _fileIcons = new();
        
        private void Awake()
        {
            _showHiddenFilesBool = showHiddenFilesToggle.isOn;
            FileLoaderMvc.Instance.FileLoaderController.onFilesUpdated += UpdateLoadedFiles;
        }

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
            UpdateLoadedFiles();
        }
        

        protected override void OnDisableChild()
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
            List<GameObject> files = FileLoaderMvc.Instance.FileLoaderController.GetLoadedFiles();
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
                
                _fileIcons.Add(fileIcon);
            }
        }

        /// <summary>
        /// Toggles visibility of hidden files.
        /// </summary>
        public void ToggleHiddenFiles()
        {
            List<GameObject> files = FileLoaderMvc.Instance.FileLoaderController.GetLoadedFiles();
            foreach (GameObject file in files)
            {
                var fileModel = file.GetComponent<FileModel>();

                if (!fileModel.IsLoaded)
                {
                    continue;
                }
                
                //Get the icon associated with the file
                GameObject fileIcon = _fileIcons.Find(icon => icon.GetComponentInChildren<TMP_Text>().text == file.GetComponent<FileModel>().FileName);
                fileIcon.SetActive(!fileModel.IsHidden || showHiddenFilesToggle.isOn);
            }
            
            _showHiddenFilesBool = showHiddenFilesToggle.isOn;
        }
        
        /// <see cref="FileLoaderController.ToggleFileVisibility"/> 
        public void ToggleFileVisibility(string fileName, bool shouldHide)
        {
            List<GameObject> files = FileLoaderMvc.Instance.FileLoaderController.GetLoadedFiles();

            GameObject file = files.Find(file => file.GetComponent<FileModel>().FileName == fileName);
            
            file.GetComponent<FileModel>().IsHidden = shouldHide;
            
            // Sets the alpha of hidden file icons to 0.5 and 1 for visible files.
            GameObject fileIcon = _fileIcons.Find(icon => icon.GetComponentInChildren<TMP_Text>().text == fileName);
            Color fileColor = fileIcon.GetComponent<Image>().color;
            fileColor.a = shouldHide ? 0.5f : 1f;
            fileIcon.GetComponent<Image>().color = fileColor;
            
            fileIcon.SetActive(_showHiddenFilesBool || !shouldHide);
        }
    }
}
