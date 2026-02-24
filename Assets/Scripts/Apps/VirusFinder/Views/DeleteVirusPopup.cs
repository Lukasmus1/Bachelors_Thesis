using System;
using Apps.VirusFinder.Commons;
using Desktop.Commons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Apps.VirusFinder.Views
{
    public class DeleteVirusPopup : MonoBehaviour
    {
        [SerializeField] private GameObject confirmButton;
        private GameObject virusToDelete;

        /// <summary>
        /// Sets the virus GameObject into the context for deletion.
        /// </summary>
        /// <param name="virus">GameObject of the virus</param>
        public void SetVirusIntoContext(GameObject virus)
        {
            virusToDelete = virus;
            EventSystem.current.SetSelectedGameObject(confirmButton);
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

        private void OnDisable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(tag, false);
        }
    }
}