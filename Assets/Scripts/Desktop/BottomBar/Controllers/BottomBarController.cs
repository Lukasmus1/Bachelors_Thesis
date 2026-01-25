using System.Collections.Generic;
using Desktop.BottomBar.Models;
using Desktop.BottomBar.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Desktop.BottomBar.Controllers
{
    public class BottomBarController
    {
        private readonly BottomBarModel _model = new();
        private BottomBarView _bottomBarView;
        
        public void SetBottomBarView(BottomBarView view)
        {
            _bottomBarView = view;
        }
        
        public GameObject CreateNewBottomBarIcon(GameObject objectToReference)
        {
           return _bottomBarView.CreateNewIcon(objectToReference);
        }

        public void SetPropsToModel(BottomBarItemModel model, GameObject openedApp)
        {
            model.OpenedApp = openedApp;
        }

        public GameObject GetOpenedApp(BottomBarItemModel model)
        {
            return model.OpenedApp;
        }

        public void SetIcons(Dictionary<GameObject, BottomBarItemView> icons)
        {
            _model.Icons = icons;
        }
        
        public void AddIcon(GameObject icon)
        {
            _model.Icons.Add(icon, icon.GetComponent<BottomBarItemView>());
        }

        public Dictionary<GameObject, BottomBarItemView> GetIcons()
        {
            return _model.Icons;
        }
        
        public void HighlightIcon(GameObject icon)
        {
            _bottomBarView.HighlightIcon(icon);
        }
        
        public void DeleteBottomBarIcon()
        {
            
        }
    }
}