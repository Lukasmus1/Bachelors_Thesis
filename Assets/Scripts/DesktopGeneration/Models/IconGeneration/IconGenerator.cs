using System.Collections.Generic;
using UnityEngine;

namespace DesktopGeneration.Models
{
    public abstract class IconGenerator
    {
        public abstract List<IconClass> GenerateIcons();
    }
}