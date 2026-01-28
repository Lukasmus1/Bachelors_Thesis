using System;
using Apps.VirusFinder.Commons;
using Desktop.Commons;
using UnityEngine;

namespace Apps.VirusFinder.Views
{
    public class DeleteVirusPopup : MonoBehaviour
    {
        private GameObject virusToDelete;

        /// <summary>
        /// Sets the virus GameObject into the context for deletion.
        /// </summary>
        /// <param name="virus">GameObject of the virus</param>
        public void SetVirusIntoContext(GameObject virus)
        {
            virusToDelete = virus;
        }

        /// <summary>
        /// Deletes the virus when confirmed from the context and closes the popup.
        /// </summary>
        public void OnConfirmDelete()
        {
            VirusFinderMvc.Instance.VirusFinderController.DeleteVirus(virusToDelete);
            Destroy(gameObject);
        }
        
        /// <summary>
        /// Closes the popup without deleting the virus.
        /// </summary>
        public void OnCancelDelete()
        {
            Destroy(gameObject);
        }
    }
}