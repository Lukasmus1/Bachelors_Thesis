using System.Collections.Generic;
using Apps.VirusFinder.Models;
using UnityEngine;

namespace Apps.VirusFinder.Controllers
{
    public class VirusFinderController
    {
        private readonly VirusFinderModel _model = new();

        /// <summary>
        /// Creates a random number of viruses from 0 up to maxViruses.
        /// </summary>
        /// <param name="maxViruses">Max number of viruses</param>
        public void CreateRandomViruses(int maxViruses)
        {
            _model.CreateRandomViruses(maxViruses);
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
        /// <param name="virusParent">Parent transform for the virus icons</param>
        /// <returns>Number of viruses</returns>
        public int FindViruses(Transform virusParent)
        {
            _model.EnableViruses(virusParent);
            return GetVirusesCount();
        }
    }
}