using System;
using Apps.CompilationHelper.Models;
using FourthWall.Commons;
using UnityEngine;

namespace Apps.CompilationHelper.Views
{
    public class FileStatusView : MonoBehaviour
    {
        [SerializeField] private FileEnum fileEnum;

        private string fileLocation;
        
        private void Awake()
        {
            //fileLocation = FourthWallMvc.Instance.CompilationSimulationController.GetFileLocation(fileEnum);
        }
    }
}