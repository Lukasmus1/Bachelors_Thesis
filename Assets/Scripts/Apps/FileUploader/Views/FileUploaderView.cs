using Apps.Commons;
using Apps.FileUploader.Commons;
using Desktop.Commons;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Apps.FileUploader.Views
{
    public class FileUploaderView : AppsCommon
    {
        [SerializeField] private TMP_InputField resultInputField;
        
        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
        }

        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }

        /// <summary>
        /// Loads a file from the user's system using a file picker dialog. It then sends the selected file's path to the controller for processing and outputs the result into an input field.
        /// </summary>
        public void LoadFile()
        {
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop); //Default path is desktop
            
            string chosenFile = EditorUtility.OpenFilePanel("Load File", desktopPath, "");

            if (string.IsNullOrEmpty(chosenFile))
            { 
                return;
            }
            
            string res = FileUploaderMvc.Instance.FileUploaderController.HandleFileUpload(chosenFile);
            if (res == null)
            {
                resultInputField.text = "Unknown File";
            }
            
            resultInputField.text = res;
        }
    }
}