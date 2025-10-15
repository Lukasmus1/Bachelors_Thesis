using System.Collections.Generic;
using Desktop.Models;
using UnityEngine;

namespace DesktopGeneration.Models
{
    public abstract class IconGenerator
    {
        public abstract List<IconClass> GenerateIcons();
    }
}