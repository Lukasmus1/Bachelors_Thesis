using System;
using UnityEngine;

namespace User.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public Texture2D ProfilePicture { get; set; }
        public DateTime StartDate { get; set; }
        
        public void InitUser()
        {
            StartDate = DateTime.Now;
        }
    }
}