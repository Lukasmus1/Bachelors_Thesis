using FourthWall.UserInformation.Models;

namespace FourthWall.UserInformation.Controllers
{
    public class UserInformationController
    {
        private readonly UserInformationModel userModel = new();
        
        /// <summary>
        /// Gets the real name of the user saved in the system.
        /// </summary>
        /// <returns>String of user's real name</returns>
        public string GetUserRealName()
        {
            return userModel.UserRealName;
        }

        /// <summary>
        /// Gets the real full name of the user saved in the system.
        /// </summary>
        /// <returns>String of the user's real full name</returns>
        public string GetUserRealFullName()
        {
            return userModel.UserRealFullName;
        }
    }
}