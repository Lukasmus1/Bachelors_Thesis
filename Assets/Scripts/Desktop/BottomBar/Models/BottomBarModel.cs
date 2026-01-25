using System.Collections.Generic;
using Desktop.BottomBar.Views;
using UnityEngine;

namespace Desktop.BottomBar.Models
{
    public class BottomBarModel
    {
        public Dictionary<GameObject, BottomBarItemView> Icons { get; set; } = new();
    }
}