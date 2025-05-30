using System.Collections.Generic;
using UnityEngine;

namespace DesktopGeneration.Abstracts
{
    public abstract class IconGeneration
    {
        protected List<GameObject> DesktopIconObjects;
        
        protected IconGeneration(List<GameObject> desktopIcons)
        {
            DesktopIconObjects = desktopIcons;
        }
        
        public abstract void GenerateIcons();
    }
}