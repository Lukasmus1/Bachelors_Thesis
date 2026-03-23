using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using Desktop.Abstracts;
using Desktop.Models;
using Desktop.Models.IconGeneration;
using Desktop.Notification.Commons;
using Desktop.Notification.Models;
using Desktop.Views;
using TMPro;
using UnityEngine;

namespace Desktop.Controllers
{
    public class DesktopGeneratorController
    {
        private WallpaperGeneration _wallpaperGenerator;
        private ColorGeneration _colorSchemeGenerator;
        private IconGenerator _iconGenerator;
        private readonly IconGenerationHelper _iconGeneratorHelper = new();
        private DesktopGeneratorView _desktopGeneratorView;

        private readonly Icons _icons = new();
        
        //Random Generators
        
        /// <summary>
        /// Gets a random wallpaper from the Resources folder.
        /// </summary>
        /// <returns>Random wallpaper from the resources folder</returns>
        public Texture2D GetRandomWallpaper()
        {
            _wallpaperGenerator = new WallpaperGeneratorRandom();
            return _wallpaperGenerator.GetWallpaperTexture();
        }
        
        /// <summary>
        /// Gets a random color scheme.
        /// </summary>
        /// <returns>Random color scheme</returns>
        public Color GetRandomColorScheme()
        {
            _colorSchemeGenerator = new ColorGeneration();
            return _colorSchemeGenerator.GenerateRandomColorScheme();
        }

        /// <summary>
        /// Generates a random icon position on the desktop.
        /// </summary>
        /// <param name="iconSize">Relative size of the icon</param>
        /// <returns>Random position on the desktop</returns>
        public Vector2 GenerateRandomIconPosition(Vector2 iconSize)
        {
            return _iconGeneratorHelper.GenerateRandomIconPosition(iconSize);
        }

        public List<IconClassOnObject> Icons
        {
            get => _icons.IconsOnObjects;
            set => _icons.IconsOnObjects = value;
        }

        /// <summary>
        /// This is a helper for clearing flags. Used when loading a save.
        /// </summary>
        public static void ClearFlags()
        {
            DesktopModel.Instance.flags.Clear();
        }
        
        //User's Generators
        /// <summary>
        /// Gets the user's wallpaper.
        /// </summary>
        /// <returns>User's wallpaper</returns>
        public Texture2D GetUserWallpaper()
        {
            _wallpaperGenerator = new WallpaperGeneratorUser();
            return _wallpaperGenerator.GetWallpaperTexture();
        }

        /// <summary>
        /// Gets the user's color scheme.
        /// </summary>
        /// <returns>Color scheme of the user's computer.</returns>
        public Color GetUserColorScheme()
        {
            _colorSchemeGenerator = new ColorGenerationUser();
            return _colorSchemeGenerator.GenerateUserColorScheme();
        }
        
        /// <summary>
        /// Gets all the icons on the user's desktop.
        /// </summary>
        /// <param name="prefabScale">Relative size of the icon</param>
        /// <returns>List of all icons on the desktop</returns>
        public List<IconClass> GetUserIcons(Vector3 prefabScale)
        {
            _iconGenerator = new IconGeneratorUser();
            return _iconGenerator.GenerateIcons(prefabScale);
        }
        
        /// <summary>
        /// Gets the user's font.
        /// </summary>
        /// <returns>User's set font</returns>
        public TMP_FontAsset GetUserFont()
        {
            var fontScript = new FontScript();
            return fontScript.GetUserFont();
        }

        //Commons
        
        /// <summary>
        /// Sets a flag for each app, so I can't open it twice for example.
        /// </summary>
        /// <param name="flag">Name of the app</param>
        /// <param name="value">True if app is open, False if not</param>
        public void SetDesktopFlag(string flag, bool value)
        {
            DesktopModel.Instance.SetFlag(flag, value);
        }

        /// <summary>
        /// Adds an IconClassOnObject to the Icons list in the desktop context.
        /// </summary>
        /// <param name="iconClass">IconClassOnObject instance</param>
        public void AddIconClassToContext(IconClassOnObject iconClass)
        {
            Icons.Add(iconClass);
        }

        /// <summary>
        /// Saves the IconClassOnObject list to the desktop model's Icons list.
        /// </summary>
        public void SaveIcons()
        {
            foreach (IconClassOnObject icon in Icons)
            {
                IconClass iconClass = DesktopModel.Instance.Icons.FirstOrDefault(x => x.Name == icon.IconName);
                if (iconClass != null)
                {
                    DesktopModel.Instance.Icons.Remove(iconClass);
                }
            
                DesktopModel.Instance.Icons.Add(new IconClass(icon));
            }
        }

        public void SetDesktopView(DesktopGeneratorView view)
        {
            _desktopGeneratorView = view;
        }
        
        /// <summary>
        /// Uses icon's IconClass.name to enable or disable the icon. 
        /// </summary>
        /// <param name="name">GameObject name</param>
        /// <param name="active">Should it be enabled?</param>
        public void ToggleIcon(string name, bool active)
        {
            DesktopModel.Instance.ToggleIcon(name, active);
            
            _desktopGeneratorView.ToggleIcon(name, active);

            if (active)
            {
                NotificationMvc.Instance.NotificationController.InstantiateNotification(NotificationType.NewApp, "New App Downloaded: " + name);
            }
        }

        /// <summary>
        /// Closes all apps
        /// </summary>
        public void CloseAllApps()
        {
            List<GameObject> linker = Tools.GetScriptReferenceLinker().GetAllApps();

            foreach (GameObject obj in linker.Where(obj => obj.activeSelf))
            {
                obj.SetActive(false);
            }
        }

        /// <summary>
        /// Closes a specific app
        /// </summary>
        /// <param name="appName">GameObject name of the app to be closed</param>
        public void CloseApp(string appName)
        {
            List<GameObject> linker = Tools.GetScriptReferenceLinker().GetAllApps();

            GameObject obj = linker.FirstOrDefault(obj => obj.name == appName);
            if (obj == null)
            {
                throw new Exception("App not found: " + appName);
            }
            
            obj.SetActive(false);
        }
    }
}