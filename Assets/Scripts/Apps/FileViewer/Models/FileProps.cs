using UnityEngine;
using User.Commons;

namespace Apps.FileViewer.Models
{
    public abstract class FileProps : MonoBehaviour
    {
        public string title;
        public string author;
        public string comments = UserMvc.Instance.UserController.GetStartDate().ToString("yyyy-MM-dd HH:mm:ss");
        public string createdDate;
    }
}