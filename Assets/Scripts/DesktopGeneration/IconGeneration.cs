using System.Collections.Generic;
using UnityEngine;

namespace DesktopGeneration
{
    public class IconGeneration
    {
        private List<Sprite> _iconSprites;
        private List<GameObject> _bottomBarIconObjects;
        public IconGeneration(List<Sprite> iconSprites, List<GameObject> bottomBarIconObjects)
        {
            _iconSprites = iconSprites;
            _bottomBarIconObjects = bottomBarIconObjects;
        }

        public IconGeneration(){}

        public void GenerateUserDesktopIcons()
        {
            
        }
    }
}
