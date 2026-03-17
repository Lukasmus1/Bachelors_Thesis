using System;
using Microsoft.Win32;
using UnityEngine;

namespace FourthWall.UserInformation.Models
{
    public class RegistryValueDetection : MonoBehaviour
    {
        private bool start = false;
        private Action _onValueMatch;

        private string keyPath;
        private string valueName;
        
        private int valueToCheck;
        
        /// <summary>
        /// Creates a new registry entry into the HKEY_CURRENT_USER\Software\ folder.
        /// </summary>
        /// <param name="keyFolder">Name of the folder to create the entry in</param>
        /// <param name="keyName">Name of the entry</param>
        /// <param name="value">Integer value of the entry</param>
        public void CreateRegistry(string keyFolder, string keyName, int value)
        {
            string fullKeyPath = @"Software\" + keyFolder;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(fullKeyPath);
            if (key == null)
            {
                throw new Exception("Couldn't create the registry key");
            }

            key.SetValue(keyName, value, RegistryValueKind.DWord);
                
            keyPath = fullKeyPath;
            valueName = keyName;
        }
        
        /// <summary>
        /// Starts the detection of the registry change/deletion.
        /// </summary>
        /// <param name="valueCheck">Value to check against</param>
        /// <param name="onValueMatch">Callback to what should happen on value match or deletion</param>
        public void StartDetection(int valueCheck, Action onValueMatch)
        {
            start = true;
            _onValueMatch += onValueMatch;
            valueToCheck = valueCheck;
        }

        /// <summary>
        /// Checks for the desired state of the registry key.
        /// </summary>
        /// <returns>Is the registry in a desired state?</returns>
        private bool IsRegistryDesired()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath);

            object value = key?.GetValue(valueName);
            if (value == null)
            {
                return true;
            }

            return value is int intValue && intValue == valueToCheck;
        }
        
        private void Update()
        {
            if (!start || !IsRegistryDesired()) return;
            
            start = false;
            _onValueMatch?.Invoke();
            
            Registry.CurrentUser.DeleteSubKey(keyPath, false);   
            Destroy(this);
        }
    }
}