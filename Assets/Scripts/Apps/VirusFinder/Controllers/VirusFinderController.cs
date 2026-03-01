using System;
using System.Collections.Generic;
using Apps.VirusFinder.Models;
using Apps.VirusFinder.Views;
using UnityEngine;

namespace Apps.VirusFinder.Controllers
{
    public class VirusFinderController
    {
        private readonly VirusFinderModel _model = new();
        private VirusFinderView _view;
        
        public Action<int> onVirusesCountChanged;
        
        /// <summary>
        /// Sets the view class for this controller.
        /// </summary>
        /// <param name="view">The view class</param>
        public void SetView(VirusFinderView view)
        {
            _view = view;
        }
        
        /// <summary>
        /// Creates a random number of viruses from 0 up to maxViruses.
        /// </summary>
        /// <param name="maxViruses">Max number of viruses</param>
        public void CreateRandomViruses(int maxViruses)
        {
            _model.CreateRandomViruses(maxViruses);
            onVirusesCountChanged?.Invoke(GetVirusesCount());
        }

        /// <summary>
        /// Returns the number of viruses currently present.
        /// </summary>
        /// <returns>Number of viruses present in the system</returns>
        public int GetVirusesCount()
        {
            return _model.Viruses.Count;
        }

        /// <summary>
        /// Enables viruses on the desktop and returns the number of viruses present.
        /// </summary>
        /// <returns>Number of viruses</returns>
        public int FindViruses()
        {
            _model.EnableViruses();
            return GetVirusesCount();
        }

        /// <summary>
        /// Enables the Virus Finder app.
        /// </summary>
        /// <param name="active">Should the app be active</param>
        public void EnableApp(bool active)
        {
            _view.SetActive(active);
        }

        /// <summary>
        /// Deletes a virus from the system.
        /// </summary>
        public void DeleteVirus(GameObject virus)
        {
            _model.DeleteVirus(virus);
            onVirusesCountChanged?.Invoke(GetVirusesCount());
        }
    }
}