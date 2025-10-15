using System;
using Desktop.Models;
using UnityEngine;

namespace Saving.Models
{
    [Serializable]
    public class SaveModel
    {
        public DesktopModel desktop;
        
        public void LoadDataFromModel(SaveModel saveModel)
        {
            desktop = saveModel.desktop;
        }
    }
}
