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

        private readonly List<GameObject> _fileIcons = new();
        
        private void Awake()
        {
            _showHiddenFilesBool = showHiddenFilesToggle.isOn;
            FileManagerMvc.Instance.FileManagerController.onFilesUpdated += UpdateLoadedFiles;
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
            FileManagerMvc.Instance.FileManagerController.onFilesUpdated += UpdateLoadedFiles;
        }

        /// <summary>
        /// Shows all files that are loaded in the file loader.
        /// </summary>
        private void UpdateLoadedFiles()
        {
            List<GameObject> files = FileManagerMvc.Instance.FileManagerController.GetLoadedFiles();
            foreach (GameObject file in files)
            {
                var fileModel = file.GetComponent<FileModel>();
                if (FileManagerMvc.Instance.FileManagerController.InstantiatedFileNames.Contains(fileModel.FileName) || !fileModel.IsLoaded)
                {
                    continue;
                }
                
                GameObject fileIcon = Instantiate(filePrefab, fileParent);
                var fileView = fileIcon.GetComponent<FileView>();
                
                fileView.SetProps(file);
                FileManagerMvc.Instance.FileManagerController.InstantiatedFileNames.Add(fileModel.FileName);
                fileIcon.SetActive(true);
                
                _fileIcons.Add(fileIcon);

                // Set the visibility of the file icon based on whether the file is hidden and the toggle state.
                ToggleFileVisibility(fileModel.FileName, fileModel.IsHidden);
            }
        }

        /// <summary>
        /// Toggles visibility of hidden files.
        /// </summary>
        public void ToggleHiddenFiles()
        {
            List<GameObject> files = FileManagerMvc.Instance.FileManagerController.GetLoadedFiles();
            foreach (GameObject file in files)
            {
                ToggleHideFile(file);
            }
            
            _showHiddenFilesBool = showHiddenFilesToggle.isOn;
        }

        private void ToggleHideFile(GameObject file)
        {
            var fileModel = file.GetComponent<FileModel>();

            if (!fileModel.IsLoaded)
            {
                return;
            }
            
            // Get the icon associated with the file and set its visibility.
            GameObject fileIcon = _fileIcons.Find(icon => icon.GetComponentInChildren<TMP_Text>().text == file.GetComponent<FileModel>().FileName);
            fileIcon.SetActive(!fileModel.IsHidden || showHiddenFilesToggle.isOn);
        }
        
        /// <see cref="FileLoaderController.ToggleFileVisibility"/> 
        public void ToggleFileVisibility(string fileName, bool shouldHide)
        {
            List<GameObject> files = FileManagerMvc.Instance.FileManagerController.GetLoadedFiles();

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
