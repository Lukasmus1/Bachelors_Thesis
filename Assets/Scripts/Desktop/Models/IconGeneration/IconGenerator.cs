using System.Collections.Generic;
using UnityEngine;

namespace Desktop.Models.IconGeneration
{
    public abstract class IconGenerator
    {
        public abstract List<IconClass> GenerateIcons(Vector3 prefabScale);
    }
}