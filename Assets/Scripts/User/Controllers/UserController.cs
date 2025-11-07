using System;
using User.Models;

namespace User.Controllers
{
    public class UserController
    {
        private readonly UserModel _userModel = new();
        
        public string Username
        {
            get => _userModel.Username;
            set => _userModel.Username = value;
        }
        
        public DateTime GetStartDate()
        {
            return _userModel.StartDate;
        }
        
        public void InitUser()
        {
            _userModel.InitUser();
        }
    }
}