using UnityEngine;

namespace Apps.FileManager.Models
{
    public class FileModel : MonoBehaviour
    {
        [SerializeField] private string fileName;
        public string FileName
        {
            get => fileName;
            set => fileName = value;
        }
        
        public bool IsLoaded { get; set; } = false;
        
        public bool IsHidden { get; set; } = false;
    }
}