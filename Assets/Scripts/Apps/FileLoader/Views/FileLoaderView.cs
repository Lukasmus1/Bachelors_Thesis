using System;
using System.Collections.Generic;
using Apps.FileLoader.Commons;
using UnityEngine;

namespace Apps.FileLoader.Views
{
    public class FileLoaderView : MonoBehaviour
    {
        [Header("Loaded Files")]
        [SerializeField] private GameObject filePrefab;
        [SerializeField] private Transform fileParent;

        private void OnEnable()
        {
            LoadLoadedFiles();
        }

        private void LoadLoadedFiles()
        {
            List<GameObject> files = FileLoaderMvc.Instance.FileLoaderController.GetLoadedFile();
        }
    }
}
