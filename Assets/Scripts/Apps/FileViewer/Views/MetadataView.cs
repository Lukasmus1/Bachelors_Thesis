using System;
using Apps.FileViewer.Commons;
using Apps.FileViewer.Models;
using TMPro;
using UnityEngine;

namespace Apps.FileViewer.Views
{
    public class MetadataView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text authorText;
        [SerializeField] private TMP_Text commentsText;
        [SerializeField] private TMP_Text createdDateText;
        
        private void Awake()
        {
            var fileProps = FileViewerMvc.Instance.FileLoaderController.OpenedFile.GetComponent<GuideFileProps>();
            titleText.text = fileProps.title;
            authorText.text = fileProps.author;
            commentsText.text = fileProps.comments;
            createdDateText.text = fileProps.createdDate;
        }

        private void OnEnable()
        {
            FileViewerMvc.Instance.FileLoaderController.MetadataOpened?.Invoke(titleText.text);
        }
    }
}