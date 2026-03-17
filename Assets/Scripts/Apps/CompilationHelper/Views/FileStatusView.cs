using System;
using System.IO;
using Apps.CompilationHelper.Models;
using Apps.CompilationHelper.Models.ScatteredFilesActions;
using FourthWall.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.CompilationHelper.Views
{
    public class FileStatusView : MonoBehaviour
    {
        [SerializeField] private FileEnum fileEnum;
        [SerializeField] private TMP_Text fileNameText;
        [SerializeField] private Button openFolderButton;
        [SerializeField] private Image fileImage;
        
        private string fileLocation;
        private string folderLocation;

        private FileAction fileAction;
        
        private void Awake()
        {
            fileLocation = FourthWallMvc.Instance.CompilationSimulationController.GetScatteredFileLocation(fileEnum);
            folderLocation = Path.GetDirectoryName(fileLocation);
            fileNameText.text = Path.GetFileName(fileLocation);
         
            fileAction = fileEnum switch
            {
                FileEnum.DesktopFile => new FileOneAction(fileLocation),
                FileEnum.AudioFile => new FileTwoAction(fileLocation),
                FileEnum.FileThree => new FileThreeAction(fileLocation),
                _ => throw new ArgumentOutOfRangeException()
            };
            fileAction.OnLoad();
            
            fileAction.onDeleteFile += FileDeleted; 
        }

        public void OpenFolderButton()
        {
            FourthWallMvc.Instance.FileGenerationController.OpenFileExplorer(folderLocation);
        }

        private void FileDeleted()
        {
            openFolderButton.interactable = false;
            fileImage.color = Color.green;
            FourthWallMvc.Instance.FileGenerationController.DestroyFolder(folderLocation); //Cleanup
            fileAction.onDeleteFile -= FileDeleted;
        }
    }
}