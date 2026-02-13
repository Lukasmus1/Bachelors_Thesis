
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace FourthWall.UserInformation.Models
{
    public class UserInformationModel
    {
        private enum ExtendedNameFormat
        {
            NameDisplay = 3,
            NameGivenName = 13,
            NameSurname = 14
        }

        // ReSharper disable once StringLiteralTypo
        [DllImport("secur32.dll", CharSet = CharSet.Auto)]
        public static extern int GetUserNameEx(int nameFormat, StringBuilder userName, ref uint userNameSize);

        private string userRealName;
        public string UserRealName
        {
            get
            {
                if (string.IsNullOrEmpty(userRealName))
                {
                    userRealName = GetUserRealNameInformation(ExtendedNameFormat.NameGivenName);
                }
                return userRealName;
            }
        }

        private string userRealFullName;
        public string UserRealFullName
        {
            get
            {
                if (string.IsNullOrEmpty(userRealFullName))
                {
                    userRealFullName = GetUserRealNameInformation(ExtendedNameFormat.NameDisplay);
                }
                return userRealFullName;
            }
        }

        /// <summary>
        /// /// Gets the user's real name information based on the specified name format.
        /// </summary>
        /// <param name="nameFormat">Format of the desired information</param>
        /// <returns>User's real name information</returns>
        private string GetUserRealNameInformation(ExtendedNameFormat nameFormat)
        {
            var userName = new StringBuilder(1024);
            var userNameSize = (uint)userName.Capacity;
            
            int result = GetUserNameEx((int)nameFormat, userName, ref userNameSize);

            if (result != 0)
            {
                return userName.ToString().ToLower();
            }
            else
            {
                Debug.LogError("No name could be retrieved for the user.");
                return "John Doe";
            }
        }
    }
}